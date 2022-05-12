using AGSDocumentCore.Models.DTOs.Queries;
using HotChocolate.Types;

public class GetAGSFolderQueryType : ObjectType<GetAGSFolderQuery>
{
    protected override void Configure(IObjectTypeDescriptor<GetAGSFolderQuery> descriptor)
    {
        descriptor.BindFieldsImplicitly();
    }
}