using System;
namespace AGSDocumentCore.Models.DTOs.Commands
{
    public class UpdateAGSFileCommand
    {
        public string FileId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string FileExtension { get; init; }
        public int SizeInByte{ get; init; }
        public string FilePath { get; init; }
        public string CreatedBy { get; init; }
    }
}