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

        public void Save(AGSFolder folder)
        {
            throw new NotImplementedException();
        }
    }
}
