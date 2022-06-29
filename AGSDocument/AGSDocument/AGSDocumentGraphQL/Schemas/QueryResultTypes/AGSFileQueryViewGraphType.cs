using AGSDocumentCore.Models.DTOs.QueryResults;
using GraphQL.Types;

public class AGSFileQueryViewGraphType : ObjectGraphType<AGSFileQueryView>
{
    public AGSFileQueryViewGraphType()
    {
        Field(x => x.FileId);
        Field(x => x.SizeInByte);
        Field(x => x.Description);
        Field(x => x.CreatedDate);
        Field(x => x.CreatedUsername);
    }
}