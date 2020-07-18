using System;
using AGSCommon.Models.EntityModels.AGSIdentity;
using AGSCommon.Models.EntityModels.Common;
using AGSIdentity.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AGSIdentity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSPolicyConstant)]
    public class ConfigController : ControllerBase, IBLLController<AGSConfigEntity>
    {
        private IRepository _repository { get; set; }

        public ConfigController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var systemConfigs = _repository.ConfigRepository.GetAll();
            return AGSResponseFactory.GetAGSResponseJsonResult(systemConfigs);
        }

        [HttpGet("{key}")]
        public IActionResult Get(string key)
        {
            var result = GetModel(key);
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        [HttpPost]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSConfigEditPolicyConstant)]
        public IActionResult Post(AGSConfigEntity configEntity)
        {
            var result = SaveModel(configEntity);
            _repository.Save();
            return AGSResponseFactory.GetAGSResponseJsonResult(result);
        }

        [HttpPut("{key}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSConfigEditPolicyConstant)]
        public IActionResult Put(AGSConfigEntity configEntity, string key)
        {
            if (configEntity.Key != key)
            {
                return BadRequest();
            }

            var result = UpdateModel(configEntity);
            _repository.Save();
            return AGSResponseFactory.GetAGSResponseJsonResult();

        }

        [HttpDelete("{key}")]
        [Authorize(Policy = AGSCommon.CommonConstant.AGSIdentityConstant.AGSConfigEditPolicyConstant)]
        public IActionResult Delete(string key)
        {
            DeleteModel(key);
            _repository.Save();
            return AGSResponseFactory.GetAGSResponseJsonResult();
        }

        public string SaveModel(AGSConfigEntity model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(model.Key))
            {
                throw new ArgumentException();
            }

            if (model.Value == null)
            {
                throw new ArgumentException();
            }

            string entityKey = _repository.ConfigRepository.Create(model);
            return entityKey;
        }

        public int UpdateModel(AGSConfigEntity model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(model.Key))
            {
                throw new ArgumentException();
            }

            if (model.Value == null)
            {
                throw new ArgumentException();
            }

            int result = _repository.ConfigRepository.Update(model);
            if (result > 0)
            {
                return result;
            }
            else
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }
        }

        public void DeleteModel(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException();
            }

            _repository.ConfigRepository.Delete(key);

        }

        public AGSConfigEntity GetModel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            var configEntity = _repository.ConfigRepository.Get(id);
            return configEntity;
        }
    }
}
