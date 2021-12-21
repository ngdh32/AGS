using System;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Models.DTOs.Commands;

namespace AGSDocumentCore.Services
{
    public class AGSDocumentUpdateService : IAGSDocumentUpdateService
    {
        public AGSDocumentUpdateService()
        {
        }

        public void AddAGSFileToFolder(AddAGSFileToFolderCommand addAGSFileToFolderCommand)
        {
            throw new NotImplementedException();
        }

        public void CreateAGSFolder(CreateAGSFolderCommand createAGSFolderCommand)
        {
            throw new NotImplementedException();
        }

        public void DeleteAGSFile(DeleteAGSFileCommand deleteAGSFileCommand)
        {
            throw new NotImplementedException();
        }

        public void DeleteAGSFolder(string folderId)
        {
            throw new NotImplementedException();
        }

        public void SetAGSFolderPermission(SetAGSFolderPermissionsCommand setAGSFolderPermissionsCommand)
        {
            throw new NotImplementedException();
        }

        public void UpdateAGSFile(UpdateAGSFileCommand updateAGSFileCommand)
        {
            throw new NotImplementedException();
        }

        public void UpdateAGSFolder(UpdateAGSFolderCommand updateAGSFolderCommand)
        {
            throw new NotImplementedException();
        }
    }
}
