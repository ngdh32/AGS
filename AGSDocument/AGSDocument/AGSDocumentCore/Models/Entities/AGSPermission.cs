using System;
using AGSDocumentCore.Models.Enums;

namespace AGSDocumentCore.Models.Entities
{
    public class AGSPermission
    {
        public string DepartmentId { get; init; }
        public AGSPermissionType PermissionType { get; init; }

        public AGSPermission(string departmentId, int permissionType)
        {
            this.DepartmentId = departmentId;
            this.PermissionType = (AGSPermissionType)permissionType;
        }

        public AGSPermission()
        {
        }
    }
}
