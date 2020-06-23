using System;
using System.Collections.Generic;
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
    public class MenusController : ControllerBase , IBLLController<AGSMenu>
    {
        private IRepository _repository { get; set; }

        public MenusController(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Return all menus
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            var result = new List<AGSMenu>();
            var menuIds = _repository.MenuRepository.GetAll();
            if (menuIds != null)
            {
                foreach(var menuId in menuIds)
                {
                    var menu = GetModel(menuId);
                    if (menu != null)
                    {
                        result.Add(menu);
                    }
                }
            }

            return new JsonResult(result);
        }


        /// <summary>
        /// Return a specified menu
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var result = GetModel(id);
            return new JsonResult(result);
        }


        /// <summary>
        /// Create a menu. Only users with specified claim are allowed
        /// </summary>
        /// <param name="menu"></param>
        [HttpPost]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSMenuEditPolicyConstant)]
        public IActionResult Post([FromBody] AGSMenu menu)
        {
            var id = SaveOrUpdateModel(menu);
            _repository.Save();
            return Ok(id);
        }

        /// <summary>
        /// Update a menu. Only users with specified claim are allowed
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="id"></param>
        [HttpPut("{id}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSMenuEditPolicyConstant)]
        public IActionResult Put([FromBody] AGSMenu menu, string id)
        {
            if (menu.Id == id)
            {
                SaveOrUpdateModel(menu);
                _repository.Save();
                return Ok();
            }else
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Delete a menu. Only users with specified claim are allowed
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSMenuEditPolicyConstant)]
        public IActionResult Delete(string id)
        {
            DeleteModel(id);
            return Ok();
        }

        public string SaveOrUpdateModel(AGSMenu model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }
            
            var entity = new AGSMenuEntity()
            {
                Name = model.Name,
                Order = model.Order,
                FunctionClaimId = model.FunctionClaim?.Id,
                ParentId = model.ParentMenu?.Id
            };

            if (model.Id == null)
            {
                model.Id = _repository.MenuRepository.Create(entity);
            }else
            {
                entity.Id = model.Id;
                _repository.MenuRepository.Update(entity);
            }

            return model.Id;
        }

        public void DeleteModel(string id)
        {
            _repository.MenuRepository.Delete(id);
        }

        public AGSMenu GetModel(string id)
        {
            var entity  = _repository.MenuRepository.Get(id);
            if (entity != null)
            {
                var result = new AGSMenu()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Order = entity.Order,
                    ParentMenu = null,
                    FunctionClaim = null
                };

                if (entity.ParentId != null)
                {
                    result.ParentMenu = GetModel(entity.ParentId);
                }

                if (entity.FunctionClaimId != null)
                {
                    FunctionClaimsController functionClaimsController = new FunctionClaimsController(_repository);
                    result.FunctionClaim = functionClaimsController.GetModel(entity.FunctionClaimId);
                }

                return result;
            }
            else
            {
                return null;
            }
            
        }


        #region BLL


        #endregion
    }
}
