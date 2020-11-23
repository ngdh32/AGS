using System;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using AGSIdentity.Models.ViewModels.API.Common;
using AGSIdentity.Repositories;
using Microsoft.Extensions.Configuration;

namespace AGSIdentity.Helpers
{
    public class UserHelper
    {
        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;

        public UserHelper(IRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public int UpdateUser(AGSUserEntity model)
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
            if (selected == null)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }

            // Not allow to change admin username
            if (selected.Username == CommonConstant.AGSAdminName && model.Username != CommonConstant.AGSAdminName)
            {
                throw new ArgumentException();
            }

            // Not allow to change the username to admin
            if (selected.Username != CommonConstant.AGSAdminName && model.Username == CommonConstant.AGSAdminName)
            {
                throw new ArgumentException();
            }

            var result = _repository.UserRepository.Update(model);
            _repository.Save();
            return result;
        }

        public string CreateUser(AGSUserEntity model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (!string.IsNullOrEmpty(model.Id))
            {
                throw new ArgumentException();
            }

            // Not allow to create admin
            if (model.Username == CommonConstant.AGSAdminName)
            {
                throw new ArgumentException();
            }

            string result = _repository.UserRepository.Create(model);
            // set the default password
            _repository.UserRepository.ResetPassword(result, _configuration["default_user_password"]);
            _repository.Save();
            return result;
        }

        public void DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            // Not allow to delete admin
            var selected = _repository.UserRepository.Get(id);
            if (selected != null)
            {
                if (selected.Username == CommonConstant.AGSAdminName)
                {
                    throw new ArgumentException();
                }
            }

            _repository.UserRepository.Delete(id);
            _repository.Save();
        }

        public AGSUserEntity GetUserById(string id)
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
            if (selected == null)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }

            bool result = _repository.UserRepository.ResetPassword(userId, _configuration["default_user_password"]);
            _repository.Save();
            return result;
        }

        public List<AGSGroupEntity> GetGroupsByUserId(string userId)
        {
            var result = new List<AGSGroupEntity>();
            var selected = _repository.UserRepository.Get(userId);

            if(selected == null)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }

            foreach (var groupId in selected.GroupIds)
            {
                var group = _repository.GroupRepository.Get(groupId);
                if (group != null)
                {
                    result.Add(group);
                }
            }

            return result;
        }

        public List<AGSUserEntity> GetAllUsers()
        {
            var result = new List<AGSUserEntity>();
            var userIds = _repository.UserRepository.GetAll();
            if (userIds != null)
            {
                foreach (var userId in userIds)
                {
                    var user = _repository.UserRepository.Get(userId);
                    if (user != null)
                    {
                        result.Add(user);
                    }
                }
            }

            return result;
        }

        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException();
            }


            bool result = _repository.UserRepository.ValidatePassword(userId, oldPassword);
            if (!result)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.UsernameOrPasswordError);
            }

            bool changeResult = _repository.UserRepository.ChangePassword(userId, newPassword);
            _repository.Save();
            return changeResult;
        }
    }
}
