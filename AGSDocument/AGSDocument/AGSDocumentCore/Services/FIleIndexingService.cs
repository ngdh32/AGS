using System;
using System.Collections.Generic;
using AGSDocumentCore.Interfaces.Services;

namespace AGSDocumentCore.Services
{
    public class FIleIndexingService : IFileIndexingService
    {
        public FIleIndexingService()
        {
        }

        public void FileIndexing(string filename, string fileContent)
        {
            throw new NotImplementedException();
        }

        public List<string> FileSearchingByContent(string keyword)
        {
            throw new NotImplementedException();
        }
    }
}
