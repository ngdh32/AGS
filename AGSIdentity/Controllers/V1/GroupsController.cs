using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AGSCommon.Models.EntityModels.AGSIdentity;
using AGSCommon.Models.EntityModels.Common;
using AGSIdentity.Models.EntityModels;
using AGSIdentity.Repositories;
using AGSIdentity.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AGSIdentity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSPolicyConstant)]
    public class GroupsController : ControllerBase , IBLLController<AGSGroupEntity>
    {
        private IRepository _repository { get; set; }
        private IConfiguration _configuration { get; set; }
        private IAuthService _authService { get; set; }

        public GroupsController(IRepository repository, IConfiguration configuration, IAuthService authService)
        {
            _repository = repository;
            _configuration = configuration;
            _authService = authService;
        }

        /// <summary>
        /// Get all groups
        /// </summary>
        [HttpGet]
        public IActionResult Get() {
            var result = new List<AGSGroupEntity>();
            var groupIds = _repository.GroupRepository.GetAll();
            if (groupIds != null)
            {
                foreach (var groupId in groupIds)
                {
                    var group = GetModel(groupId);
                    if (group != null)
                    {
                        result.Add(group);
                    }
                }
            }

            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        [HttpGet("{id}/functionclaims")]
        public IActionResult GetFunctionClaims(string id)
        {
            var result = GetGroupFunctionClaims(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        [HttpGet("{id}")]
        /// <summary>
        /// Get a specified groups
        /// </summary>
        public IActionResult Get(string id) {
            var group = GetModel(id);
            return AGSResponseFactory.GetAGSResponseJsonResult(group);
        }

        


        /// <summary>
        /// Get all the users in a specified group
        /// </summary>
        [HttpGet("{id}/users")]
        public IActionResult GetAllUsersinGroup(string id) {
            List<AGSUserEntity> result = new List<AGSUserEntity>();
            var userIds = _repository.UserRepository.GetAll();
            UsersController usersController = new UsersController(_repository, _configuration, _authService);
            foreach(var userId in userIds)
            {
                var user = _repository.UserRepository.Get(userId);
                if (user != null)
                {
                    if (user.GroupIds != null)
                    {
                        {
                            result.Add(usersController.GetModel(userId));
                        }
                    }
                }
                
            }
            
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }


        /// <summary>
        /// Create a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="group"></param>
        [HttpPost]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSGroupEditPolicyConstant))]
        public IActionResult Post([FromBody] AGSGroupEntity group)
        {
            var id = SaveModel(group);
            _repository.Save();
            return AGSResponseFactory.GetAGSResponseJsonResult(id);
        }

        

        /// <summary>
        /// Update a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="group"></param>
        /// <param name="id"></param>
        [HttpPut("{id}")]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSGroupEditPolicyConstant))]
        public IActionResult Put([FromBody] AGSGroupEntity group, string id)
        {
            if (group.Id == id)
            {
                var result = UpdateModel(group);
                _repository.Save();
                return AGSResponseFactory.GetAGSResponseJsonResult();

            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Delete a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [Authorize(Policy = (AGSCommon.CommonConstant.AGSIdentityConstant.AGSGroupEditPolicyConstant))]
        public IActionResult Delete(string id)
        {
            DeleteModel(id);
            _repository.Save();
            return AGSResponseFactory.GetAGSResponseJsonResult();
        }




        #region BLL

        public string SaveModel(AGSGroupEntity group)
        {
            if (group == null)
            {
                throw new ArgumentNullException();
            }

            if(string.IsNullOrEmpty(group.Id))
            {
                group.Id = AGSCommon.CommonFunctions.GenerateId();
            }

            
            string entityId = _repository.GroupRepository.Create(group);
            return entityId;
        }

        public int UpdateModel(AGSGroupEntity group)
        {
            if (group == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(group.Id))
            {
                throw new ArgumentException();
            }

            
            int result = _repository.GroupRepository.Update(group);
            if (result > 0)
            {
                return result;
            }
            else
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }
        }

        public void DeleteModel(string groupId)
        {
            if (string.IsNullOrEmpty(groupId))
            {
                throw new ArgumentNullException();
            }

            _repository.GroupRepository.Delete(groupId);
        }

        public AGSGroupEntity GetModel(string groupId)
        {
            if (string.IsNullOrEmpty(groupId))
            {
                throw new ArgumentNullException();
            }

            var entity = _repository.GroupRepository.Get(groupId);
            return entity;
        }

        public List<AGSFunctionClaimEntity> GetGroupFunctionClaims(string groupId)
        {
            var result = new List<AGSFunctionClaimEntity>();

            var selectedGroup = _repository.GroupRepository.Get(groupId);

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

        #endregion
    }
}
