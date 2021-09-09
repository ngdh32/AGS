using System;
using System.Collections.Generic;
using System.Linq;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using AGSIdentity.Models.EntityModels.AGSIdentity.EF;

namespace AGSIdentity.Repositories.EF
{
    public class EFDepartmentsRepository : IDepartmentsRepository
    {
        private EFApplicationDbContext _applicationDbContext { get; set; }

        public EFDepartmentsRepository(EFApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public string Create(AGSDepartmentEntity functionClaim)
        {
            var result = new EFDepartment();
            UpdateEFDepartment(functionClaim, result);
            result.Id = CommonConstant.GenerateId();
            _applicationDbContext.Departments.Add(result);
            return result.Id;
        }

        public void Delete(string id)
        {
            var selected = (from x in _applicationDbContext.Departments
                            where x.Id == id
                            select x).FirstOrDefault();
            if (selected != null)
            {

                _applicationDbContext.Departments.Remove(selected);
            }
        }

        public AGSDepartmentEntity Get(string id)
        {
            var selected = (from x in _applicationDbContext.Departments
                            where x.Id == id
                            select x).FirstOrDefault();
            if (selected != null)
            {
                var result = GetAGSDepartmentEntity(selected);
                return result;
            }
            else
            {
                return null;
            }
        }

        public List<string> GetAll()
        {
            var result = new List<string>();
            foreach (var fc in _applicationDbContext.Departments.ToList())
            {
                result.Add(fc.Id);
            }
            return result;
        }

        public int Update(AGSDepartmentEntity departmentEntity)
        {
            var selected = (from x in _applicationDbContext.Departments
                            where x.Id == departmentEntity.Id
                            select x).FirstOrDefault();
            if (selected != null)
            {
                UpdateEFDepartment(departmentEntity, selected);
                _applicationDbContext.Departments.Update(selected);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public AGSDepartmentEntity GetAGSDepartmentEntity(EFDepartment efDepartment)
        {
            var result = new AGSDepartmentEntity()
            {
                Id = efDepartment.Id,
                Name = efDepartment.Name,
                HeadUserId = efDepartment.HeadUserId,
                ParentDepartmentId = efDepartment.ParentDepartmentId
            };

            return result;
        }

        public void UpdateEFDepartment(AGSDepartmentEntity departmentEntity, EFDepartment efDepartment)
        {
            efDepartment.Id = departmentEntity.Id;
            efDepartment.Name = departmentEntity.Name;
            efDepartment.ParentDepartmentId = departmentEntity.ParentDepartmentId;
            efDepartment.HeadUserId = departmentEntity.HeadUserId;
        }
    }
}
