using System;
using System.Collections.Generic;
using AGSIdentity.Models.DataModels;

namespace AGSIdentity.Repositories
{
    public interface IFunctionClaimRepository
    {
        FunctionClaim Get(int id);
        List<FunctionClaim> GetAll();
        void Delete(int id);
        void Create(FunctionClaim functionClaim);
        void Update(FunctionClaim functionClaim);
    }
}
