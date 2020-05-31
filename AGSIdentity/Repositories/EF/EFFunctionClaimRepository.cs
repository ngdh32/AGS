using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using AGSIdentity.Models.DataModels;
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

        public FunctionClaim Get(int id)
        {
            var selected = (from x in _applicationDbContext.FunctionClaims
                            where x.Id == id
                            select x).FirstOrDefault();
            return selected;
        }

        public List<FunctionClaim> GetAll()
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

        public void Create(FunctionClaim functionClaim)
        {
            _applicationDbContext.FunctionClaims.Add(functionClaim);
        }

        public void Update(FunctionClaim functionClaim)
        {
            _applicationDbContext.FunctionClaims.Update(functionClaim);
        }
    }
}
