using System;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using AGSIdentity.Models.ViewModels.API.Common;
using AGSIdentity.Repositories;

namespace AGSIdentity.Helpers
{
    public class GroupHelper
    {
        private IRepository _repository { get; set; }

        public GroupHelper(IRepository repository)
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

            var result = _repository.GroupRepository.Create(model);
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


            var result = _repository.GroupRepository.Update(model);
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

            _repository.GroupRepository.Delete(id);
            _repository.Save();
        }

        public AGSGroupEntity GetGroupById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            var entity = _repository.GroupRepository.Get(id);
            return entity;
        }

        public List<AGSGroupEntity> GetAllGroups()
        {
            var result = new List<AGSGroupEntity>();
            var groupIds = _repository.GroupRepository.GetAll();
            if (groupIds != null)
            {
                foreach (var groupId in groupIds)
                {
                    var group = _repository.GroupRepository.Get(groupId);
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
            var selectedGroup = _repository.GroupRepository.Get(id);

            if (selectedGroup != null)
            {
                if (selectedGroup.FunctionClaimIds != null)
                {
                    foreach (var functionClaimId in selectedGroup.FunctionClaimIds)
                    {
                        var functionClaim = _repository.FunctionClaimRepository.Get(functionClaimId);
                        result.Add(functionClaim);
                    }
                }
            }

            return result;
        }

        public List<AGSUserEntity> GetUsersByGroupId(string id)
        {
            var result = new List<AGSUserEntity>();
            var userIds = _repository.UserRepository.GetAll();
            if (userIds != null)
            {
                foreach(var userId in userIds)
                {
                    var user = _repository.UserRepository.Get(userId);
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
