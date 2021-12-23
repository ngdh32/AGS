using System;
namespace AGSDocumentCore.Models.DTOs.Commands
{
    public record UpdateAGSFileCommand(string fileId, string name, string description, string fileExtension, int sizeInByte, string filePath, string createdBy);

}