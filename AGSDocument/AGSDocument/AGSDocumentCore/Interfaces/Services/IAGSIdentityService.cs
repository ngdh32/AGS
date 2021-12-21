using System;
using System.Collections.Generic;
using AGSDocumentCore.Models.DTOs.Queries;

namespace AGSDocumentCore.Interfaces.Services
{
    public interface IAGSIdentityService
    {
        public List<AGSUser> GetUsers();
    }
}
