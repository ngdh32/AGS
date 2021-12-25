using System;
using System.Collections.Generic;
using AGSDocumentCore.Models.Enums;

namespace AGSDocumentCore.Models.DTOs.Commands
{
    public class SetAGSFolderPermissionsCommand
    {
        public string FolderId { get; set; }
        public List<AGSFolderPermissionView> Permissions { get; init; }
    }

    public class AGSFolderPermissionView
    {
        public string DepartmentId { get; set; }
        public AGSPermissionType PermissionType { get; init; }
    }
}
