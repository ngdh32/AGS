using System;
using System.Collections.Generic;
using AGSDocumentCore.Models.DTOs.Queries;
using AGSDocumentCore.Models.DTOs.QueryResults;

namespace AGSDocumentCore.Interfaces.Services
{
    public interface IAGSDocumentQueryService
    {
        public AGSFolderQueryView GetAGSFolder(GetAGSFolderQuery getAGSFolderQuery);

        public List<AGSFileQueryView> AGSFileIndexSearch(GetAGSFileSearchQuery agsFileIndexSearchQuery);
    }
}
