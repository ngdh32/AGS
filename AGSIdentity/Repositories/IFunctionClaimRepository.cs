using System;
using System.Collections.Generic;
using AGSCommon.Models.DataModels.AGSIdentity;

namespace AGSIdentity.Repositories
{
    public interface IFunctionClaimRepository
    {
        AGSFunctionClaim Get(int id);
        List<AGSFunctionClaim> GetAll();
        void Delete(int id);
        void Create(AGSFunctionClaim functionClaim);
        void Update(AGSFunctionClaim functionClaim);
    }
}
