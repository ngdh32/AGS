using System;
namespace AGSDocumentCore.Models.Entities
{
    public class AGSFile : AGSEntity
    {
        private string Name { get; set; }
        private string Description { get; set; }
        private int SizeInByte { get; set; }
        private string FilePath { get; set; }
        private string FolderId { get; set; }
        private string CreatedBy { get; set; }
        private DateTime CreatedDate { get; set; }

        public void UpdateFile()
        {

        }

        public AGSFile()
        {
        }
    }
}
