using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Models.DTOs.Queries;
using AGSDocumentCore.Models.DTOs.QueryResults;
using AGSDocumentCore.Models.Entities;
using HotChocolate;
using HotChocolate.Types;

public class AGSDocumentQueryType : ObjectType<IAGSDocumentQueryService>
{
    protected override void Configure(IObjectTypeDescriptor<IAGSDocumentQueryService> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor
            .Field(x => x.GetAGSFolder(default))
            .Type<AGSFolderQueryViewGraphType>()
            .Resolve(context =>
            {
                var claimsPrincipal = context.GetUser();
                IAGSDocumentQueryService queryService = context.Service<IAGSDocumentQueryService>();
                var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
                var getAGSFolderQuery = context.ArgumentValue<GetAGSFolderQuery>("getAGSFolderQuery");
                return queryService.GetAGSFolder(getAGSFolderQuery);
            });
    }

    // public AGSFolderQueryView GetFolder(){
    //     return result;
    // }

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