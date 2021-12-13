using System;
namespace AGSDocumentCore.Models.DTOs.Commands
{
    public record AddAGSFileToFolderCommand(string fileId, string folderId);
}
