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

        [HttpGet]
        [Authorize(Policy = CommonConstant.AGSUserReadClaimConstant)]
        public IActionResult Get() {
            var result = _usersHelper.GetAllUsers();
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = CommonConstant.AGSUserReadClaimConstant)]
        public IActionResult Get(string id)
        {
            var user = _usersHelper.GetUserById(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(user);
            
        }


        [HttpGet("{id}/groups")]
        [Authorize(Policy = CommonConstant.AGSUserReadClaimConstant)]
        public IActionResult GetGroups(string id)
        {
            var result = _usersHelper.GetGroupsByUserId(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


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

        [HttpDelete("{id}")]
        [Authorize(Policy = CommonConstant.AGSUserEditClaimConstant)]
        public IActionResult Delete(string id) {
            _usersHelper.DeleteUser(id);
            return AGSResponseFactory.GetAGSResponseJsonResult();
        }

        [HttpPost("{id}/resetpw")]
        [Authorize(Policy = CommonConstant.AGSUserEditClaimConstant)]
        public IActionResult ResetPW(string id)
        {
            var result = _usersHelper.ResetPassword(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        [HttpPost("changepw")]
        [Authorize]
        public IActionResult ChangePW([FromBody] ChangePasswordRequestModel changePasswordRequest)
        {
            var userId = HttpContext?.User?.Claims?.Where(c => c.Type == "name").FirstOrDefault()?.Value ?? "";

            var result = _usersHelper.ChangePassword(userId, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        
    }
}
