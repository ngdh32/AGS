using System;
using AGSDocumentCore.Models.Entities;
using HotChocolate.Types;

namespace AGSDocumentGraphQL.Models.Types
{
    public class PermissionType : ObjectType<AGSPermission>
    {
        protected override void Configure(IObjectTypeDescriptor<AGSPermission> descriptor)
        {
            descriptor.BindFieldsImplicitly();
        }
    }
}
