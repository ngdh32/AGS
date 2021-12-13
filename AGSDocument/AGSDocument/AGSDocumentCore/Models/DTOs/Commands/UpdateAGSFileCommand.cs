using System;
namespace AGSDocumentCore.Models.DTOs.Commands
{
    public record UpdateAGSFileCommand(string folderId, string fileId, string name);
}
