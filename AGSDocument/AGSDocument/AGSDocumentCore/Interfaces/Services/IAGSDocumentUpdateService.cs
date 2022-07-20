using System;
using AGSDocumentCore.Models.DTOs.Commands;

namespace AGSDocumentCore.Interfaces.Services
{
    public interface IAGSDocumentUpdateService
    {
        public CommandResult CreateAGSFolder(CreateAGSFolderCommand command);

        public CommandResult AddAGSFolder(AddAGSFolderToFolderCommand command);

        public CommandResult UpdateAGSFolder(UpdateAGSFolderCommand command);

        public CommandResult AddAGSFileToFolder(AddAGSFileToFolderCommand command);

        public CommandResult SetAGSFolderPermission(SetAGSFolderPermissionsCommand command);

        public CommandResult UpdateAGSFile(UpdateAGSFileCommand command);

        public CommandResult DeleteAGSFile(DeleteAGSFileCommand command);

        public CommandResult DeleteAGSFolder(DeleteAGSFolderCommand command);
    }
}
