using System;
using AGSDocumentCore.Models.Enums;

namespace AGSDocumentCore.Models.DTOs.Queries
{
    public record AGSFileIndexSearchQuery(string keyword, string userId, SearchTypeEnum searchType);
}
