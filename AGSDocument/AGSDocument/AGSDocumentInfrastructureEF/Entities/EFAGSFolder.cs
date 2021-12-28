using System;
using System.Collections.Generic;

namespace AGSDocumentInfrastructureEF.Entities
{
    public class EFAGSFolder : EFAGSEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ParentFolderId { get; set; }

        public ICollection<EFAGSFile> EFAGSFiles { get; set; }
        public ICollection<EFAGSFolderPermission> EFAGSFolderPermissions { get; set; }
    }
}
