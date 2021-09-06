using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AGSIdentity.Attributes;
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
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSGroupReadClaimConstant)]
        public List<AGSGroupEntity> Get() {
            var result = _groupsHelper.GetAllGroups();
            return result;
        }

        /// <summary>
        /// Get one specified group's all function claims
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpGet("{id}/functionclaims")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSGroupReadClaimConstant)]
        public List<AGSFunctionClaimEntity> GetFunctionClaims(string id)
        {
            var result = _groupsHelper.GetFunctionClaimsByGroupId(id);
            return result;
        }

        /// <summary>
        /// Get a specified groups
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpGet("{id}")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSGroupReadClaimConstant)]
        public AGSGroupEntity Get(string id) {
            var result = _groupsHelper.GetGroupById(id);
            return result;
        }
        

        /// <summary>
        /// Get all the users in a specified group
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpGet("{id}/users")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSGroupReadClaimConstant)]
        public List<AGSUserEntity> GetAllUsersinGroup(string id) {
            var result = _groupsHelper.GetUsersByGroupId(id);
            return result;
        }


        /// <summary>
        /// Create a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="group"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPost]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSGroupEditClaimConstant)]
        public string Post([FromBody] AGSGroupEntity group)
        {
            var result = _groupsHelper.CreateGroup(group);
            return result;
        }

        

        /// <summary>
        /// Update a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="group"></param>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPut]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSGroupEditClaimConstant)]
        public int Put([FromBody] AGSGroupEntity group)
        {
            var result = _groupsHelper.UpdateGroup(group);
            return result;
        }


        /// <summary>
        /// Delete a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpDelete("{id}")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSGroupEditClaimConstant)]
        public bool Delete(string id)
        {
            _groupsHelper.DeleteGroup(id);
            return true;
        }
    }
}
