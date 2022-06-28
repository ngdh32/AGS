using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AGSDocumentCore.Models.DTOs.Services;
using AGSDocumentCore.Models.Entities;

namespace AGSDocumentCore.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<List<AGSUserViewModel>> GetUsers();
    }
}
