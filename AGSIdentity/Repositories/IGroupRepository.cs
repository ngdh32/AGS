using System;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels;

namespace AGSIdentity.Repositories
{
    public interface IGroupRepository
    {
        AGSGroupEntity Get(string id);
        List<string> GetAll();
        void Delete(string id);
        // return newly inserted id
        string Create(AGSGroupEntity group);
        void Update(AGSGroupEntity group);
        //void AddFunctionClaimToGroup(string groupId, string functionClaimId);
        //void RemoveFunctionClaimFromGroup(string groupId, string functionClaimId);
        //List<string> GetFunctionClaimIdsByGroupId(string groupId);
    }
}
