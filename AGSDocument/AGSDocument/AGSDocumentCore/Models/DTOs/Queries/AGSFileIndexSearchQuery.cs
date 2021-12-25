using System;
using AGSDocumentCore.Models.Enums;

namespace AGSDocumentCore.Models.DTOs.Queries
{
    public class AGSFileIndexSearchQuery
    {
        public string Keyword { get; init; }
        public string UserId { get; init; }
        public SearchTypeEnum SearchType { get; init; }
    }
}
