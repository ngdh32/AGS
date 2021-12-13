using System;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Interfaces.Repositories
{
    public interface IFolderRepository
    {
        public void Save(AGSFolder folder);
    }
}
