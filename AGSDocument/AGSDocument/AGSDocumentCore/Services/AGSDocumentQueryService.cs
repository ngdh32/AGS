using System;
using System.Collections.Generic;
using System.Linq;
using AGSDocumentCore.Interfaces.Repositories;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Models.DTOs.Queries;
using AGSDocumentCore.Models.DTOs.QueryResults;
using AGSDocumentCore.Models.DTOs.Services;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Services
{
    public class AGSDocumentQueryService : IAGSDocumentQueryService
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IFileIndexingService _fileIndexingService;
        private readonly IFileService _fileService;
        private readonly IAGSIdentityService _identityService;
        public AGSDocumentQueryService(IFolderRepository folderRepository, IFileIndexingService fileIndexingService, IFileService fileService, IAGSIdentityService identityService)
        {
            _folderRepository = folderRepository;
            _fileIndexingService = fileIndexingService;
            _fileService = fileService;
            _identityService = identityService;
        }

        public List<AGSFileQueryView> AGSFileIndexSearch(GetAGSFileSearchQuery agsFileIndexSearchQuery)
        {
            var users = _identityService.GetUsers().Result;
            var user = users.FirstOrDefault(x => x.UserId == agsFileIndexSearchQuery.UserId);
            if (user == null)
                return null;

            var result = new List<AGSFileQueryView>();
            var fileIds = new List<string>();
            switch(agsFileIndexSearchQuery.SearchType)
            {
                case Models.Enums.SearchTypeEnum.FileContent:
                    fileIds = _fileIndexingService.FileSearchingByContent(agsFileIndexSearchQuery.Keyword);
                    break;
                case Models.Enums.SearchTypeEnum.Filename:
                default:
                    fileIds = _folderRepository.SearchFilesByName(agsFileIndexSearchQuery.Keyword);
                    break;
            }

            foreach (var fileId in fileIds)
            {
                var (file, folderId) = _folderRepository.GetFileById(fileId);
                var folder = _folderRepository.GetFolderById(folderId);
                var checkPermission = CheckIfUserHasPermissionToAccess(user, folder.Permissions.ToList());
                if (!checkPermission)
                    return null;

                var fileCreatedUsername = users.FirstOrDefault(x => x.UserId == file.Id)?.Username ?? file.CreatedBy;
                result.Add(new AGSFileQueryView()
                {
                    FileId = file.Id,
                    Description = file.Description,
                    SizeInByte = file.SizeInByte,
                    CreatedDate = file.CreatedDate,
                    CreatedUsername = fileCreatedUsername
                });
            }
            return result;
        }

        // just one level of children folders
        public AGSFolderQueryView GetAGSFolder(GetAGSFolderQuery getAGSFolderQuery)
        {
            var users = _identityService.GetUsers().Result;

            var retrievingUser = users.FirstOrDefault(x => x.UserId == getAGSFolderQuery.UserId);
            if (retrievingUser == null)
                return null;

            var folder = _folderRepository.GetFolderById(getAGSFolderQuery.FolderId);
            if (folder == null)
                return null;

            var permissionChecking = CheckIfUserHasPermissionToAccess(retrievingUser, folder.Permissions.ToList());
            if (!permissionChecking)
                return null;

            var createdUsername = users.FirstOrDefault(x => x.UserId == folder.CreatedBy)?.Username ?? folder.CreatedBy;
            var childrenFolders = new List<AGSChildrenFolderQueryView>();
            foreach (var childrenFolder in folder.ChildrenFolders)
            {
                var childrenFolderPermissionChecking = CheckIfUserHasPermissionToAccess(retrievingUser, childrenFolder.Permissions.ToList());
                if (!childrenFolderPermissionChecking)
                    continue;

                var childrenFolderCreatedUsername = users.FirstOrDefault(x => x.UserId == childrenFolder.CreatedBy)?.Username ?? childrenFolder.CreatedBy;
                childrenFolders.Add(new AGSChildrenFolderQueryView()
                {
                    FolderId = childrenFolder.Id,
                    Name = childrenFolder.Name,
                    Description = childrenFolder.Description,
                    CreatedDate = childrenFolder.CreatedDate,
                    CreatedUsername = childrenFolderCreatedUsername,
                    Permissions = childrenFolder.Permissions.ToList()
                });
            }
            var files = new List<AGSFileQueryView>();
            foreach (var file in folder.Files)
            {
                var fileCreatedUser = users.FirstOrDefault(x => x.UserId == file.CreatedBy)?.Username ?? file.CreatedBy;
                files.Add(new AGSFileQueryView()
                {
                    FileId = file.Id,
                    Description = file.Description,
                    SizeInByte = file.SizeInByte,
                    CreatedDate = file.CreatedDate,
                    CreatedUsername = fileCreatedUser
                });
            }
            var result = new AGSFolderQueryView()
            {
                FolderId = folder.Id,
                Name = folder.Name,
                Description = folder.Description,
                CreatedDate = folder.CreatedDate,
                CreatedUsername = createdUsername,
                Permissions = folder.Permissions.ToList(),
                ChildrenFolders = childrenFolders,
                Files = files
            };
            return result;
        }

        private bool CheckIfUserHasPermissionToAccess(AGSUserViewModel user, List<AGSPermission> permissions)
        {
            var result = permissions.Any(x => user.Departments.Any(y => y.DepartmentId == x.DepartmentId));
            return result;
        }
    }
}
