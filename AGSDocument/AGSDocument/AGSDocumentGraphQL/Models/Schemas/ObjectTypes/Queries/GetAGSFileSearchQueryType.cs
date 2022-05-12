using AGSDocumentCore.Models.DTOs.Queries;
using HotChocolate.Types;

public class AGSFileIndexSearchQueryType : ObjectType<GetAGSFileSearchQuery>
{
    protected override void Configure(IObjectTypeDescriptor<GetAGSFileSearchQuery> descriptor)
    {
        descriptor.BindFieldsImplicitly();
    }
}