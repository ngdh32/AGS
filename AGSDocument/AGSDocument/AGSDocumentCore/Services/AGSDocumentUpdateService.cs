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
        public void AddAGSFolder(AddAGSFolderToFolderCommand command)
        {
            var folder = _folderRepository.GetFolderById(command.parentFolderId);
            folder.AddNewFolder(command.name, command.description, command.createdBy, command.permissions);
            _folderRepository.SaveFolder(folder);
        }

        public void CreateAGSFolder(CreateAGSFolderCommand command)
        {
            var folder = new AGSFolder(command.name, command.description, command.createdBy, command.permissions);
            _folderRepository.SaveFolder(folder);
        }

        public void UpdateAGSFolder(UpdateAGSFolderCommand command)
        {
            var folder = _folderRepository.GetFolderById(command.folderId);
            folder.UpdateFolder(command.name, command.description);
            _folderRepository.SaveFolder(folder);
        }

        public void DeleteAGSFolder(DeleteAGSFolderCommand command)
        {
            _folderRepository.DeleteFolder(command.folderId);
        }

        public void SetAGSFolderPermission(SetAGSFolderPermissionsCommand command)
        {
            var folder = _folderRepository.GetFolderById(command.folderId);
            var permissions = new List<AGSPermission>();
            foreach (var permission in command.permissions)
            {
                permissions.Add(new AGSPermission()
                {
                    DepartmentId = permission.departmentId,
                    PermissionType = permission.permissionType
                });
            }
            folder.SetPermissions(permissions);
            _folderRepository.SaveFolder(folder);
        }

        #endregion

        #region File
        public void AddAGSFileToFolder(AddAGSFileToFolderCommand command)
        {
            var folder = _folderRepository.GetFolderById(command.folderId);
            folder.AddNewFile(new AGSFile(
                command.name
                , command.description
                , command.fileExtension
                , command.sizeInBytes
                , command.filepath
                , command.createdBy
            ));

            _folderRepository.SaveFolder(folder);
        }

        public void DeleteAGSFile(DeleteAGSFileCommand command)
        {
            var (file, folderId) = _folderRepository.GetFileById(command.fileId);
            var folder = _folderRepository.GetFolderById(folderId);
            folder.DeleteFile(command.fileId);
            _folderRepository.SaveFolder(folder);
        }

        public void UpdateAGSFile(UpdateAGSFileCommand command)
        {
            var (file, folderId) = _folderRepository.GetFileById(command.fileId);
            file.UpdateFile(command.name, command.fileExtension, command.description, command.sizeInByte, command.createdBy);
            _folderRepository.SaveFile(file);
        }

        #endregion
    }
}
