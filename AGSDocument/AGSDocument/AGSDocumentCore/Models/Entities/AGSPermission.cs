using System;
using AGSDocumentCore.Models.Enums;

namespace AGSDocumentCore.Models.Entities
{
    public class AGSPermission
    {
        private string DepartmentId { get; set; }
        private AGSPermissionType PermissionType { get; set; }


        public AGSPermission()
        {
        }
    }
}
