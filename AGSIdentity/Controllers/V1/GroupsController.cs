﻿using System;
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
        private readonly GroupsHelper _groupsHelper;

        public GroupsController(GroupsHelper groupsHelper)
        {
            _groupsHelper = groupsHelper;
        }

        /// <summary>
        /// Get all groups
        /// </summary>
        [HttpGet]
        [Authorize(Policy = CommonConstant.AGSGroupReadClaimConstant)]
        public IActionResult Get() {
            var result = _groupsHelper.GetAllGroups();
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        /// <summary>
        /// Get one specified group's all function claims
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpGet("{id}/functionclaims")]
        [Authorize(Policy = CommonConstant.AGSGroupReadClaimConstant)]
        public IActionResult GetFunctionClaims(string id)
        {
            var result = _groupsHelper.GetFunctionClaimsByGroupId(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        /// <summary>
        /// Get a specified groups
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpGet("{id}")]
        [Authorize(Policy = CommonConstant.AGSGroupReadClaimConstant)]
                public IActionResult Get(string id) {
            var result = _groupsHelper.GetGroupById(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }
        

        /// <summary>
        /// Get all the users in a specified group
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpGet("{id}/users")]
        [Authorize(Policy = CommonConstant.AGSGroupReadClaimConstant)]
        public IActionResult GetAllUsersinGroup(string id) {
            var result = _groupsHelper.GetUsersByGroupId(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        /// <summary>
        /// Create a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="group"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPost]
        [Authorize(Policy = CommonConstant.AGSGroupEditClaimConstant)]
        public IActionResult Post([FromBody] AGSGroupEntity group)
        {
            var result = _groupsHelper.CreateGroup(group);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        

        /// <summary>
        /// Update a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="group"></param>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPut]
        [Authorize(Policy = CommonConstant.AGSGroupEditClaimConstant)]
        public IActionResult Put([FromBody] AGSGroupEntity group)
        {
            var result = _groupsHelper.UpdateGroup(group);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        /// <summary>
        /// Delete a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpDelete("{id}")]
        [Authorize(Policy = CommonConstant.AGSGroupEditClaimConstant)]
        public IActionResult Delete(string id)
        {
            _groupsHelper.DeleteGroup(id);
            return AGSResponseFactory.GetAGSResponseJsonResult();
        }
    }
}
