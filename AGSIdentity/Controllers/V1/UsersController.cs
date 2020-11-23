using System;
using System.Collections.Generic;
using System.Linq;
using AGSIdentity.Models.EntityModels;
using AGSIdentity.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AGSIdentity.Services.AuthService;
using System.Threading;
using AGSIdentity.Services.AuthService.Identity;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using AGSIdentity.Models.ViewModels.API.Users;
using AGSIdentity.Helpers;

namespace AGSIdentity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserHelper _userHelper;

        public UsersController(UserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        [HttpGet]
        [FunctionClaimAuth(CommonConstant.AGSUserReadClaimConstant)]
        public IActionResult Get() {
            var result = _userHelper.GetAllUsers();
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        [HttpGet("{id}")]
        [FunctionClaimAuth(CommonConstant.AGSUserReadClaimConstant)]
        public IActionResult Get(string id)
        {
            var user = _userHelper.GetUserById(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(user);
            
        }


        [HttpGet("{id}/groups")]
        [FunctionClaimAuth(CommonConstant.AGSUserReadClaimConstant)]
        public IActionResult GetGroups(string id)
        {
            var result = _userHelper.GetGroupsByUserId(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        [HttpPost]
        [FunctionClaimAuth(CommonConstant.AGSUserEditClaimConstant)]
        public IActionResult Post([FromBody] AGSUserEntity user)
        {
            var id = _userHelper.CreateUser(user);
            return AGSResponseFactory.GetAGSResponseJsonResult(id);
        }

        [HttpPut]
        [FunctionClaimAuth(CommonConstant.AGSUserReadClaimConstant)]
        public IActionResult Put([FromBody] AGSUserEntity user) {
            var result = _userHelper.UpdateUser(user);
            return AGSResponseFactory.GetAGSResponseJsonResult();
        }

        [HttpDelete("{id}")]
        [FunctionClaimAuth(CommonConstant.AGSUserReadClaimConstant)]
        public IActionResult Delete(string id) {
            _userHelper.DeleteUser(id);
            return AGSResponseFactory.GetAGSResponseJsonResult();
        }

        [HttpPost("{id}/resetpw")]
        [FunctionClaimAuth(CommonConstant.AGSUserReadClaimConstant)]
        public IActionResult ResetPW(string id)
        {
            var result = _userHelper.ResetPassword(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        [HttpPost("changepw")]
        [Authorize]
        public IActionResult ChangePW([FromBody] ChangePasswordRequestModel changePasswordRequest)
        {
            var userId = HttpContext?.User?.Claims?.Where(c => c.Type == "name").FirstOrDefault()?.Value ?? "";

            var result = _userHelper.ChangePassword(userId, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        
    }
}
