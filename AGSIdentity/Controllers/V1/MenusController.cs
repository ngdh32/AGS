using System;
using System.Collections.Generic;
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
    public class MenusController : ControllerBase , IBLLController<AGSMenuEntity>
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
            var result = new List<AGSMenuEntity>();
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

            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done, result));
        }


        /// <summary>
        /// Return a specified menu
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var result = GetModel(id);
            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done, result));
        }


        /// <summary>
        /// Create a menu. Only users with specified claim are allowed
        /// </summary>
        /// <param name="menu"></param>
        [HttpPost]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSMenuEditPolicyConstant)]
        public IActionResult Post([FromBody] AGSMenuEntity menu)
        {
            var id = SaveModel(menu);
            _repository.Save();
            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done, id));
        }

        /// <summary>
        /// Update a menu. Only users with specified claim are allowed
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="id"></param>
        [HttpPut("{id}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSMenuEditPolicyConstant)]
        public IActionResult Put([FromBody] AGSMenuEntity menu, string id)
        {
            if (menu.Id == id)
            {
                var result = UpdateModel(menu);
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
        /// Delete a menu. Only users with specified claim are allowed
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSMenuEditPolicyConstant)]
        public IActionResult Delete(string id)
        {
            DeleteModel(id);
            _repository.Save();
            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done));
        }

        public int UpdateModel(AGSMenuEntity model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                throw new ArgumentException();
            }

            int result = _repository.MenuRepository.Update(model);
            return result;
        }

        public string SaveModel(AGSMenuEntity model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (!string.IsNullOrEmpty(model.Id))
            {
                throw new ArgumentException();
            }

            string entityId = _repository.MenuRepository.Create(model);

            return entityId;
        }

        public void DeleteModel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            _repository.MenuRepository.Delete(id);
        }

        public AGSMenuEntity GetModel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            var entity  = _repository.MenuRepository.Get(id);
            return entity;
            
        }


        #region BLL


        #endregion
    }
}
