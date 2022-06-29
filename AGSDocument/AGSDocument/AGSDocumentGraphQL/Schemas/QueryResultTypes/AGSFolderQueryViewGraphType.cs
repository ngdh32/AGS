using System;
using System.Collections.Generic;
using AGSDocumentCore.Models.DTOs.QueryResults;
using AGSDocumentCore.Models.Entities;
using GraphQL.Types;

public class AGSFolderQueryViewGraphType : ObjectGraphType<AGSFolderQueryView>
{
    public AGSFolderQueryViewGraphType()
    {
        Field(x => x.FolderId);
        Field(x => x.Name);
        Field(x => x.Description, true);
        Field(x => x.CreatedDate);
        Field(x => x.CreatedUsername, true);
        Field<List<AGSPermission>>(x => x.Permissions, true, typeof (ListGraphType<AGSPermissionGraphType>));
        // Field<ListGraphType<AGSFileQueryViewType>>(x => x.Files, true);
        // Field<ListGraphType<StringGraphType>>(x => x.ChildrenFolderId);
    }
}