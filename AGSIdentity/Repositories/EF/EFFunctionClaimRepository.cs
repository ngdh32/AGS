using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using AGSCommon.Models.DataModels.AGSIdentity;
using System.Linq;
using AGSIdentity.Models.EntityModels.EF;
using AGSIdentity.Models.EntityModels;

namespace AGSIdentity.Repositories.EF
{
    public class EFFunctionClaimRepository : IFunctionClaimRepository
    {
        private ApplicationDbContext _applicationDbContext { get; set; }

        public EFFunctionClaimRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public AGSFunctionClaimEntity Get(string id)
        {
            var selected = (from x in _applicationDbContext.FunctionClaims
                            where x.Id == id
                            select x).FirstOrDefault();
            if (selected != null)
            {
                var result = new AGSFunctionClaimEntity()
                {
                    Id = selected.Id,
                    Name = selected.Name
                };
                return result;
            }
            else
            {
                return null;
            }
            
        }

        public List<string> GetAll()
        {
            var result = new List<string>();
            foreach(var fc in _applicationDbContext.FunctionClaims.ToList())
            {
                result.Add(fc.Id);
            }
            return result;
        }

        public void Delete(string id)
        {
            var selected = (from x in _applicationDbContext.FunctionClaims
                            where x.Id == id
                            select x).FirstOrDefault();
            if (selected != null)
            {

                _applicationDbContext.FunctionClaims.Remove(selected);
            }
        }

        public string Create(AGSFunctionClaimEntity functionClaim)
        {
            FunctionClaim result = new FunctionClaim()
            {
                Id = functionClaim.Id,
                Name = functionClaim.Name
            };
            _applicationDbContext.FunctionClaims.Add(result);
            return result.Id;

        }

        public void Update(AGSFunctionClaimEntity functionClaim)
        {
            var selected = (from x in _applicationDbContext.FunctionClaims
                            where x.Id == functionClaim.Id
                            select x).FirstOrDefault();
            if (selected != null)
            {
                selected.Name = functionClaim.Name;
                _applicationDbContext.FunctionClaims.Update(selected);
            }
            
        }
    }
}
