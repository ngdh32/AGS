using System;
using AGSDocumentCore.Models.DTOs;

namespace AGSDocumentCore.Interfaces
{
    public interface IFileService
    {
        public string UploadAGSFile(UploadAGSFileCommand uploadAGSFileRequest);
    }
}
