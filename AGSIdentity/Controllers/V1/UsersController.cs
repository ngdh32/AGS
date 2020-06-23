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
    public class UsersController : ControllerBase , IBLLController<AGSUser>
    {
        private IRepository _repository { get; set; }

        public UsersController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get() {
            var result = new List<AGSUser>();
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

            return new JsonResult(result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var user = GetModel(id);
            return new JsonResult(user);
        }


        [HttpGet("{id}/menus")]
        public IActionResult GetUserMenus(string id)
        {
            var result = new List<AGSMenu>();
            var user = GetModel(id);
            var menuIds = _repository.MenuRepository.GetAll();
            MenusController menusController = new MenusController(_repository);
            if (menuIds != null)
            {
                foreach (var menuId in menuIds)
                {
                    var menu = menusController.GetModel(menuId);
                    if (menu != null)
                    {
                        foreach (var group in user.Groups)
                        {
                            if (group.FunctionClaims.Exists(x => x.Id == menu.FunctionClaim.Id))
                            {
                                if (menu != null)
                                {
                                    result.Add(menu);
                                }
                            }
                        }
                    }
                    
                }
            }
            
            return new JsonResult(result);
        }

        [HttpPost]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditPolicyConstant)]
        public IActionResult Post([FromBody] AGSUser user)
        {
            var id = SaveOrUpdateModel(user);
            _repository.Save();
            return new JsonResult(id);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditPolicyConstant)]
        public IActionResult Put([FromBody] AGSUser user, string id) {
            if (user.Id != id)
            {
                return BadRequest();
            }
            else
            {
                SaveOrUpdateModel(user);
                _repository.Save();
                return Ok();
            }

        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditPolicyConstant)]
        public IActionResult Delete(string id) {
            DeleteModel(id);
            _repository.Save();
            return Ok();
        }

        public string SaveOrUpdateModel(AGSUser model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            var entity = new AGSUserEntity()
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                GroupIds = model.Groups?.Select(x => x.Id)?.ToList() ?? new List<string>()
            };

            if (model.Id == null)
            {
                model.Id = _repository.UserRepository.Create(entity);
            }else
            {
                entity.Id = model.Id;
                _repository.UserRepository.Update(entity);
            }

            return model.Id;
        }

        public void DeleteModel(string id)
        {
            _repository.UserRepository.Delete(id);
        }

        public AGSUser GetModel(string id)
        {
            var selected = _repository.UserRepository.Get(id);
            if (selected != null)
            {
                var result = new AGSUser()
                {
                    Id = selected.Id,
                    Email = selected.Email,
                    Username = selected.Username,
                    Groups = new List<AGSGroup>()
                };
                

                GroupsController groupsController = new GroupsController(_repository);
                if (selected.GroupIds != null)
                {
                    foreach(var groupId in selected.GroupIds)
                    {
                        result.Groups.Add(groupsController.GetModel(groupId));
                    }
                }

                return result;
            }else
            {
                return null;
            }
        }
    }
}
