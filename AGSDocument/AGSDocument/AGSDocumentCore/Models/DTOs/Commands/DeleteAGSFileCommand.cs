using System;
namespace AGSDocumentCore.Models.DTOs.Commands
{
    public record DeleteAGSFileCommand(string fileId, string folderId);
}
