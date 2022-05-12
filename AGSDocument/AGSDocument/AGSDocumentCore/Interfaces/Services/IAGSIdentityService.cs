using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AGSDocumentCore.Models.DTOs.Services;

namespace AGSDocumentCore.Interfaces.Services
{
    public interface IAGSIdentityService
    {
        public Task<List<AGSUserViewModel>> GetUsers();
    }
}
