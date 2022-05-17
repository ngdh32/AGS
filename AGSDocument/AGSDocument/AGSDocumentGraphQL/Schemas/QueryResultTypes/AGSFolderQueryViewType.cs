using AGSDocumentCore.Models.DTOs.QueryResults;
using GraphQL.Types;

public class AGSFolderQueryViewType : ObjectGraphType<AGSFolderQueryView>
{
    public AGSFolderQueryViewType()
    {
        Field(x => x.FolderId);
        Field(x => x.Name);
        Field(x => x.Description);
        Field(x => x.CreatedDate);
        Field(x => x.CreatedUsername);
        Field(x => x.Permissions);
        Field(x => x.Files);
        Field(x => x.ChildrenFolderIds);
    }
}