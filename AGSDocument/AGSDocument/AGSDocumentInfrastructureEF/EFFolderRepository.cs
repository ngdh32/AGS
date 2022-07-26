using System;
using System.Collections.Generic;
using System.Linq;
using AGSDocumentCore.Interfaces.Repositories;
using AGSDocumentCore.Models.Entities;
using AGSDocumentInfrastructureEF.Entities;
using J2N.Collections.Generic.Extensions;

namespace AGSDocumentInfrastructureEF
{
    public class EFFolderRepository : IFolderRepository
    {
        private readonly EFDbContext _dbContext;

        public EFFolderRepository(EFDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteFolder(string folderId)
        {
            var files = _dbContext.Files.Where(x => x.FolderId == folderId);
            if (files != null && files.Count() > 0)
            {
                _dbContext.Files.RemoveRange(files);
            }

            var folder = _dbContext.Folders.FirstOrDefault(x => x.Id == folderId);
            if (folder != null)
            {
                _dbContext.Folders.Remove(folder);
            }

            _dbContext.SaveChanges();
        }

        public AGSFolder GetFolderByFileId(string fileId)
        {
            var file = _dbContext.Files.FirstOrDefault(x => x.Id == fileId);
            if (file == null)
            {
                return null;
            }

            var folder = _dbContext.Folders.FirstOrDefault(x => x.Id == file.FolderId);
            return folder == null ? null : GetAGSFolderFromEntity(folder);
        }

        public AGSFile GetFileById(string fileId)
        {
            var file = _dbContext.Files.FirstOrDefault(x => x.Id == fileId);
            return file == null? null : GetAGSFileFromEntity(file);
        }

        public AGSFolder GetFolderById(string folderId)
        {
            var folder = _dbContext.Folders.FirstOrDefault(x => x.Id == folderId);
            return folder == null ? null : GetAGSFolderFromEntity(folder);
        }

        public void SaveFile(AGSFile file)
        {
            var efFile = _dbContext.Files.FirstOrDefault(x => x.Id == file.Id);
            if (efFile != null)
            {
                efFile.Name = file.Name;
                efFile.FileExtension = file.FileExtension;
                efFile.Description = file.Description;
                efFile.SizeInByte = file.SizeInByte;
                efFile.FilePath = file.FilePath;
                efFile.CreatedBy = file.CreatedBy;
                efFile.CreatedDate = file.CreatedDate;

                _dbContext.SaveChanges();
            }
        }

        public void SaveFolder(AGSFolder folder)
        {
            var agsFolder = _dbContext.Folders.FirstOrDefault(x => x.Id == folder.Id);
            var isFolderNew = agsFolder == null;
            agsFolder ??= new EFAGSFolder()
            {
                Id = folder.Id
            };

            agsFolder.Name = folder.Name;
            agsFolder.Description = folder.Description;
            agsFolder.CreatedBy = folder.CreatedBy;
            agsFolder.CreatedDate = folder.CreatedDate;

            _ = isFolderNew ? _dbContext.Folders.Add(agsFolder) : _dbContext.Folders.Update(agsFolder);

            // remove those files that no longer exist
            var toBeDeleteFiles = _dbContext.Files.Where(x => x.FolderId == folder.Id && !folder.Files.Any(y => x.Id == y.Id));
            if (toBeDeleteFiles != null)
                _dbContext.Files.RemoveRange(toBeDeleteFiles);

            foreach(var file in folder.Files)
            {
                var efFile = _dbContext.Files.FirstOrDefault(x => x.Id == file.Id);
                var isNewFile = efFile == null;
                efFile ??= new EFAGSFile()
                {
                    Id = file.Id
                };

                efFile.Name = file.Name;
                efFile.FileExtension = file.FileExtension;
                efFile.Description = file.Description;
                efFile.SizeInByte = file.SizeInByte;
                efFile.FilePath = file.FilePath;
                efFile.CreatedBy = file.CreatedBy;
                efFile.CreatedDate = file.CreatedDate;
                efFile.FolderId = folder.Id;

                _ = isNewFile ? _dbContext.Files.Add(efFile) : _dbContext.Files.Update(efFile);   
            }

            // remove those permissions that no longer exist
            var toBeDeletedPermissions = _dbContext.FolderPermissions.Where(x => x.FolderId == folder.Id && !folder.Permissions.Any(y => x.Id == y.Id));
            if (toBeDeletedPermissions != null)
                _dbContext.FolderPermissions.RemoveRange(toBeDeletedPermissions);

            foreach(var permission in folder.Permissions)
            {
                var efPermission = _dbContext.FolderPermissions.FirstOrDefault(x => x.Id == permission.Id);
                var isNewPermission = efPermission == null;
                efPermission ??= new EFAGSFolderPermission()
                {
                    Id = permission.Id
                };

                efPermission.FolderId = folder.Id;
                efPermission.DepartmentId = permission.DepartmentId;
                efPermission.PermissionType = (int)permission.PermissionType;

                _ = isNewPermission ? _dbContext.FolderPermissions.Add(efPermission) : _dbContext.FolderPermissions.Update(efPermission);   
            }

            _dbContext.SaveChanges();
        }

        public List<string> SearchFilesByName(string keyword)
        {
            return _dbContext.Files.Where(x => x.Name.ToUpper().Contains(keyword.ToUpper()))?.Select(y => y.Id)?.ToList();
        }

        private AGSFile GetAGSFileFromEntity(EFAGSFile file)
        {
            return new AGSFile(file.Id, file.Name, file.Description, file.FileExtension, file.SizeInByte, file.FilePath, file.CreatedBy, file.CreatedDate);
        }

        private AGSPermission GetAGSPermissionFromEntity(EFAGSFolderPermission permission)
        {
            return new AGSPermission(permission.Id, permission.DepartmentId, permission.PermissionType);
        }

        private AGSFolder GetAGSFolderFromEntity(EFAGSFolder folder)
        {
            var agsFiles = folder.EFAGSFiles.Select(x => GetAGSFileFromEntity(x));
            var agsPermissions = folder.EFAGSFolderPermissions.Select(x => GetAGSPermissionFromEntity(x));
            var childrenFolders = _dbContext.Folders.Where(x => x.ParentFolderId == folder.Id);
            var agsChildrenFolders = new List<AGSFolder>();
            if (childrenFolders != null && childrenFolders.Count() > 0)
            {
                agsChildrenFolders = childrenFolders.Select(x => GetAGSFolderFromEntity(x)).ToList();
            }
            return new AGSFolder(folder.Id, folder.Name, folder.CreatedBy, folder.CreatedDate, agsFiles.ToList(), agsChildrenFolders.ToList(), agsPermissions.ToList());
        }
    }
}
