using System;
using System.Collections.Generic;
using System.Linq;
using AGSIdentity.Attributes;
using AGSIdentity.Helpers;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using AGSIdentity.Models.ViewModels.API.Common;
using AGSIdentity.Models.ViewModels.API.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AGSIdentity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersHelper _usersHelper;

        public UsersController(UsersHelper usersHelper)
        {
            _usersHelper = usersHelper;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>          
        [HttpGet]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSUserReadClaimConstant)]
        public List<AGSUserEntity> Get() {
            var result = _usersHelper.GetAllUsers();
            return result;
        }

        /// <summary>
        /// Get one specified user
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>     
        [HttpGet("{id}")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSUserReadClaimConstant)]
        public AGSUserEntity Get(string id)
        {
            var result = _usersHelper.GetUserById(id);
            return result;
        }

        /// <summary>
        /// Get one specified user's all groups
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>           
        [HttpGet("{id}/groups")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSUserReadClaimConstant)]
        public List<AGSGroupEntity> GetGroups(string id)
        {
            var result = _usersHelper.GetGroupsByUserId(id);
            return result;
        }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPost]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSUserEditClaimConstant)]
        public string Post([FromBody] AGSUserEntity user)
        {
            var result = _usersHelper.CreateUser(user);
            return result;
        }

        /// <summary>
        /// Update one specified user
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPut]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSUserEditClaimConstant)]
        public int Put([FromBody] AGSUserEntity user)
        {
            var result = _usersHelper.UpdateUser(user);
            return result;
        }

        /// <summary>
        /// Delete one specified user
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpDelete("{id}")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSUserEditClaimConstant)]
        public bool Delete(string id) {
            _usersHelper.DeleteUser(id);
            return true;
        }

        /// <summary>
        /// Reset one specified user's password
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPost("{id}/resetpw")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSUserEditClaimConstant)]
        public bool ResetPW(string id)
        {
            var result = _usersHelper.ResetPassword(id);
            return result;
        }


        /// <summary>
        /// Update one specified user's password
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPost("changepw")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSUserChangePasswordClaimConstant)]
        public bool ChangePW([FromBody] ChangePasswordRequestModel changePasswordRequest)
        {
            var userId = HttpContext?.User?.Claims?.Where(c => c.Type == "sub").FirstOrDefault()?.Value ?? "";

            var result = _usersHelper.ChangePassword(userId, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);
            return result;
        }

        
    }
}
