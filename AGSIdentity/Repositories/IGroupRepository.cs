using System;
using System.Collections.Generic;
using AGSCommon.Models.DataModels.AGSIdentity;

namespace AGSIdentity.Repositories
{
    public interface IGroupRepository
    {
        AGSGroup  Get(string id);
        List<AGSGroup> GetAll();
        void Delete(string id);
        void Create(AGSGroup  group);
        void Update(AGSGroup  group);
    }
}
