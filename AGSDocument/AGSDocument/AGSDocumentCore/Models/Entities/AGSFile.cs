using System;
namespace AGSDocumentCore.Models.Entities
{
    public class AGSFile : AGSEntity
    {
        public string Name { get; private set; }
        public string FileExtension { get; private set; }
        public string Description { get; private set; }
        public int SizeInByte { get; private set; }
        public string FilePath { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public void UpdateFile(string name, string fileExtension, string description, int sizeInByte, string createdBy, DateTime createdDate)
        {
            this.Name = name;
            this.FileExtension = fileExtension;
            this.Description = description;
            this.SizeInByte = sizeInByte;
            this.CreatedBy = createdBy;
            this.CreatedDate = createdDate;
        }

        public AGSFile(string name, string description, string fileExtension, int sizeInByte, string filePath, string createdBy, DateTime createdDate)
        {
            this.Name = name;
            this.FileExtension = fileExtension;
            this.Description = description;
            this.SizeInByte = sizeInByte;
            this.FilePath = filePath;
            this.CreatedBy = createdBy;
            this.CreatedDate = createdDate;
        }
    }
}
