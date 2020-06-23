using System;
using System.Collections.Generic;
using System.Linq;
using AGSCommon.Models.DataModels.AGSIdentity;
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
    public class GroupsController : ControllerBase , IBLLController<AGSGroup>
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
            var result = new List<AGSGroup>();
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

            return new JsonResult(groupIds);
        }

        [HttpGet("{id}")]
        /// <summary>
        /// Get a specified groups
        /// </summary>
        public IActionResult Get(string id) {
            var group = GetModel(id);
            return new JsonResult(group);
        }

        


        /// <summary>
        /// Get all the users in a specified group
        /// </summary>
        [HttpGet("{id}/users")]
        public IActionResult GetAllUsersinGroup(string id) {
            List<AGSUser> result = new List<AGSUser>();
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
            return new JsonResult(result);

        }


        /// <summary>
        /// Create a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="group"></param>
        [HttpPost]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSGroupEditPolicyConstant)]
        public IActionResult Post([FromBody] AGSGroup group)
        {
            var id = SaveOrUpdateModel(group);
            _repository.Save();
            return Ok(id);
        }

        

        /// <summary>
        /// Update a group. Only users with specified claim are allowed
        /// </summary>
        /// <param name="group"></param>
        /// <param name="id"></param>
        [HttpPut("{id}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSGroupEditPolicyConstant)]
        public IActionResult Put([FromBody] AGSGroup group, string id)
        {
            if (group.Id == id)
            {
                SaveOrUpdateModel(group);
                _repository.Save();
                return Ok();
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

        public string SaveOrUpdateModel(AGSGroup group)
        {
            if (group == null)
            {
                throw new ArgumentNullException();
            }

            var entity = new AGSGroupEntity()
            {
                Name = group.Name,
                FunctionClaimIds = new List<string>() 
            };
            if (group.FunctionClaims != null)
            {
                entity.FunctionClaimIds = group.FunctionClaims.Select(x => x.Id).ToList();
            }


            if (group.Id == null)
            {
                // add new AGS group
                group.Id = _repository.GroupRepository.Create(entity);
            }
            else
            {
                // update existing AGS group
                entity.Id = group.Id;
                _repository.GroupRepository.Update(entity);
            }

            return group.Id;
        }

        public void DeleteModel(string groupId)
        {
            _repository.GroupRepository.Delete(groupId);
        }

        public AGSGroup GetModel(string groupId)
        {
            var entity = _repository.GroupRepository.Get(groupId);
            if (entity != null)
            {
                var result = new AGSGroup()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    FunctionClaims = new List<AGSFunctionClaim>()
                };
                
                FunctionClaimsController functionClaimsController = new FunctionClaimsController(_repository);
                if (entity.FunctionClaimIds != null)
                {
                    foreach (var functionClaimId in entity.FunctionClaimIds)
                    {
                        var functionClaim = functionClaimsController.GetModel(functionClaimId);
                        if (functionClaim != null)
                        {
                            result.FunctionClaims.Add(functionClaim);
                        }

                    }
                }

                return result;
            }
            else
            {
                return null;
            }

        }

        #endregion
    }
}
