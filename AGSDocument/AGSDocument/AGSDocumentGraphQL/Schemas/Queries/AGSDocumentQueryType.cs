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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public class AGSDocumentQueryType : ObjectType
{
    public AGSDocumentQueryType()
    {
        
    }
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor
            .Field("getFolder")
            .Resolve(context =>
            {
                var claimsPrincipal = context.GetUser();
                var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
                return GetFolder();
            });
    }

    public AGSFolderQueryView GetFolder(){
        return result;
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