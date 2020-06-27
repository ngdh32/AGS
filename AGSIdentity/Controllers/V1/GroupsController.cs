using System;
using System.Collections.Generic;
using System.Linq;
using AGSCommon.Models.EntityModels.AGSIdentity;
using AGSCommon.Models.EntityModels.Common;
using AGSIdentity.Models.EntityModels;
using AGSIdentity.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AGSIdentity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSPolicyConstant)]
    public class GroupsController : ControllerBase , IBLLController<AGSGroupEntity>
    {
        private IRepository _repository { get; set; }

        public GroupsController(IRepository repository)
        {
            _repository = repository;
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

            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done, result));
        }

        [HttpGet("{id}")]
        /// <summary>
        /// Get a specified groups
        /// </summary>
        public IActionResult Get(string id) {
            var group = GetModel(id);
            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done, group));
        }

        


        /// <summary>
        /// Get all the users in a specified group
        /// </summary>
        [HttpGet("{id}/users")]
        public IActionResult GetAllUsersinGroup(string id) {
            List<AGSUserEntity> result = new List<AGSUserEntity>();
            var userIds = _repository.UserRepository.GetAll();
            UsersController usersController = new UsersController(_repository);
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
            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done, result));

        }


        /// <summary>
        /// Create a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="group"></param>
        [HttpPost]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSGroupEditPolicyConstant)]
        public IActionResult Post([FromBody] AGSGroupEntity group)
        {
            var id = SaveModel(group);
            _repository.Save();
            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done, id));
        }

        

        /// <summary>
        /// Update a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="group"></param>
        /// <param name="id"></param>
        [HttpPut("{id}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSGroupEditPolicyConstant)]
        public IActionResult Put([FromBody] AGSGroupEntity group, string id)
        {
            if (group.Id == id)
            {
                var result = UpdateModel(group);
                if (result > 0)
                {
                    _repository.Save();
                    return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done));
                }
                else
                {
                    return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.ModelNotFound));
                }
                
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
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSGroupEditPolicyConstant)]
        public IActionResult Delete(string id)
        {
            DeleteModel(id);
            _repository.Save();
            return Ok();
        }

        #region BLL

        public string SaveModel(AGSGroupEntity group)
        {
            if (group == null)
            {
                throw new ArgumentNullException();
            }

            if (!string.IsNullOrEmpty(group.Id))
            {
                throw new ArgumentException();
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
            return result;
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

        #endregion
    }
}
