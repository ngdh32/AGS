using AGSDocumentCore.Models.DTOs.Queries;
using HotChocolate.Types;

public class CommandResultGraphType : ObjectType<CommandResult>
{
    public CommandResultGraphType()
    {

    }
    protected override void Configure(IObjectTypeDescriptor<CommandResult> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(f => f.ErrorCode);
        descriptor.Field(f => f.ErrorMessage);
    }
}