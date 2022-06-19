using AGSDocumentCore.Models.DTOs.QueryResults;
using GraphQL.Types;

public class AGSFileQueryViewType : ObjectGraphType<AGSFileQueryView>
{
    public AGSFileQueryViewType()
    {
        Field(x => x.FileId);
        Field(x => x.SizeInByte);
        Field(x => x.Description);
        Field(x => x.CreatedDate);
        Field(x => x.CreatedUsername);
    }
}