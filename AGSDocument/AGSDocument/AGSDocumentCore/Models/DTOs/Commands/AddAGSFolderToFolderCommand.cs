using System;
using System.Collections.Generic;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Models.DTOs.Commands
{
    public record AddAGSFolderToFolderCommand(string parentFolderId, string name, string description, string createdBy, List<AGSPermission> permissions)
}
