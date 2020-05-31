using System;
using System.Collections.Generic;
using AGSIdentity.Models.DataModels;

namespace AGSIdentity.Repositories
{
    public interface IUserRepository
    {
        User  Get(string id);
        List<User> GetAll();
        void Delete(string id);
        void Create(User  user);
        void Update(User  user);
    }
}
