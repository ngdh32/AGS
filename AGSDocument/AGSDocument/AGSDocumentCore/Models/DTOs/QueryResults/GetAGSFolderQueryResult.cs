using System;
using System.Collections.Generic;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Models.DTOs.QueryResults
{
    public record AGSFolderQueryView(string folderId, string name, string description, DateTime createdDate, string createdBy, List<AGSPermission> permissions, List<AGSFolderQueryView> childrenFolders, List<AGSFileQueryView> files);

    public record AGSFileQueryView(string fileId, string description, int sizeInByte, DateTime createdDate, string createdBy);
}
