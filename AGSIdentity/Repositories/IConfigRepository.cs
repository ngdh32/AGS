using System;
using System.Collections.Generic;
using AGSCommon.Models.EntityModels.AGSIdentity;

namespace AGSIdentity.Repositories
{
    public interface IConfigRepository
    {
        AGSConfigEntity Get(string id);
        List<AGSConfigEntity> GetAll();
        void Delete(string id);
        // return newly inserted id
        string Create(AGSConfigEntity configEntity);
        // return how many records are updated.
        int Update(AGSConfigEntity configEntity);
    }
}
