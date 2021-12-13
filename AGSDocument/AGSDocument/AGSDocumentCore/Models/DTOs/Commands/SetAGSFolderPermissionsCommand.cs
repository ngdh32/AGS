using System;
using System.Collections.Generic;
using AGSDocumentCore.Models.Enums;

namespace AGSDocumentCore.Models.DTOs.Commands
{
    public record SetAGSFolderPermissionsCommand(string folderId, List<AGSFolderPermissionView> permissions);

    public record AGSFolderPermissionView(string departmentId, AGSPermissionType permissionType);
}
