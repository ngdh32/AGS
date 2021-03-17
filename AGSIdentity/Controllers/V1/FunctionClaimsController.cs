using System;
using System.Collections.Generic;
using AGSIdentity.Helpers;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using AGSIdentity.Models.ViewModels.API.Common;
using AGSIdentity.Repositories;
using AGSIdentity.Services.AuthService.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AGSIdentity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class FunctionClaimsController : ControllerBase
    {
        private readonly FunctionClaimsHelper _functionClaimsHelper;

        public FunctionClaimsController(FunctionClaimsHelper functionClaimsHelper)
        {
            _functionClaimsHelper = functionClaimsHelper;
        }

        /// <summary>
        /// Return all the function claims
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpGet]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimReadClaimConstant)]
        public IActionResult Get()
        {
            var result = _functionClaimsHelper.GetAllFunctionClaims();
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        /// <summary>
        /// Return the specified function claims
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpGet("{id}")]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimReadClaimConstant)]
        public IActionResult Get(string id)
        {
            var result = _functionClaimsHelper.GetFunctionClaimById(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        /// <summary>
        /// Create a function claim. Only users with specified claim are allowed 
        /// </summary>
        /// <param name="functionClaim"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPost]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimEditClaimConstant)]
        public IActionResult Post([FromBody] AGSFunctionClaimEntity functionClaim)
        {
            var result = _functionClaimsHelper.CreateFunctionClaim(functionClaim);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        /// <summary>
        /// Update a function claim. Only users with specified claim are allowed
        /// </summary>
        /// <param name="functionClaim"></param>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPut]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimEditClaimConstant)]
        public IActionResult Put([FromBody] AGSFunctionClaimEntity functionClaim)
        {
            var result = _functionClaimsHelper.UpdateFunctionClaim(functionClaim);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        /// <summary>
        /// Delete a function claim. Only users with specified claim are allowed
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpDelete("{id}")]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimEditClaimConstant)]
        public IActionResult Delete(string id)
        {
            _functionClaimsHelper.DeleteFunctionClaim(id);
            return AGSResponseFactory.GetAGSResponseJsonResult();
        }

    }
}
