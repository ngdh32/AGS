using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AGSDocumentCore.Models.DTOs.Queries;

namespace AGSDocumentCore.Interfaces.Services
{
    public interface IAGSIdentityService
    {
        public Task<List<AGSUser>> GetUsers();
    }
}
