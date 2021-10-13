using System;
using System.Collections.Generic;
using AGSIdentity.Helpers;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using AGSIdentity.Models.ViewModels.API.Common;
using AGSIdentity.Repositories;
using AGSIdentity.Services.AuthService.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AGSIdentity.Attributes;

namespace AGSIdentity.Controllers.V1
{
    public class FunctionClaimsController : V1BaseController
    {
        private readonly FunctionClaimsHelper _functionClaimsHelper;

        public FunctionClaimsController(FunctionClaimsHelper functionClaimsHelper) : base()
        {
            _functionClaimsHelper = functionClaimsHelper;
        }

        /// <summary>
        /// Return all the function claims
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpGet]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimReadClaimConstant)]
        public List<AGSFunctionClaimEntity> Get()
        {
            var result = _functionClaimsHelper.GetAllFunctionClaims();
            return result;
        }


        /// <summary>
        /// Return the specified function claims
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpGet("{id}")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimReadClaimConstant)]
        public AGSFunctionClaimEntity Get(string id)
        {
            var result = _functionClaimsHelper.GetFunctionClaimById(id);
            return result;
        }


        /// <summary>
        /// Create a function claim. Only users with specified claim are allowed 
        /// </summary>
        /// <param name="functionClaim"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPost]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimEditClaimConstant)]
        public string Post([FromBody] AGSFunctionClaimEntity functionClaim)
        {
            var result = _functionClaimsHelper.CreateFunctionClaim(functionClaim);
            return result;
        }

        /// <summary>
        /// Update a function claim. Only users with specified claim are allowed
        /// </summary>
        /// <param name="functionClaim"></param>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPut]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimEditClaimConstant)]
        public int Put([FromBody] AGSFunctionClaimEntity functionClaim)
        {
            var result = _functionClaimsHelper.UpdateFunctionClaim(functionClaim);
            return result;
        }


        /// <summary>
        /// Delete a function claim. Only users with specified claim are allowed
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpDelete("{id}")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimEditClaimConstant)]
        public bool Delete(string id)
        {
            _functionClaimsHelper.DeleteFunctionClaim(id);
            return true;
        }

    }
}
