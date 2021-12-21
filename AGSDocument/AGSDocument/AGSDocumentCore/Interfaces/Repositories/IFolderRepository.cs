using System;
using System.Collections.Generic;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Interfaces.Repositories
{
    public interface IFolderRepository
    {
        public AGSFolder GetFolderById(string folderId);

        public void SaveFolder(AGSFolder folder);

        public void DeleteFolder(string folderId);

        public (AGSFile file, string folderId) GetFileById(string fileId);

        public List<string> SearchFilesByName(string keyword);
    }
}
