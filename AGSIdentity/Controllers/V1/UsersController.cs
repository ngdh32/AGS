using System;
using System.Linq;
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
        [Authorize(Policy = CommonConstant.AGSUserReadClaimConstant)]
        public IActionResult Get() {
            var result = _usersHelper.GetAllUsers();
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        /// <summary>
        /// Get one specified user
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>     
        [HttpGet("{id}")]
        [Authorize(Policy = CommonConstant.AGSUserReadClaimConstant)]
        public IActionResult Get(string id)
        {
            var user = _usersHelper.GetUserById(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(user);
            
        }

        /// <summary>
        /// Get one specified user's all groups
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>           
        [HttpGet("{id}/groups")]
        [Authorize(Policy = CommonConstant.AGSUserReadClaimConstant)]
        public IActionResult GetGroups(string id)
        {
            var result = _usersHelper.GetGroupsByUserId(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPost]
        [Authorize(Policy = CommonConstant.AGSUserEditClaimConstant)]
        public IActionResult Post([FromBody] AGSUserEntity user)
        {
            try
            {
                var id = _usersHelper.CreateUser(user);
                return AGSResponseFactory.GetAGSResponseJsonResult(id);
            }catch(AGSException ex)
            {
                return AGSResponseFactory.GetAGSExceptionJsonResult(ex);
            }catch(Exception ex)
            {
                return StatusCode(500);
            }

        }

        /// <summary>
        /// Update one specified user
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPut]
        [Authorize(Policy = CommonConstant.AGSUserEditClaimConstant)]
        public IActionResult Put([FromBody] AGSUserEntity user) {
            try
            {
                var result = _usersHelper.UpdateUser(user);
                return AGSResponseFactory.GetAGSResponseJsonResult();
            }catch(AGSException ex)
            {
                return AGSResponseFactory.GetAGSExceptionJsonResult(ex);
            }catch(Exception ex)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Delete one specified user
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpDelete("{id}")]
        [Authorize(Policy = CommonConstant.AGSUserEditClaimConstant)]
        public IActionResult Delete(string id) {
            _usersHelper.DeleteUser(id);
            return AGSResponseFactory.GetAGSResponseJsonResult();
        }

        /// <summary>
        /// Reset one specified user's password
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPost("{id}/resetpw")]
        [Authorize(Policy = CommonConstant.AGSUserEditClaimConstant)]
        public IActionResult ResetPW(string id)
        {
            var result = _usersHelper.ResetPassword(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        /// <summary>
        /// Update one specified user's password
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct function claims</response>  
        [HttpPost("changepw")]
        [Authorize(Policy = CommonConstant.AGSUserChangePasswordClaimConstant)]
        public IActionResult ChangePW([FromBody] ChangePasswordRequestModel changePasswordRequest)
        {
            var userId = HttpContext?.User?.Claims?.Where(c => c.Type == "sub").FirstOrDefault()?.Value ?? "";

            var result = _usersHelper.ChangePassword(userId, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        
    }
}
