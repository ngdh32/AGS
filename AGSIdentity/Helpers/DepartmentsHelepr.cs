using System;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using AGSIdentity.Models.ViewModels.API.Common;
using AGSIdentity.Repositories;

namespace AGSIdentity.Helpers
{
    public class DepartmentsHelepr
    {
        private IRepository _repository { get; set; }

        public DepartmentsHelepr(IRepository repository)
        {
            _repository = repository;
        }

        public AGSDepartmentEntity GetDepartmentById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            var result = _repository.DepartmentsRepository.Get(id);
            return result;
        }

        public List<AGSDepartmentEntity> GetAllDepartments()
        {
            var result = new List<AGSDepartmentEntity>();

            var DepartmentIds = _repository.DepartmentsRepository.GetAll();
            if (DepartmentIds != null)
            {
                foreach (var DepartmentId in DepartmentIds)
                {
                    var entity = _repository.DepartmentsRepository.Get(DepartmentId);
                    if (entity != null)
                    {
                        result.Add(entity);
                    }
                }
            }

            return result;
        }

        public string CreateDepartment(AGSDepartmentEntity model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (!string.IsNullOrEmpty(model.Id))
            {
                throw new ArgumentException();
            }

            var result = _repository.DepartmentsRepository.Create(model);
            _repository.Save();
            return result;
        }

        public int UpdateDepartment(AGSDepartmentEntity model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                throw new ArgumentException();
            }


            var result = _repository.DepartmentsRepository.Update(model);
            if (result == 0)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.ModelNotFound);
            }

            _repository.Save();
            return result;
        }

        public void DeleteDepartment(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            _repository.DepartmentsRepository.Delete(id);
            _repository.Save();
        }
    }
}
