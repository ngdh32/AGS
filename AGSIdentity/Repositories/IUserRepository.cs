using System;
using System.Collections.Generic;
using AGSCommon.Models.DataModels.AGSIdentity;

namespace AGSIdentity.Repositories
{
    public interface IUserRepository
    {
        AGSUser  Get(string id);
        List<AGSUser> GetAll();
        void Delete(string id);
        void Create(AGSUser  user);
        void Update(AGSUser  user);
    }
}
