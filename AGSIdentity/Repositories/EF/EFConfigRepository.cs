using System;
using System.Collections.Generic;
using System.Linq;
using AGSCommon.Models.EntityModels.AGSIdentity;
using AGSIdentity.Models.EntityModels.EF;

namespace AGSIdentity.Repositories.EF
{
    public class EFConfigRepository : IConfigRepository
    {
        private EFApplicationDbContext _applicationDbContext { get; set; }

        public EFConfigRepository(EFApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public string Create(AGSConfigEntity configEntity)
        {
            var entity = new EFConfigValue();
            UpdateEFConfigValue(configEntity, entity);
            _applicationDbContext.ConfigValues.Add(entity);
            return entity.Key;
        }

        public void Delete(string key)
        {
            var selected = (from x in _applicationDbContext.ConfigValues
                            where x.Key == key
                            select x).FirstOrDefault();
            if (selected != null)
            {

                _applicationDbContext.ConfigValues.Remove(selected);
            }
        }

        public AGSConfigEntity Get(string key)
        {
            var selected = (from x in _applicationDbContext.ConfigValues
                            where x.Key == key
                            select x).FirstOrDefault();
            if (selected != null)
            {
                var result = GetAGSConfigEntity(selected);
                return result;
            }else
            {
                return null;
            }
        }

        public List<AGSConfigEntity> GetAll()
        {
            var result = new List<AGSConfigEntity>();
            foreach(var configEntity in _applicationDbContext.ConfigValues)
            {
                result.Add(GetAGSConfigEntity(configEntity));
            }
            return result;
        }

        public int Update(AGSConfigEntity configEntity)
        {
            var selected = (from x in _applicationDbContext.ConfigValues
                            where x.Key == configEntity.Key
                            select x).FirstOrDefault();
            if (selected != null)
            {
                UpdateEFConfigValue(configEntity, selected);
                _applicationDbContext.ConfigValues.Update(selected);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void UpdateEFConfigValue(AGSConfigEntity configEntity, EFConfigValue efConfigValue)
        {
            efConfigValue.Key = configEntity.Key;
            efConfigValue.Value = configEntity.Value;
            efConfigValue.IsSecure = configEntity.isSecure;
        }

        public AGSConfigEntity GetAGSConfigEntity(EFConfigValue configValue)
        {
            var result = new AGSConfigEntity()
            {
                Key = configValue.Key,
                Value = configValue.Value,
                isSecure = configValue.IsSecure
            };

            return result;
        }
    }
}
