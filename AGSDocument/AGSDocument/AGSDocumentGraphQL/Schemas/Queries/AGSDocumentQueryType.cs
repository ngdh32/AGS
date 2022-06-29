using System;
using System.Collections.Generic;
using System.Linq;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Models.DTOs.Queries;
using AGSDocumentCore.Models.DTOs.QueryResults;
using AGSDocumentCore.Models.Entities;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public class AGSDocumentQueryType : ObjectGraphType
{
    public AGSDocumentQueryType()
    {
        Field<AGSFolderQueryViewGraphType>("GetFolders", arguments: new QueryArguments(new QueryArgument<StringGraphType>{
            Name = "folderId"
        }), resolve: context => {
            // var queryService = context.RequestServices.GetRequiredService<IAGSDocumentQueryService>();
            // var httpContextAccessor = context.RequestServices.GetRequiredService<IHttpContextAccessor>();

            return result;
            // var folderId = context.GetArgument<string>("folderId");
            // return queryService.GetAGSFolder(new GetAGSFolderQuery(){
            //     FolderId = folderId,
            //     UserId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub").Value
            // });
        });
    }

    private AGSFolderQueryView result = new AGSFolderQueryView()
    {
        FolderId = "Folder Id",
        Name = "Folder Name",
        Description = "Description",
        CreatedDate = DateTime.Now,
        CreatedUsername = "CreatedUsername",
        Permissions = new List<AGSPermission> (){
            new AGSPermission(){
                Id = "Permission 1",
                DepartmentId = "DepartmentId 1"
                // PermissionType = 
            }
        },
        Files = new List<AGSFileQueryView>()
    };
}