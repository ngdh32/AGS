using System;
using System.Collections.Generic;
using AGSCommon.Models.EntityModels.AGSIdentity;

namespace AGSIdentity.Repositories
{
    public interface IUserRepository
    {
        AGSUserEntity Get(string id);
        List<string> GetAll();
        void Delete(string id);
        // return newly inserted id
        string Create(AGSUserEntity user);
        // return how many records are updated.
        int Update(AGSUserEntity user);
        //List<string> GetGroupIdsByUser(string userId);
        //void AddUserToGroup(string userId, string groupId);
        //void RemoveUserFromGroup(string userId, string groupId);
    }
}
