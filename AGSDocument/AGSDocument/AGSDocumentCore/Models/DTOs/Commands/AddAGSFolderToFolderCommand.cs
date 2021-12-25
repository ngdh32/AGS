using System;
using System.Collections.Generic;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Models.DTOs.Commands
{
    public class AddAGSFolderToFolderCommand
    {
        public string ParentFolderId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string CreatedBy { get; init; }
        public List<AGSPermission> Permissions { get; init; }
    }
}
