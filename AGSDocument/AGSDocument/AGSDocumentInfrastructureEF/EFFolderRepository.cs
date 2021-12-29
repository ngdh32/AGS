using System;
using System.Collections.Generic;
using System.Linq;
using AGSDocumentCore.Interfaces.Repositories;
using AGSDocumentCore.Models.Entities;
using AGSDocumentInfrastructureEF.Entities;

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
            var folder = _dbContext.Folders.FirstOrDefault(x => x.Id == folderId);
            if (folder != null)
            {
                // related permissions and files are removed when the folders are rmeoved
                _dbContext.Folders.Remove(folder);

                _dbContext.SaveChanges();
            }
        }

        public (AGSFile file, string folderId) GetFileById(string fileId)
        {
            var file = _dbContext.Files.FirstOrDefault(x => x.Id == fileId);
            if (file == null)
            {
                return (null, null);
            }

            var result = GetAGSFileFromEntity(file);
            return (result, file.FolderId);
        }

        public AGSFolder GetFolderById(string folderId)
        {
            var folder = _dbContext.Folders.FirstOrDefault(x => x.Id == folderId);
            if (folder == null)
            {
                return null;
            }

            var childrenFolderIds = _dbContext.Folders.Where(x => x.ParentFolderId == folderId)?.Select(x => x.Id);
            var childrenFolders = new List<AGSFolder>();
            if (childrenFolderIds != null && childrenFolderIds.Count() > 0)
            {
                childrenFolders = childrenFolderIds.Select(x => GetFolderById(folderId)).ToList();
            }

            return GetAGSFolderFromEntity(folder, childrenFolders);
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
            if (agsFolder != null)
            {
                agsFolder.Name = folder.Name;
                agsFolder.Description = folder.Description;
                agsFolder.CreatedBy = folder.CreatedBy;
                agsFolder.CreatedDate = folder.CreatedDate;
            }
            else
            {
                agsFolder = new EFAGSFolder()
                {
                    Id = folder.Id,
                    Name = folder.Name,
                    Description = folder.Description,
                    CreatedBy = folder.CreatedBy,
                    CreatedDate = folder.CreatedDate
                };

                _dbContext.Folders.Add(agsFolder);
            }

            if (folder.Files == null || folder.Files.Count == 0)
            {
                var toBeDeletedFiles = _dbContext.Files.Where(x => x.FolderId == folder.Id);
                if (toBeDeletedFiles != null)
                    _dbContext.Files.RemoveRange(toBeDeletedFiles);
            }

            if (folder.Files != null && folder.Files.Count > 0)
            {
                // just need to add/remove files. Update file will be done in SaveFile
                var toBeDeleteFiles = _dbContext.Files.Where(x => x.FolderId == folder.Id && folder.Files.All(y => x.Id != y.Id));
                if (toBeDeleteFiles != null)
                    _dbContext.Files.RemoveRange(toBeDeleteFiles);

                foreach (var file in folder.Files.Where(x => _dbContext.Files.Where(y => y.FolderId == folder.Id).All(z => z.Id != x.Id)))
                {
                    EFAGSFile efFile = new ();
                    efFile.Name = file.Name;
                    efFile.FileExtension = file.FileExtension;
                    efFile.Description = file.Description;
                    efFile.SizeInByte = file.SizeInByte;
                    efFile.FilePath = file.FilePath;
                    efFile.CreatedBy = file.CreatedBy;
                    efFile.CreatedDate = file.CreatedDate;
                    efFile.FolderId = folder.Id;

                    _dbContext.Files.Add(efFile);
                }
            }

            if (folder.Permissions == null || folder.Permissions.Count == 0)
            {
                var toBeDeletedPermissions = _dbContext.FolderPermissions.Where(x => x.FolderId == folder.Id);
                if (toBeDeletedPermissions != null)
                    _dbContext.FolderPermissions.RemoveRange(toBeDeletedPermissions);
            }

            if (folder.Permissions != null && folder.Permissions.Count > 0)
            {
                var toBeDeletedPermissions = _dbContext.FolderPermissions.Where(x => x.FolderId == folder.Id && folder.Permissions.All(y => y.DepartmentId != x.DepartmentId));
                if (toBeDeletedPermissions != null)
                    _dbContext.FolderPermissions.RemoveRange(toBeDeletedPermissions);

                foreach (var permission in folder.Permissions.Where(x => _dbContext.FolderPermissions.Where(y => y.FolderId == folder.Id).All(z => z.DepartmentId != x.DepartmentId)))
                {
                    EFAGSFolderPermission efPermission = new EFAGSFolderPermission()
                    {
                        DepartmentId = permission.DepartmentId,
                        FolderId = folder.Id,
                        PermissionType = (int)permission.PermissionType
                    };

                    _dbContext.FolderPermissions.Add(efPermission);
                }
            }

            _dbContext.SaveChanges();
        }

        public List<string> SearchFilesByName(string keyword)
        {
            return _dbContext.Files.Where(x => x.Name.ToUpper().Contains(keyword.ToUpper()))?.Select(y => y.Id)?.ToList();
        }

        private AGSFile GetAGSFileFromEntity(EFAGSFile file)
        {
            return new(file.Id, file.Name, file.Description, file.FileExtension, file.SizeInByte, file.FilePath, file.CreatedBy, file.CreatedDate);
        }

        private AGSPermission GetAGSPermissionFromEntity(EFAGSFolderPermission permission)
        {
            return new(permission.DepartmentId, permission.PermissionType);
        }

        private AGSFolder GetAGSFolderFromEntity(EFAGSFolder folder, List<AGSFolder> childrenFolders)
        {
            var agsFiles = folder.EFAGSFiles.Select(x => GetAGSFileFromEntity(x));
            var agsPermissions = folder.EFAGSFolderPermissions.Select(x => GetAGSPermissionFromEntity(x));
            return new(folder.Id, folder.Name, folder.CreatedBy, folder.CreatedDate, agsFiles.ToList(), childrenFolders.ToList(), agsPermissions.ToList());
        }
    }
}
