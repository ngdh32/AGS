using System;
using System.Collections.Generic;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Models.DTOs.QueryResults
{
    public record AGSFolderQueryView(string folderId, string name, string description, DateTime createdDate, string createdUsername, List<AGSPermission> permissions, List<AGSChildrenFolderQueryView> childrenFolders, List<AGSFileQueryView> files);

    public record AGSChildrenFolderQueryView(string folderId, string name, string description, DateTime createdDate, string createdUsername, List<AGSPermission> permissions);

    public record AGSFileQueryView(string fileId, string description, int sizeInByte, DateTime createdDate, string createdUsername);
}
