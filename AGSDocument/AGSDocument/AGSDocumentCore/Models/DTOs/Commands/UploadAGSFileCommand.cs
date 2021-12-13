using System;
namespace AGSDocumentCore.Models.DTOs.Commands
{
    public record UploadAGSFileCommand(string filename, byte[] fileContent);
}
