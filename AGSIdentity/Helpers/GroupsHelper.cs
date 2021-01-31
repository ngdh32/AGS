using System;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using AGSIdentity.Models.ViewModels.API.Common;
using AGSIdentity.Repositories;

namespace AGSIdentity.Helpers
{
    public class GroupsHelper
    {
        private IRepository _repository { get; set; }

        public GroupsHelper(IRepository repository)
        {
            _repository = repository;
        }

        public string CreateGroup(AGSGroupEntity model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (!string.IsNullOrEmpty(model.Id))
            {
                throw new ArgumentException();
            }

            var result = _repository.GroupsRepository.Create(model);
            _repository.Save();
            return result;
        }

        public int UpdateGroup(AGSGroupEntity model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                throw new ArgumentException();
            }


            var result = _repository.GroupsRepository.Update(model);
            if (result == 0)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }

            _repository.Save();
            return result;
        }

        public void DeleteGroup(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            _repository.GroupsRepository.Delete(id);
            _repository.Save();
        }

        public AGSGroupEntity GetGroupById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            var entity = _repository.GroupsRepository.Get(id);
            return entity;
        }

        public List<AGSGroupEntity> GetAllGroups()
        {
            var result = new List<AGSGroupEntity>();
            var groupIds = _repository.GroupsRepository.GetAll();
            if (groupIds != null)
            {
                foreach (var groupId in groupIds)
                {
                    var group = _repository.GroupsRepository.Get(groupId);
                    if (group != null)
                    {
                        result.Add(group);
                    }
                }
            }

            return result;
        }

        public List<AGSFunctionClaimEntity> GetFunctionClaimsByGroupId(string id)
        {
            var result = new List<AGSFunctionClaimEntity>();
            var selectedGroup = _repository.GroupsRepository.Get(id);

            if (selectedGroup != null)
            {
                if (selectedGroup.FunctionClaimIds != null)
                {
                    foreach (var functionClaimId in selectedGroup.FunctionClaimIds)
                    {
                        var functionClaim = _repository.FunctionClaimsRepository.Get(functionClaimId);
                        result.Add(functionClaim);
                    }
                }
            }

            return result;
        }

        public List<AGSUserEntity> GetUsersByGroupId(string id)
        {
            var result = new List<AGSUserEntity>();
            var userIds = _repository.UsersRepository.GetAll();
            if (userIds != null)
            {
                foreach(var userId in userIds)
                {
                    var user = _repository.UsersRepository.Get(userId);
                    if (user != null && user.GroupIds != null)
                    {
                        if (user.GroupIds.Contains(id))
                        {
                            result.Add(user);
                        }
                    }

                }
            }

            return result;
        }
    }
}
