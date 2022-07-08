using System;
using System.IO;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Models.DTOs.Commands;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace AGSDocumentFileService
{
    public class FileService : IFileService
    {
        private BlobServiceClient _blobServiceClient;
        private BlobContainerClient _blobContainerClient;
        private readonly string _containerName;
        private readonly string _connectionString;

        public FileService(IConfiguration configuration)
        {
            _containerName = configuration["AzureBlob:ContainerName"];
            _connectionString = configuration["AzureBlob:ConnectionString"];
            _blobServiceClient = new BlobServiceClient(_connectionString);
            // _blobContainerClient = _blobServiceClient.CreateBlobContainerAsync(_containerName).Result;
        }

        public byte[] GetAGSFileContent(string filepath)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(filepath);
            var downloadResult = blobClient.DownloadContent();
            return downloadResult?.Value?.Content?.ToArray();
        }

        public void RemoveAGSFile(string filepath)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(filepath);
            blobClient.Delete();
        }

        public string UploadAGSFile(UploadAGSFileCommand uploadAGSFileRequest)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(uploadAGSFileRequest.Filename);
            using (var memoryStream = new MemoryStream(uploadAGSFileRequest.FileContent))
            {
                var uploadResult = blobClient.Upload(memoryStream);
                return uploadAGSFileRequest.Filename;
            }
        }
    }
}
