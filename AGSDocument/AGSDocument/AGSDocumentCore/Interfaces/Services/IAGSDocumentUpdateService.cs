using System;
using AGSDocumentCore.Models.DTOs.Commands;

namespace AGSDocumentCore.Interfaces.Services
{
    public interface IAGSDocumentUpdateService
    {
        public void CreateAGSFolder(CreateAGSFolderCommand command);

        public void AddAGSFolder(AddAGSFolderToFolderCommand command);

        public void UpdateAGSFolder(UpdateAGSFolderCommand command);

        public void AddAGSFileToFolder(AddAGSFileToFolderCommand command);

        public void SetAGSFolderPermission(SetAGSFolderPermissionsCommand command);

        public void UpdateAGSFile(UpdateAGSFileCommand command);

        public void DeleteAGSFile(DeleteAGSFileCommand command);

        public void DeleteAGSFolder(DeleteAGSFolderCommand command);
    }
}
