using AGSDocumentCore.Models.Entities;
using GraphQL.Types;

public class AGSPermissionGraphType: ObjectGraphType<AGSPermission>
{
    public AGSPermissionGraphType()
    {
        Field(x => x.Id);
        // Field(x => x.PermissionType);
        Field(x => x.DepartmentId);
    }
}