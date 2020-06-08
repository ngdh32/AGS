using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using AGSCommon.Models.DataModels.AGSIdentity;
using System.Linq;

namespace AGSIdentity.Repositories.EF
{
    public class EFFunctionClaimRepository : IFunctionClaimRepository
    {
        private ApplicationDbContext _applicationDbContext { get; set; }

        public EFFunctionClaimRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public AGSFunctionClaim Get(int id)
        {
            var selected = (from x in _applicationDbContext.FunctionClaims
                            where x.Id == id
                            select x).FirstOrDefault();
            return selected;
        }

        public List<AGSFunctionClaim> GetAll()
        {
            return _applicationDbContext.FunctionClaims.ToList();
        }

        public void Delete(int id)
        {
            var selected = (from x in _applicationDbContext.FunctionClaims
                            where x.Id == id
                            select x).FirstOrDefault();
            if (selected != null)
            {

                _applicationDbContext.FunctionClaims.Remove(selected);
            }
        }

        public void Create(AGSFunctionClaim functionClaim)
        {
            _applicationDbContext.FunctionClaims.Add(functionClaim);
        }

        public void Update(AGSFunctionClaim functionClaim)
        {
            _applicationDbContext.FunctionClaims.Update(functionClaim);
        }
    }
}
