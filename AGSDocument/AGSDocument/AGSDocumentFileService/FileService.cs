using System;
using System.IO;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Models.DTOs.Commands;
using Microsoft.Extensions.Configuration;

namespace AGSDocumentFileService
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;

        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public byte[] GetAGSFileContent(string filepath)
        {
            if (File.Exists(filepath))
                return File.ReadAllBytes(filepath);
            else
                return null;
        }

        public void RemoveAGSFile(string filepath)
        {
            if (File.Exists(filepath))
                File.Delete(filepath);
        }

        public string UploadAGSFile(UploadAGSFileCommand uploadAGSFileRequest)
        {
            var filepath = Path.Combine(_configuration["AGSDocumentBaseFolder"], uploadAGSFileRequest.Filename);
            File.WriteAllBytes(filepath, uploadAGSFileRequest.FileContent);
            return filepath;
        }
    }
}
