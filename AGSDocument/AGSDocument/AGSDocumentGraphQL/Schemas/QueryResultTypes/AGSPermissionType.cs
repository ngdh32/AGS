using AGSDocumentCore.Models.Entities;
using GraphQL.Types;

public class AGSPermissionType: ObjectGraphType<AGSPermission>
{
    public AGSPermissionType()
    {
        Field(x => x.PermissionType);
        Field(x => x.DepartmentId);
    }
}