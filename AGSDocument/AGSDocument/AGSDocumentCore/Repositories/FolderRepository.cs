using System;
using System.Collections.Generic;
using AGSDocumentCore.Interfaces.Repositories;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        public FolderRepository()
        {
        }

        public void DeleteFolder(string folderId)
        {
            throw new NotImplementedException();
        }

        public (AGSFile file, string folderId) GetFileById(string fileId)
        {
            throw new NotImplementedException();
        }

        public AGSFolder GetFolderById(string folderId)
        {
            throw new NotImplementedException();
        }

        public void SaveFile(AGSFile file)
        {
            throw new NotImplementedException();
        }

        public void SaveFolder(AGSFolder folder)
        {
            throw new NotImplementedException();
        }

        public List<string> SearchFilesByName(string keyword)
        {
            throw new NotImplementedException();
        }
    }
}
