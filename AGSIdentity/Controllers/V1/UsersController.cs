using System;
using System.Collections.Generic;
using System.Linq;
using AGSCommon.Models.EntityModels.AGSIdentity;
using AGSCommon.Models.EntityModels.Common;
using AGSCommon.Models.ViewModels.AGSIdentity;
using AGSIdentity.Models.EntityModels;
using AGSIdentity.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AGSIdentity.Services.AuthService;
using System.Threading;

namespace AGSIdentity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSPolicyConstant)]
    public class UsersController : ControllerBase
    {
        private IRepository _repository { get; set; }
        private IConfiguration _configuration { get; set; }
        private IAuthService _authService { get; set; }

        public UsersController(IRepository repository, IConfiguration configuration, IAuthService authService)
        {
            _repository = repository;
            _configuration = configuration;
            _authService = authService;
        }

        [HttpGet]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserReadClaimConstant))]
        public IActionResult Get() {
            var result = new List<AGSUserEntity>();
            var userIds = _repository.UserRepository.GetAll();
            if (userIds != null)
            {
                foreach(var userId in userIds)
                {
                    var user = GetModel(userId);
                    if (user != null)
                    {
                        result.Add(user);
                    }
                }
            }

            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserReadClaimConstant))]
        public IActionResult Get(string id)
        {
            var user = GetModel(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(user);
            
        }


        [HttpGet("{id}/groups")]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserReadClaimConstant))]
        public IActionResult GetGroups(string id)
        {
            var result = GetAGSGroupEntitiesByUserId(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        [HttpPost]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditClaimConstant))]
        public IActionResult Post([FromBody] AGSUserEntity user)
        {
            var id = SaveModel(user);
            _repository.Save();
            return AGSResponseFactory.GetAGSResponseJsonResult(id);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditClaimConstant))]
        public IActionResult Put([FromBody] AGSUserEntity user, string id) {
            if (user.Id != id)
            {
                return BadRequest();
            }
            else
            {
                int result = UpdateModel(user);
                _repository.Save();
                return AGSResponseFactory.GetAGSResponseJsonResult();
            }

        }

        [HttpDelete("{id}")]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditClaimConstant))]
        public IActionResult Delete(string id) {
            DeleteModel(id);
            _repository.Save();
            return AGSResponseFactory.GetAGSResponseJsonResult();
        }

        [HttpPost("{id}/resetpw")]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditClaimConstant))]
        public IActionResult ResetPW(string id)
        {
            var result = ResetPassword(id);
            _repository.Save();
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        [HttpPost("{id}/changepw")]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditClaimConstant))]
        public IActionResult ChangePW([FromBody] ChangeUserPasswordViewModel changeUserPasswordView, string id)
        {
            if (changeUserPasswordView.UserId != id)
            {
                return BadRequest();
            }

            var result = ChangePassword(changeUserPasswordView);
            _repository.Save();
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        public int UpdateModel(AGSUserEntity model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                throw new ArgumentException();
            }

            
            var selected = _repository.UserRepository.Get(model.Id);
            if (selected != null)
            {
                // Not allow to change admin username
                if (selected.Username == AGSCommon.CommonConstant.AGSIdentityConstant.AGSAdminName)
                {
                    if (model.Username != AGSCommon.CommonConstant.AGSIdentityConstant.AGSAdminName)
                    {
                        throw new ArgumentException();
                    }
                }else
                // Not allow to change the username to admin
                {
                    if (model.Username == AGSCommon.CommonConstant.AGSIdentityConstant.AGSAdminName)
                    {
                        throw new ArgumentException();
                    }
                }
            }
            else
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }


            var result = _repository.UserRepository.Update(model);
            return result;
        }

        public string SaveModel(AGSUserEntity model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = AGSCommon.CommonFunctions.GenerateId();
            }

            // Not allow to create admin
            if (model.Username == AGSCommon.CommonConstant.AGSIdentityConstant.AGSAdminName)
            {
                throw new ArgumentException();
            }

            string entityId = _repository.UserRepository.Create(model);
            _repository.UserRepository.ResetPassword(entityId, _configuration["default_user_password"]);

            return entityId;
        }

        public void DeleteModel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            // Not allow to delete admin
            var selected = _repository.UserRepository.Get(id);
            if (selected != null)
            {
                if (selected.Username == AGSCommon.CommonConstant.AGSIdentityConstant.AGSAdminName)
                {
                    throw new ArgumentException();
                }
            }

            _repository.UserRepository.Delete(id);
        }

        public AGSUserEntity GetModel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            var selected = _repository.UserRepository.Get(id);
            return selected;
        }

        public bool ResetPassword(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException();
            }

            var selected = _repository.UserRepository.Get(userId);
            if (selected != null)
            {
                bool result = _repository.UserRepository.ResetPassword(userId, _configuration["default_user_password"]);
                return result;
            }
            else
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }
        }

        public List<AGSGroupEntity> GetAGSGroupEntitiesByUserId(string userId)
        {
            var result = new List<AGSGroupEntity>();
            var selected = _repository.UserRepository.Get(userId);
            if (selected != null)
            {
                foreach(var groupId in selected.GroupIds)
                {
                    var group = _repository.GroupRepository.Get(groupId);
                    if (group != null)
                    {
                        result.Add(group);
                    }
                }

                return result;
            }else
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }
        }



        public bool ChangePassword(ChangeUserPasswordViewModel changeUserPasswordView)
        {
            if (string.IsNullOrEmpty(changeUserPasswordView.UserId))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(changeUserPasswordView.OldPassword))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(changeUserPasswordView.NewPassword))
            {
                throw new ArgumentNullException();
            }

            if (changeUserPasswordView.UserId != _authService.GetCurrentUserId())
            {
                throw new ArgumentException();
            }


            bool result = _repository.UserRepository.ValidatePassword(changeUserPasswordView.UserId, changeUserPasswordView.OldPassword);
            if (result)
            {
                bool changeResult = _repository.UserRepository.ChangePassword(changeUserPasswordView.UserId, changeUserPasswordView.NewPassword);
                return changeResult;
            }else
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.UsernameOrPasswordError);
            }


        }
    }
}
