using System;
using AGSDocumentCore.Models.Entities;
using HotChocolate.Types;

namespace AGSDocumentGraphQL.Models.Types
{
    public class FolderType : ObjectType<AGSFolder>
    {
        protected override void Configure(IObjectTypeDescriptor<AGSFolder> descriptor)
        {
            descriptor.BindFieldsImplicitly();
        }
    }
}
