using System;
namespace AGSDocumentCore.Models.DTOs.Commands
{
    public record AddAGSFileToFolderCommand(string folderId, string name, string fileExtension, string description, int sizeInBytes, string filepath, string createdBy);
}
