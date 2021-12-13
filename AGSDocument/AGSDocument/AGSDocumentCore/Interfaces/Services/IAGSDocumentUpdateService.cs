using System;
using AGSDocumentCore.Models.DTOs.Commands;

namespace AGSDocumentCore.Interfaces.Services
{
    public interface IAGSDocumentUpdateService
    {
        public void CreateAGSFolder(CreateAGSFolderCommand createAGSFolderCommand);

        public void UpdateAGSFolder(UpdateAGSFolderCommand updateAGSFolderCommand);

        public void AddAGSFileToFolder(AddAGSFileToFolderCommand addAGSFileToFolderCommand);

        public void SetAGSFolderPermission(SetAGSFolderPermissionsCommand setAGSFolderPermissionsCommand);

        public void UpdateAGSFile(UpdateAGSFileCommand updateAGSFileCommand);

        public void DeleteAGSFile(DeleteAGSFileCommand deleteAGSFileCommand);

        public void DeleteAGSFolder(string folderId);
    }
}
