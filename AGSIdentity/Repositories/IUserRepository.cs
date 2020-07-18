using System;
using System.Collections.Generic;
using AGSCommon.Models.EntityModels.AGSIdentity;
using AGSCommon.Models.ViewModels.AGSIdentity;

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


        bool ValidatePassword(string userId, string password);
        bool ResetPassword(string userId, string defaultPasswordHash);
        bool ChangePassword(string userId, string defaultPassword);
        
    }
}
