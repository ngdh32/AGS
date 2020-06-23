using System;
using System.Collections.Generic;
using AGSCommon.Models.DataModels.AGSIdentity;
using AGSIdentity.Models.EntityModels;
using AGSIdentity.Models.ExceptionModels;
using AGSIdentity.Repositories;
using AGSIdentity.Services.AuthService;
using AGSIdentity.Services.ExceptionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AGSIdentity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSPolicyConstant)]
    public class FunctionClaimsController : ControllerBase , IBLLController<AGSFunctionClaim>
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
            var result = new List<AGSFunctionClaim>();
            var functionClaimIds = _repository.FunctionClaimRepository.GetAll();
            foreach(var functionClaimId in functionClaimIds)
            {
                var functionClaim = GetModel(functionClaimId);
                if (functionClaim != null)
                {
                    result.Add(functionClaim);
                }
            }
            return new JsonResult(result);
        }


        /// <summary>
        /// Return the specified function claims
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var functionClaim = GetModel(id);
            return new JsonResult(functionClaim); ;
        }


        /// <summary>
        /// Create a function claim. Only users with specified claim are allowed 
        /// </summary>
        /// <param name="functionClaim"></param>
        [HttpPost]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionClaimEditPolicyConstant)]
        public IActionResult Post([FromBody]  AGSFunctionClaim functionClaim)
        {
            var id = SaveOrUpdateModel(functionClaim);
            _repository.Save();
            return Ok(id);
        }

        /// <summary>
        /// Update a function claim. Only users with specified claim are allowed
        /// </summary>
        /// <param name="functionClaim"></param>
        /// <param name="id"></param>
        [HttpPut("{id}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionClaimEditPolicyConstant)]
        public IActionResult Put([FromBody] AGSFunctionClaim functionClaim, string id)
        {
            if (functionClaim.Id == id)
            {
                SaveOrUpdateModel(functionClaim);
                _repository.Save();
                return Ok();
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
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionClaimEditPolicyConstant)]
        public IActionResult DeleteAPI(string id)
        {
            DeleteModel(id);
            _repository.Save();
            return Ok();
        }


        #region BLL
        public AGSFunctionClaim GetModel(string id)
        {
            var entity = _repository.FunctionClaimRepository.Get(id);
            if (entity != null)
            {
                var result = new AGSFunctionClaim()
                {
                    Id = entity.Id,
                    Name = entity.Name
                };
                return result;
            }else
            {
                return null;
            }
            
        }

        public string SaveOrUpdateModel(AGSFunctionClaim functionClaim)
        {
            if (functionClaim == null)
            {
                throw new ArgumentNullException();
            }

            var entity = new AGSFunctionClaimEntity()
            {
                Name = functionClaim.Name
            };
            
            if (functionClaim.Id != null)
            {
                _repository.FunctionClaimRepository.Update(entity);
            }else
            {
                entity.Id = functionClaim.Id;
                functionClaim.Id = _repository.FunctionClaimRepository.Create(entity);
            }
            return functionClaim.Id;
        }

        public void DeleteModel(string id)
        {
            _repository.FunctionClaimRepository.Delete(id);
        }

        #endregion

    }
}
