using System;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Interfaces.Repositories
{
    public interface IFolderRepository
    {
        public AGSFolder GetById(string folderId);

        public void Save(AGSFolder folder);
    }
}
