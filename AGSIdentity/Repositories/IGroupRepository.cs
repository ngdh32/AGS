using System;
using System.Collections.Generic;
using AGSIdentity.Models.DataModels;

namespace AGSIdentity.Repositories
{
    public interface IGroupRepository
    {
        Group  Get(string id);
        List<Group> GetAll();
        void Delete(string id);
        void Create(Group  group);
        void Update(Group  group);
    }
}
