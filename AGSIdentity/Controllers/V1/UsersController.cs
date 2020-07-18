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

        public UsersController(IRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        [HttpGet]
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
        public IActionResult Get(string id)
        {
            var user = GetModel(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(user);
            
        }


        [HttpGet("{id}/groups")]
        public IActionResult GetGroups(string id)
        {
            var result = GetAGSGroupEntitiesByUserId(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        [HttpPost]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditPolicyConstant)]
        public IActionResult Post([FromBody] AGSUserEntity user)
        {
            var id = SaveModel(user);
            _repository.Save();
            return AGSResponseFactory.GetAGSResponseJsonResult(id);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditPolicyConstant)]
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
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditPolicyConstant)]
        public IActionResult Delete(string id) {
            DeleteModel(id);
            _repository.Save();
            return AGSResponseFactory.GetAGSResponseJsonResult();
        }

        [HttpDelete("{id}/resetpw")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditPolicyConstant)]
        public IActionResult ResetPW(string id)
        {
            var result = ResetPassword(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        [HttpDelete("{id}/changepw")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditPolicyConstant)]
        public IActionResult ChangePW([FromBody] ChangeUserPasswordViewModel changeUserPasswordView, string id)
        {
            if (changeUserPasswordView.UserId == id)
            {
                var result = ChangePassword(changeUserPasswordView);
                return AGSResponseFactory.GetAGSResponseJsonResult(result);
            }else
            {
                return BadRequest();
            }
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

            
            var result = _repository.UserRepository.Update(model);
            if (result > 0)
            {
                return result;
            }
            else
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }
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
