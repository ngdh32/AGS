using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AGSDocumentCore.Interfaces.Services;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Microsoft.Extensions.Configuration;

namespace AGSDocumentCore.Services
{
    public class FileIndexingService : IFileIndexingService
    {
        private readonly IConfiguration _configuration;
        private const LuceneVersion _appLuceneVersion = LuceneVersion.LUCENE_48;
        private const string _fileNameField = "filename";
        private const string _fileContentField = "filecontent";
        private const string _fileIdField = "fileId";

        public FileIndexingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void FileIndexing(string fileId, string filename, string fileContent)
        {
            // Construct a machine-independent path for the index
            var basePath = _configuration["AGSDocumentLuceneFolder"];
            var indexPath = Path.Combine(basePath, "index");

            using var dir = FSDirectory.Open(GetIndexFolderPath());

            // Create an analyzer to process the text
            var analyzer = new StandardAnalyzer(_appLuceneVersion);

            // Create an index writer
            var indexConfig = new IndexWriterConfig(_appLuceneVersion, analyzer);
            using var writer = new IndexWriter(dir, indexConfig);

            writer.AddDocument(new Document()
            {
                new StringField(_fileIdField, fileId, Field.Store.YES),
                new StringField(_fileNameField, filename, Field.Store.YES),
                new TextField(_fileContentField, fileContent, Field.Store.YES)
            }); ;

            writer.Flush(true, false);
        }

        public List<string> FileSearchingByContent(string keyword)
        {
            using var dir = FSDirectory.Open(GetIndexFolderPath());
            var analyzer = new StandardAnalyzer(_appLuceneVersion);
            var indexConfig = new IndexWriterConfig(_appLuceneVersion, analyzer);
            using var writer = new IndexWriter(dir, indexConfig);
            // Search with a phrase
            var queryParser = new QueryParser(_appLuceneVersion, _fileContentField, analyzer);
            Query query = queryParser.Parse(keyword);
            // Re-use the writer to get real-time updates
            using var reader = writer.GetReader(applyAllDeletes: true);
            var searcher = new IndexSearcher(reader);
            var hits = searcher.Search(query, 20 /* top 20 */).ScoreDocs;

            // Display the output in a table
            return hits.Select(x => searcher.Doc(x.Doc).Get(_fileIdField)).ToList();
        }

        private string GetIndexFolderPath() => Path.Combine(_configuration["AGSDocumentLuceneFolder"]);
    }
}
