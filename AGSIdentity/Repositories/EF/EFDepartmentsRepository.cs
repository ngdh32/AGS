using System;
using System.Collections.Generic;
using System.Linq;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using AGSIdentity.Models.EntityModels.AGSIdentity.EF;
using AGSIdentity.Models.ViewModels.API.Common;
using Microsoft.AspNetCore.Identity;

namespace AGSIdentity.Repositories.EF
{
    public class EFDepartmentsRepository : IDepartmentsRepository
    {
        private EFApplicationDbContext _applicationDbContext { get; set; }

        private UserManager<EFApplicationUser> _userManager { get; set; }

        public EFDepartmentsRepository(EFApplicationDbContext applicationDbContext, UserManager<EFApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        public string Create(AGSDepartmentEntity department)
        {
            var result = new EFDepartment();
            result.Id = CommonConstant.GenerateId();
            UpdateEFDepartment(department, result);
            _applicationDbContext.Departments.Add(result);
            UpdateUserDepartment(department, result);
            return result.Id;
        }

        public void Delete(string id)
        {
            var selected = (from x in _applicationDbContext.Departments
                            where x.Id == id
                            select x).FirstOrDefault();
            if (selected != null)
            {
                RemoveAllUsersFromDepartment(selected.Id);
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
                UpdateUserDepartment(departmentEntity, selected);
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
                ParentDepartmentId = efDepartment.ParentDepartmentId,
                UserIds = _applicationDbContext.UserDepartments?.Where(y => y.DepartmentId == efDepartment.Id)?.Select(x => x.UserId)?.ToList() ?? new List<string>()
            };

            return result;
        }

        private void UpdateEFDepartment(AGSDepartmentEntity departmentEntity, EFDepartment efDepartment)
        {
            efDepartment.Id = departmentEntity.Id;
            efDepartment.Name = departmentEntity.Name;
            efDepartment.ParentDepartmentId = departmentEntity.ParentDepartmentId;
            efDepartment.HeadUserId = departmentEntity.HeadUserId;
            efDepartment.UserDepartments = new List<EFApplicationUserDepartment>();
        }

        private void UpdateUserDepartment(AGSDepartmentEntity departmentEntity, EFDepartment efDepartment)
        {
            RemoveAllUsersFromDepartment(departmentEntity.Id);

            // add back the latest link
            foreach (var userId in departmentEntity.UserIds)
            {
                var selectedUser = _userManager.FindByIdAsync(userId).Result;
                _applicationDbContext.UserDepartments.Add(new EFApplicationUserDepartment()
                {
                    Department = efDepartment,
                    User = selectedUser
                });
            }
        }

        public void RemoveAllUsersFromDepartment(string departmentId)
        {
            // remove all users linked with the department
            var selectedUserDepartments = _applicationDbContext.UserDepartments.Where(x => x.DepartmentId == departmentId);
            if (selectedUserDepartments != null)
            {
                _applicationDbContext.UserDepartments.RemoveRange(selectedUserDepartments);
            }
        }

        

        public void AddUserToDepartment(string userId, string departmentId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.UserNotFound);
            }

            var department = _applicationDbContext.Departments.FirstOrDefault(x => x.Id == departmentId);
            if (department == null)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.DepartmentNotFound);
            }

            _applicationDbContext.UserDepartments.Add(new EFApplicationUserDepartment()
            {
                Department = department,
                User = user
            });

        }

        public void RemoveUserFromDepartment(string userId, string departmentId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.UserNotFound);
            }

            var department = _applicationDbContext.Departments.FirstOrDefault(x => x.Id == departmentId);
            if (department == null)
            {
                throw new AGSException(AGSResponse.ResponseCodeEnum.DepartmentNotFound);
            }

            var userDepartments = _applicationDbContext.UserDepartments.Where(x => x.UserId == userId && x.DepartmentId == departmentId);
            if (userDepartments != null)
            {
                _applicationDbContext.UserDepartments.RemoveRange(userDepartments);
            }
        }

        public List<AGSDepartmentEntity> GetDepartmentsByUserId(string userId)
        {
            var result = new List<AGSDepartmentEntity>();
            var departmentIds = _applicationDbContext.UserDepartments?.Where(x => x.UserId == userId)?.Select(y => y.DepartmentId);
            foreach(var departmentId in departmentIds)
            {
                var department = _applicationDbContext.Departments.FirstOrDefault(x => x.Id == departmentId);
                result.Add(GetAGSDepartmentEntity(department));
            }

            return result;
        }
    }
}
