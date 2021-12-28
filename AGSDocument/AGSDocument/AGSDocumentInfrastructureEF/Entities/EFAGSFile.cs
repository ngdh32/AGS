using System;
namespace AGSDocumentInfrastructureEF.Entities
{
    public class EFAGSFile : EFAGSEntity
    {
        public string Name { get; set; }
        public string FileExtension { get; set; }
        public string Description { get; set; }
        public int SizeInByte { get; set; }
        public string FilePath { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string FolderId { get; set; }
        public EFAGSFolder EFAGSFolder { get; set; }
    }
}
