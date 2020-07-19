using System;
using System.Collections.Generic;
using AGSCommon.Models.EntityModels.AGSIdentity;
using AGSCommon.Models.EntityModels.Common;
using AGSIdentity.Models.EntityModels;
using AGSIdentity.Repositories;
using AGSIdentity.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AGSIdentity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSPolicyConstant)]
    public class FunctionClaimsController : ControllerBase , IBLLController<AGSFunctionClaimEntity>
    {
        private IRepository _repository { get; set; }

        public FunctionClaimsController(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Return all the function claims
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            var result = new List<AGSFunctionClaimEntity>();
            var functionClaimIds = _repository.FunctionClaimRepository.GetAll();
            foreach(var functionClaimId in functionClaimIds)
            {
                var functionClaim = GetModel(functionClaimId);
                if (functionClaim != null)
                {
                    result.Add(functionClaim);
                }
            }
            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done, result));
        }


        /// <summary>
        /// Return the specified function claims
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var functionClaim = GetModel(id);
            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done, functionClaim));
        }


        /// <summary>
        /// Create a function claim. Only users with specified claim are allowed 
        /// </summary>
        /// <param name="functionClaim"></param>
        [HttpPost]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionClaimEditPolicyConstant))]
        public IActionResult Post([FromBody] AGSFunctionClaimEntity functionClaim)
        {
            var id = SaveModel(functionClaim);
            _repository.Save();
            return AGSResponseFactory.GetAGSResponseJsonResult(id);
        }

        /// <summary>
        /// Update a function claim. Only users with specified claim are allowed
        /// </summary>
        /// <param name="functionClaim"></param>
        /// <param name="id"></param>
        [HttpPut("{id}")]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionClaimEditPolicyConstant))]
        public IActionResult Put([FromBody] AGSFunctionClaimEntity functionClaim, string id)
        {
            if (functionClaim.Id == id)
            {
                var result = UpdateModel(functionClaim);
                _repository.Save();
                return AGSResponseFactory.GetAGSResponseJsonResult();
            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Delete a function claim. Only users with specified claim are allowed
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionClaimEditPolicyConstant))]
        public IActionResult DeleteAPI(string id)
        {
            DeleteModel(id);
            _repository.Save();
            return AGSResponseFactory.GetAGSResponseJsonResult();
        }


        #region BLL
        public AGSFunctionClaimEntity GetModel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            var entity = _repository.FunctionClaimRepository.Get(id);
            return entity;
        }

        public int UpdateModel(AGSFunctionClaimEntity functionClaim)
        {
            if (functionClaim == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(functionClaim.Id))
            {
                throw new ArgumentException();
            }

            
            int result = _repository.FunctionClaimRepository.Update(functionClaim);
            if (result > 0)
            {
                return result;
            }
            else
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }
        }

        public string SaveModel(AGSFunctionClaimEntity functionClaim)
        {
            if (functionClaim == null)
            {
                throw new ArgumentNullException();
            }

            if (!string.IsNullOrEmpty(functionClaim.Id))
            {
                throw new ArgumentException();
            }

            
            string entityId = _repository.FunctionClaimRepository.Create(functionClaim);
            return entityId;
        }

        public void DeleteModel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            _repository.FunctionClaimRepository.Delete(id);
        }

        #endregion

    }
}
