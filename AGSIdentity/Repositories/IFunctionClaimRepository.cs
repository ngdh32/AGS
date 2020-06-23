using System;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels;

namespace AGSIdentity.Repositories
{
    public interface IFunctionClaimRepository
    {
        AGSFunctionClaimEntity Get(string id);
        List<string> GetAll();
        void Delete(string id);
        // return newly inserted id
        string Create(AGSFunctionClaimEntity functionClaim);
        void Update(AGSFunctionClaimEntity functionClaim);
    }
}
