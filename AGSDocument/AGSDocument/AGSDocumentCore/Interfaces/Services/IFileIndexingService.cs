using System;
using System.Collections.Generic;

namespace AGSDocumentCore.Interfaces.Services
{
    public interface IFileIndexingService
    {
        public void FileIndexing(string filename, string fileContent);

        public List<string> FileSearchingByContent(string keyword);
    }
}
