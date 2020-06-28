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

namespace AGSIdentity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSPolicyConstant)]
    public class UsersController : ControllerBase
    {
        private IRepository _repository { get; set; }

        public UsersController(IRepository repository)
        {
            _repository = repository;
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

            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done, result));
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var user = GetModel(id);
            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done, user));
        }



        [HttpPost]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditPolicyConstant)]
        public IActionResult Post([FromBody] AGSUserWithPasswordModel user)
        {
            var id = SaveModel(user);
            _repository.Save();
            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done, id));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditPolicyConstant)]
        public IActionResult Put([FromBody] AGSUserWithPasswordModel user, string id) {
            if (user.Id != id)
            {
                return BadRequest();
            }
            else
            {
                int result = UpdateModel(user);
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

        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditPolicyConstant)]
        public IActionResult Delete(string id) {
            DeleteModel(id);
            _repository.Save();
            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done));
        }

        public int UpdateModel(AGSUserWithPasswordModel model)
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
            return result;
        }

        public string SaveModel(AGSUserWithPasswordModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (!string.IsNullOrEmpty(model.Id))
            {
                throw new ArgumentException();
            }

            string entityId = _repository.UserRepository.Create(model);

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
    }
}
