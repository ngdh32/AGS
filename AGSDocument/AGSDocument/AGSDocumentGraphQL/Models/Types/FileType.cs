using System;
using AGSDocumentCore.Models.Entities;
using HotChocolate.Types;

namespace AGSDocumentGraphQL.Models.Types
{
    public class FileType : ObjectType<AGSFile>
    {
        protected override void Configure(IObjectTypeDescriptor<AGSFile> descriptor)
        {
            descriptor.BindFieldsImplicitly();
        }
    }
}
