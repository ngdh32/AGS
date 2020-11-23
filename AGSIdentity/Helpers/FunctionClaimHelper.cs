using System;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using AGSIdentity.Models.ViewModels.API.Common;
using AGSIdentity.Repositories;

namespace AGSIdentity.Helpers
{
    public class FunctionClaimHelper
    {
        private IRepository _repository { get; set; }

        public FunctionClaimHelper(IRepository repository)
        {
            _repository = repository;    
        }

        public AGSFunctionClaimEntity GetFunctionClaimById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            var result = _repository.FunctionClaimRepository.Get(id);
            return result;
        }

        public List<AGSFunctionClaimEntity> GetAllFunctionClaims()
        {
            var result = new List<AGSFunctionClaimEntity>();

            var functionClaimIds = _repository.FunctionClaimRepository.GetAll();
            foreach (var functionClaimId in functionClaimIds)
            {
                var entity = _repository.FunctionClaimRepository.Get(functionClaimId);
                if (entity != null)
                {
                    result.Add(entity);
                }
            }

            return result;
        }

        public string CreateFunctionClaim(AGSFunctionClaimEntity model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (!string.IsNullOrEmpty(model.Id))
            {
                throw new ArgumentException();
            }

            var result = _repository.FunctionClaimRepository.Create(model);
            _repository.Save();
            return result;
        }

        public int UpdateFunctionClaim(AGSFunctionClaimEntity model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                throw new ArgumentException();
            }


            var result = _repository.FunctionClaimRepository.Update(model);
            if (result == 0)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }

            _repository.Save();
            return result;
        }

        public void DeleteFunctionClaim(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            _repository.FunctionClaimRepository.Delete(id);
            _repository.Save();
        }
    }
}
