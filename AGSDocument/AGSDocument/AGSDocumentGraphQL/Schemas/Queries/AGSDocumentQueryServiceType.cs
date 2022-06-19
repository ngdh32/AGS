using System.Linq;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Models.DTOs.Queries;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public class AGSDocumentQueryServiceType : ObjectGraphType<object>
{
    public AGSDocumentQueryServiceType()
    {
        Field<AGSFolderQueryViewType>("GetFolders", arguments: new QueryArguments(new QueryArgument<StringGraphType>{
            Name = "folderId"
        }), resolve: context => {
            var queryService = context.RequestServices.GetRequiredService<IAGSDocumentQueryService>();
            var httpContextAccessor = context.RequestServices.GetRequiredService<IHttpContextAccessor>();

            var folderId = context.GetArgument<string>("folderId");
            return queryService.GetAGSFolder(new GetAGSFolderQuery(){
                FolderId = folderId,
                UserId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub").Value
            });
        });
    }
}