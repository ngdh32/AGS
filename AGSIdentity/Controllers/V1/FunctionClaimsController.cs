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
        private readonly FunctionClaimHelper _functionClaimHelper;

        public FunctionClaimsController(FunctionClaimHelper functionClaimHelper)
        {
            _functionClaimHelper = functionClaimHelper;
        }

        /// <summary>
        /// Return all the function claims
        /// </summary>
        [HttpGet]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimReadClaimConstant)]
        public IActionResult Get()
        {
            var result = _functionClaimHelper.GetAllFunctionClaims();
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        /// <summary>
        /// Return the specified function claims
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimReadClaimConstant)]
        public IActionResult Get(string id)
        {
            var result = _functionClaimHelper.GetFunctionClaimById(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        /// <summary>
        /// Create a function claim. Only users with specified claim are allowed 
        /// </summary>
        /// <param name="functionClaim"></param>
        [HttpPost]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimEditClaimConstant)]
        public IActionResult Post([FromBody] AGSFunctionClaimEntity functionClaim)
        {
            var result = _functionClaimHelper.CreateFunctionClaim(functionClaim);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        /// <summary>
        /// Update a function claim. Only users with specified claim are allowed
        /// </summary>
        /// <param name="functionClaim"></param>
        /// <param name="id"></param>
        [HttpPut]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimEditClaimConstant)]
        public IActionResult Put([FromBody] AGSFunctionClaimEntity functionClaim)
        {
            var result = _functionClaimHelper.UpdateFunctionClaim(functionClaim);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        /// <summary>
        /// Delete a function claim. Only users with specified claim are allowed
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [Authorize(Policy = CommonConstant.AGSFunctionClaimEditClaimConstant)]
        public IActionResult Delete(string id)
        {
            _functionClaimHelper.DeleteFunctionClaim(id);
            return AGSResponseFactory.GetAGSResponseJsonResult();
        }

    }
}
