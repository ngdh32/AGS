using System;
using AGSDocumentCore.Models.Enums;

namespace AGSDocumentCore.Models.Entities
{
    public class AGSPermission
    {
        public string Id { get; init; }
        public string DepartmentId { get; init; }
        public AGSPermissionType PermissionType { get; init; }

        public AGSPermission(string Id, string departmentId, int permissionType)
        {
            this.Id = Id;
            this.DepartmentId = departmentId;
            this.PermissionType = (AGSPermissionType)permissionType;
        }

        public AGSPermission()
        {
        }
    }
}
