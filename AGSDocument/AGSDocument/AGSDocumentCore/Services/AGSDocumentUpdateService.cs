using System;
using System.Collections.Generic;
using AGSDocumentCore.Interfaces.Repositories;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Models.DTOs.Commands;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Services
{
    public class AGSDocumentUpdateService : IAGSDocumentUpdateService
    {
        private readonly IFolderRepository _folderRepository;
        public AGSDocumentUpdateService(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }

        #region Folder
        public CommandResult AddAGSFolder(AddAGSFolderToFolderCommand command)
        {
            var folder = _folderRepository.GetFolderById(command.ParentFolderId);
            folder.AddNewFolder(command.Name, command.Description, command.CreatedBy, command.Permissions);
            _folderRepository.SaveFolder(folder);
            return new CommandResult{
                ErrorCode = (int)ErrorCodeType.Success
            };
        }

        public CommandResult CreateAGSFolder(CreateAGSFolderCommand command)
        {
            var folder = new AGSFolder(command.Name, command.Description, command.CreatedBy, command.Permissions);
            _folderRepository.SaveFolder(folder);
            return new CommandResult{
                ErrorCode = (int)ErrorCodeType.Success
            };
        }

        public CommandResult UpdateAGSFolder(UpdateAGSFolderCommand command)
        {
            var folder = _folderRepository.GetFolderById(command.FolderId);
            folder.UpdateFolder(command.Name, command.Description);
            _folderRepository.SaveFolder(folder);
            return new CommandResult{
                ErrorCode = (int)ErrorCodeType.Success
            };
        }

        public CommandResult DeleteAGSFolder(DeleteAGSFolderCommand command)
        {
            _folderRepository.DeleteFolder(command.FolderId);
            return new CommandResult{
                ErrorCode = (int)ErrorCodeType.Success
            };
        }

        public CommandResult SetAGSFolderPermission(SetAGSFolderPermissionsCommand command)
        {
            var folder = _folderRepository.GetFolderById(command.FolderId);
            var permissions = new List<AGSPermission>();
            foreach (var permission in command.Permissions)
            {
                permissions.Add(new AGSPermission()
                {
                    DepartmentId = permission.DepartmentId,
                    PermissionType = permission.PermissionType
                });
            }
            folder.SetPermissions(permissions);
            _folderRepository.SaveFolder(folder);
            return new CommandResult{
                ErrorCode = (int)ErrorCodeType.Success
            };
        }

        #endregion

        #region File
        public CommandResult AddAGSFileToFolder(AddAGSFileToFolderCommand command)
        {
            var folder = _folderRepository.GetFolderById(command.FolderId);
            folder.AddNewFile(new AGSFile(
                command.Name
                , command.Description
                , command.FileExtension
                , command.SizeInByte
                , command.FilePath
                , command.CreatedBy
            ));

            _folderRepository.SaveFolder(folder);
            return new CommandResult{
                ErrorCode = (int)ErrorCodeType.Success
            };
        }

        public CommandResult DeleteAGSFile(DeleteAGSFileCommand command)
        {
            var (file, folderId) = _folderRepository.GetFileById(command.FileId);
            var folder = _folderRepository.GetFolderById(folderId);
            folder.DeleteFile(command.FileId);
            _folderRepository.SaveFolder(folder);
            return new CommandResult{
                ErrorCode = (int)ErrorCodeType.Success
            };
        }

        public CommandResult UpdateAGSFile(UpdateAGSFileCommand command)
        {
            var (file, folderId) = _folderRepository.GetFileById(command.FileId);
            file.UpdateFile(command.Name, command.FileExtension, command.Description, command.SizeInByte, command.CreatedBy);
            _folderRepository.SaveFile(file);
            return new CommandResult{
                ErrorCode = (int)ErrorCodeType.Success
            };
        }

        #endregion
    }
}
