﻿using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using System.Linq;
using AGSIdentity.Models.EntityModels;
using AGSIdentity.Models.EntityModels.AGSIdentity.EF;
using AGSIdentity.Models.EntityModels.AGSIdentity;

namespace AGSIdentity.Repositories.EF
{
    public class EFFunctionClaimsRepository : IFunctionClaimsRepository
    {
        private EFApplicationDbContext _applicationDbContext { get; set; }

        public EFFunctionClaimsRepository(EFApplicationDbContext applicationDbContext)
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
                var result = GetFunctionClaimEntity(selected);
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
            var result = new EFFunctionClaim();
            UpdateFunctionClaim(functionClaim, result);
            result.Id = CommonConstant.GenerateId();
            _applicationDbContext.FunctionClaims.Add(result);
            return result.Id;

        }

        public int Update(AGSFunctionClaimEntity functionClaim)
        {
            var selected = (from x in _applicationDbContext.FunctionClaims
                            where x.Id == functionClaim.Id
                            select x).FirstOrDefault();
            if (selected != null)
            {
                UpdateFunctionClaim(functionClaim, selected);
                _applicationDbContext.FunctionClaims.Update(selected);
                return 1;
            }else
            {
                return 0;
            }
            
        }

        public AGSFunctionClaimEntity GetFunctionClaimEntity(EFFunctionClaim functionClaim)
        {
            var result = new AGSFunctionClaimEntity()
            {
                Id = functionClaim.Id,
                Name = functionClaim.Name,
                Description = functionClaim.Description
            };

            return result;
        }

        public void UpdateFunctionClaim(AGSFunctionClaimEntity functionClaimEntity, EFFunctionClaim efFunctionClaim)
        {
            efFunctionClaim.Id = functionClaimEntity.Id;
            efFunctionClaim.Name = functionClaimEntity.Name;
            efFunctionClaim.Description = functionClaimEntity.Description;
        }
    }
}
