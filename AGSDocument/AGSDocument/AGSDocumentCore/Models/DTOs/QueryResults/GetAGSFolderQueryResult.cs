using System;
using System.Collections.Generic;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Models.DTOs.QueryResults
{
    public class AGSFolderQueryView
    {
        public string FolderId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public DateTime CreatedDate { get; init; }
        public string CreatedUsername { get; init; }
        public List<AGSPermission> Permissions { get; init; }
        public List<AGSChildrenFolderQueryView> ChildrenFolders { get; init; }
        public List<AGSFileQueryView> Files { get; init; }
    }

    public class AGSChildrenFolderQueryView
    {
        public string FolderId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public DateTime CreatedDate { get; init; }
        public string CreatedUsername { get; init; }
        public List<AGSPermission> Permissions { get; init; }
    }

    public class AGSFileQueryView
    {
        public string FileId { get; init; }
        public string Description { get; init; }
        public int SizeInByte { get; init; }
        public DateTime CreatedDate { get; init; }
        public string CreatedUsername { get; init; }
    }
}
