using System;
namespace AGSDocumentInfrastructureEF.Entities
{
    public class EFAGSFolderPermission : EFAGSEntity
    {
        public string FolderId { get; set; }
        public int PermissionType { get; set; }
        public string DepartmentId { get; set; }

        public EFAGSFolder Folder { get; set; }
    }
}
