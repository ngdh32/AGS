using System;
using System.Collections.Generic;
using AGSDocumentCore.Models.DTOs.QueryResults;
using AGSDocumentCore.Models.Entities;
using HotChocolate.Types;

public class AGSFolderQueryViewGraphType: ObjectType<AGSFolderQueryView>
{
    public AGSFolderQueryViewGraphType()
    {
        
    }

    protected override void Configure(IObjectTypeDescriptor<AGSFolderQueryView> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(f => f.Name);
        descriptor.Field(f => f.FolderId);
        descriptor.Field(f => f.Description);
        descriptor.Field(f => f.CreatedDate);
        descriptor.Field(f => f.CreatedUsername);
        descriptor.Field(f => f.Permissions);
        descriptor.Field(f => f.ChildrenFolderIds);
        descriptor.Field(f => f.Files);
    }
}