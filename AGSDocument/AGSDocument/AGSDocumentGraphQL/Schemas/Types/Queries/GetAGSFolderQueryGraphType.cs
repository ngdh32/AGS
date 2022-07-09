using AGSDocumentCore.Models.DTOs.Queries;
using HotChocolate.Types;

public class GetAGSFolderQueryGraphType : InputObjectType<GetAGSFolderQuery>
{
    public GetAGSFolderQueryGraphType()
    {

    }
    protected override void Configure(IInputObjectTypeDescriptor<GetAGSFolderQuery> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(f => f.FolderId);
        descriptor.Field(f => f.UserId);
    }
}