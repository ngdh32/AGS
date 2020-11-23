using System;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels.AGSIdentity;

namespace AGSIdentity.Repositories
{
    public interface IGroupRepository
    {
        AGSGroupEntity Get(string id);
        List<string> GetAll();
        void Delete(string id);
        // return newly inserted id
        string Create(AGSGroupEntity group);
        // return how many records are updated.
        int Update(AGSGroupEntity group);
        //void AddFunctionClaimToGroup(string groupId, string functionClaimId);
        //void RemoveFunctionClaimFromGroup(string groupId, string functionClaimId);
        //List<string> GetFunctionClaimIdsByGroupId(string groupId);
    }
}
