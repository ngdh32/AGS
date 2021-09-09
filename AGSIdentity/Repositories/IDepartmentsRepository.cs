using System;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels.AGSIdentity;

namespace AGSIdentity.Repositories
{
    public interface IDepartmentsRepository
    {
        AGSDepartmentEntity Get(string id);
        List<string> GetAll();
        void Delete(string id);
        // return newly inserted id
        string Create(AGSDepartmentEntity departmentEntity);
        // return how many records are updated.
        int Update(AGSDepartmentEntity departmentEntity);
    }
}
