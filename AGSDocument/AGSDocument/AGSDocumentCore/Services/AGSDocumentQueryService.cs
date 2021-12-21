using System;
using System.Collections.Generic;
using System.Linq;
using AGSDocumentCore.Interfaces.Repositories;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Models.DTOs.Queries;
using AGSDocumentCore.Models.DTOs.QueryResults;
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

        public List<AGSFileQueryView> AGSFileIndexSearch(AGSFileIndexSearchQuery agsFileIndexSearchQuery)
        {
            var result = new List<AGSFileQueryView>();
            


            return result;
        }

        public AGSFolderQueryView GetAGSFolder(GetAGSFolderQuery getAGSFolderQuery)
        {
            var users = _identityService.GetUsers();

            var retrievingUser = users.FirstOrDefault(x => x.userId == getAGSFolderQuery.userId);
            if (retrievingUser == null)
                return null;

            var folder = _folderRepository.GetById(getAGSFolderQuery.folderId);
            if (folder == null)
                return null;

            var permissionChecking = CheckIfUserHasPermissionToAccess(retrievingUser, folder.Permissions.ToList());
            if (!permissionChecking)
                return null;

            var createdUsername = users.FirstOrDefault(x => x.userId == folder.CreatedBy)?.username ?? folder.CreatedBy;
            var childrenFolders = new List<AGSChildrenFolderQueryView>();
            foreach (var childrenFolder in folder.ChildrenFolders)
            {
                var childrenFolderPermissionChecking = CheckIfUserHasPermissionToAccess(retrievingUser, childrenFolder.Permissions.ToList());
                if (!childrenFolderPermissionChecking)
                    continue;

                var childrenFolderCreatedUsername = users.FirstOrDefault(x => x.userId == childrenFolder.CreatedBy)?.username ?? childrenFolder.CreatedBy;
                childrenFolders.Add(new AGSChildrenFolderQueryView(childrenFolder.Id, childrenFolder.Name, childrenFolder.Description, childrenFolder.CreatedDate, childrenFolderCreatedUsername, childrenFolder.Permissions.ToList()));
            }
            var files = new List<AGSFileQueryView>();
            foreach (var file in folder.Files)
            {
                var fileCreatedUser = users.FirstOrDefault(x => x.userId == file.CreatedBy)?.username ?? file.CreatedBy;
                files.Add(new AGSFileQueryView(file.Id, file.Description, file.SizeInByte, file.CreatedDate, fileCreatedUser));
            }
            var result = new AGSFolderQueryView(folder.Id, folder.Name, folder.Description, folder.CreatedDate, createdUsername, folder.Permissions.ToList(), childrenFolders, files);
            return result;
        }

        private bool CheckIfUserHasPermissionToAccess(AGSUser user, List<AGSPermission> permissions)
        {
            var result = permissions.Any(x => user.departments.Any(y => y.departmentId == x.DepartmentId));
            return result;
        }
    }
}
