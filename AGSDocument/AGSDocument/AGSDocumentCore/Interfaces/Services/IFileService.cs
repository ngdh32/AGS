using System;
using AGSDocumentCore.Models.DTOs.Commands;

namespace AGSDocumentCore.Interfaces.Services
{
    public interface IFileService
    {
        public string UploadAGSFile(UploadAGSFileCommand uploadAGSFileRequest);

        public byte[] GetAGSFileContent(string filepath);

        public void RemoveAGSFile(string filepath);
    }
}
