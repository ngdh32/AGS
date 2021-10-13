using System;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using AGSIdentity.Models.ViewModels.API.Common;
using AGSIdentity.Repositories;
using Microsoft.Extensions.Configuration;

namespace AGSIdentity.Helpers
{
    public class UsersHelper
    {
        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;

        public UsersHelper(IRepository repository, IConfiguration configuration)
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

            var selected = _repository.UsersRepository.Get(model.Id);
            if (selected == null)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }

            var selectedUserByName = _repository.UsersRepository.GetByUsername(model.Username);
            // Not allow to change the username to admin
            if (selected.Username != CommonConstant.AGSAdminName && model.Username == CommonConstant.AGSAdminName)
            {
                throw new ArgumentException();
            }

            // Not allow to change admin username
            if (selected.Username == CommonConstant.AGSAdminName && model.Username != CommonConstant.AGSAdminName)
            {
                throw new ArgumentException();
            }

            // not allow duplicate username
            if (selectedUserByName != null && selectedUserByName.Id != model.Id)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.UsernameDuplicate);
            }

            

            

            var result = _repository.UsersRepository.Update(model);
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

            if (string.IsNullOrEmpty(model.Username))
            {
                throw new ArgumentException();
            }

            // Not allow to create admin
            if (model.Username == CommonConstant.AGSAdminName)
            {
                throw new ArgumentException();
            }

            var selectedUserByName = _repository.UsersRepository.GetByUsername(model.Username);
            if (selectedUserByName != null)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.UsernameDuplicate);
            }

            string result = _repository.UsersRepository.Create(model);
            // set the default password
            _repository.UsersRepository.ResetPassword(result, _configuration["default_user_password"]);
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
            var selected = _repository.UsersRepository.Get(id);
            if (selected != null)
            {
                if (selected.Username == CommonConstant.AGSAdminName)
                {
                    throw new ArgumentException();
                }
            }

            _repository.UsersRepository.Delete(id);
            _repository.Save();
        }

        public AGSUserEntity GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            var selected = _repository.UsersRepository.Get(id);
            return selected;
        }

        public bool ResetPassword(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException();
            }

            var selected = _repository.UsersRepository.Get(userId);
            if (selected == null)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }

            bool result = _repository.UsersRepository.ResetPassword(userId, _configuration["default_user_password"]);
            _repository.Save();
            return result;
        }

        public List<AGSGroupEntity> GetGroupsByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException();
            }

            var result = new List<AGSGroupEntity>();
            var selected = _repository.UsersRepository.Get(userId);

            if(selected == null)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }

            foreach (var groupId in selected.GroupIds)
            {
                var group = _repository.GroupsRepository.Get(groupId);
                if (group != null)
                {
                    result.Add(group);
                }
            }

            return result;
        }

        public List<AGSDepartmentEntity> GetDepartmentsByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException();
            }

            var selected = _repository.UsersRepository.Get(userId);
            if (selected == null)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }

            var result = _repository.DepartmentsRepository.GetDepartmentsByUserId(userId);

            return result;
        }

        public List<AGSUserEntity> GetAllUsers()
        {
            var result = new List<AGSUserEntity>();
            var userIds = _repository.UsersRepository.GetAll();
            if (userIds != null)
            {
                foreach (var userId in userIds)
                {
                    var user = _repository.UsersRepository.Get(userId);
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


            bool result = _repository.UsersRepository.ValidatePassword(userId, oldPassword);
            if (!result)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.UsernameOrPasswordError);
            }

            bool changeResult = _repository.UsersRepository.ChangePassword(userId, newPassword);
            _repository.Save();
            return changeResult;
        }
    }
}
