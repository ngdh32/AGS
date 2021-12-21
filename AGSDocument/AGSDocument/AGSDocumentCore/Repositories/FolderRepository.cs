using System;
using AGSDocumentCore.Interfaces.Repositories;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        public FolderRepository()
        {
        }

        public AGSFolder GetFolderById(string folderId)
        {
            throw new NotImplementedException();
        }

        public void SaveFolder(AGSFolder folder)
        {
            throw new NotImplementedException();
        }
    }
}
