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
            throw new NotImplementedException();
        }

        public void SaveFolder(AGSFolder folder)
        {
            throw new NotImplementedException();
        }

        public List<string> SearchFilesByName(string keyword)
        {
            throw new NotImplementedException();
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
