using System;
namespace AGSDocumentCore.Models.DTOs.Commands
{
    public class UploadAGSFileCommand
    {
        public string Filename { get; init; }
        public byte[] FileContent { get; init; }
    }
}
