using System;
using System.Collections.Generic;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Models.DTOs.Commands
{
    public record UpdateAGSFolderCommand(string folderId, string name, string description, List<AGSPermission> permissions);
}
