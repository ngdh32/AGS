using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AGSIdentity.Helpers;
using AGSIdentity.Models.EntityModels;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using AGSIdentity.Repositories;
using AGSIdentity.Services.AuthService;
using AGSIdentity.Services.AuthService.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AGSIdentity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupsController : ControllerBase 
    {
        private readonly GroupHelper _groupHelper;

        public GroupsController(GroupHelper groupHelper)
        {
            _groupHelper = groupHelper;
        }

        /// <summary>
        /// Get all groups
        /// </summary>
        [HttpGet]
        [Authorize(Policy = CommonConstant.AGSGroupReadClaimConstant)]
        public IActionResult Get() {
            var result = _groupHelper.GetAllGroups();
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        [HttpGet("{id}/functionclaims")]
        [Authorize(Policy = CommonConstant.AGSGroupReadClaimConstant)]
        public IActionResult GetFunctionClaims(string id)
        {
            var result = _groupHelper.GetFunctionClaimsByGroupId(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = CommonConstant.AGSGroupReadClaimConstant)]
        /// <summary>
        /// Get a specified groups
        /// </summary>
        public IActionResult Get(string id) {
            var result = _groupHelper.GetGroupById(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        


        /// <summary>
        /// Get all the users in a specified group
        /// </summary>
        [HttpGet("{id}/users")]
        [Authorize(Policy = CommonConstant.AGSGroupReadClaimConstant)]
        public IActionResult GetAllUsersinGroup(string id) {
            var result = _groupHelper.GetUsersByGroupId(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        /// <summary>
        /// Create a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="group"></param>
        [HttpPost]
        [Authorize(Policy = CommonConstant.AGSGroupEditClaimConstant)]
        public IActionResult Post([FromBody] AGSGroupEntity group)
        {
            var result = _groupHelper.CreateGroup(group);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        

        /// <summary>
        /// Update a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="group"></param>
        /// <param name="id"></param>
        [HttpPut]
        [Authorize(Policy = CommonConstant.AGSGroupEditClaimConstant)]
        public IActionResult Put([FromBody] AGSGroupEntity group)
        {
            var result = _groupHelper.UpdateGroup(group);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        /// <summary>
        /// Delete a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [Authorize(Policy = CommonConstant.AGSGroupEditClaimConstant)]
        public IActionResult Delete(string id)
        {
            _groupHelper.DeleteGroup(id);
            return AGSResponseFactory.GetAGSResponseJsonResult();
        }
    }
}
