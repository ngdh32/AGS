using System;
using AGSDocumentCore.Models.DTOs.Commands;

namespace AGSDocumentCore.Interfaces
{
    public interface IAGSDocumentUpdateService
    {
        public void CreateAGSFolder(CreateAGSFolderCommand createAGSFolderRequest);

        public void UpdateAGSFolder(UpdateAGSFolderCommand updateAGSFolderRequest);

        public void AddAGSFileToFolder(AddAGSFileToFolderCommand addAGSFileToFolder);

        public void UpdateAGSFile(UpdateAGSFileCommand updateAGSFileRequest);

        public void DeleteAGSFile(DeleteAGSFileCommand deleteAGSFile);

        public void DeleteAGSFolder(string folderId);
    }
}
