using System;
namespace AGSDocumentCore.Interfaces.Services
{
    public interface IFileIndexingService
    {
        public void FileIndexing(string filename, string fileContent);

        public void FileSearchingByContent(string keyword);
    }
}
