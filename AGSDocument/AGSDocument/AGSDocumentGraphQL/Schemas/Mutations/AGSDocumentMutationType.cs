using System.Security.Claims;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Models.DTOs.Commands;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;

public class AGSDocumentMutationType : ObjectType<IAGSDocumentUpdateService>
{
    protected override void Configure(IObjectTypeDescriptor<IAGSDocumentUpdateService> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor
            .Field(x => x.CreateAGSFolder(default))
            .Type<CommandResultGraphType>()
            .Resolve(context =>
            {
                var updateService = GetAGSDocumentUpdateService(context);
                var userId = GetUserId(context);
                var command = context.ArgumentValue<CreateAGSFolderCommand>("command");
                return updateService.CreateAGSFolder(command);
            });


        descriptor
            .Field(x => x.AddAGSFolder(default))
            .Type<CommandResultGraphType>()
            .Resolve(context =>
            {
                var updateService = GetAGSDocumentUpdateService(context);
                var userId = GetUserId(context);
                var command = context.ArgumentValue<AddAGSFolderToFolderCommand>("command");
                return updateService.AddAGSFolder(command);
            });

        descriptor
            .Field(x => x.UpdateAGSFolder(default))
            .Type<CommandResultGraphType>()
            .Resolve(context =>
            {
                var updateService = GetAGSDocumentUpdateService(context);
                var userId = GetUserId(context);
                var command = context.ArgumentValue<UpdateAGSFolderCommand>("command");
                return updateService.UpdateAGSFolder(command);
            });

        descriptor
            .Field(x => x.AddAGSFileToFolder(default))
            .Type<CommandResultGraphType>()
            .Resolve(context =>
            {
                var updateService = GetAGSDocumentUpdateService(context);
                var userId = GetUserId(context);
                var command = context.ArgumentValue<AddAGSFileToFolderCommand>("command");
                return updateService.AddAGSFileToFolder(command);
            });

        descriptor
            .Field(x => x.SetAGSFolderPermission(default))
            .Type<CommandResultGraphType>()
            .Resolve(context =>
            {
                var updateService = GetAGSDocumentUpdateService(context);
                var userId = GetUserId(context);
                var command = context.ArgumentValue<SetAGSFolderPermissionsCommand>("command");
                return updateService.SetAGSFolderPermission(command);
            });

        descriptor
            .Field(x => x.UpdateAGSFile(default))
            .Type<CommandResultGraphType>()
            .Resolve(context =>
            {
                var updateService = GetAGSDocumentUpdateService(context);
                var userId = GetUserId(context);
                var command = context.ArgumentValue<UpdateAGSFileCommand>("command");
                return updateService.UpdateAGSFile(command);
            });

        descriptor
            .Field(x => x.DeleteAGSFile(default))
            .Type<CommandResultGraphType>()
            .Resolve(context =>
            {
                var updateService = GetAGSDocumentUpdateService(context);
                var userId = GetUserId(context);
                var command = context.ArgumentValue<DeleteAGSFileCommand>("command");
                return updateService.DeleteAGSFile(command);
            }); 

        descriptor
            .Field(x => x.DeleteAGSFolder(default))
            .Type<CommandResultGraphType>()
            .Resolve(context =>
            {
                var updateService = GetAGSDocumentUpdateService(context);
                var userId = GetUserId(context);
                var command = context.ArgumentValue<DeleteAGSFolderCommand>("command");
                return updateService.DeleteAGSFolder(command);
            }); 
    }

    private IAGSDocumentUpdateService GetAGSDocumentUpdateService(IResolverContext context) => context.Service<IAGSDocumentUpdateService>();

    private string GetUserId(IResolverContext context)
    {
        var claimsPrincipal = context.GetUser();
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId;
    }
}