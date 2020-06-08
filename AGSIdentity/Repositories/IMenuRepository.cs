using System;
using System.Collections.Generic;
using AGSCommon.Models.DataModels.AGSIdentity;

namespace AGSIdentity.Repositories
{
    public interface IMenuRepository
    {
        AGSMenu  Get(int id);
        List<AGSMenu> GetAll();
        void Delete(int id);
        void Create(AGSMenu  menu);
        void Update(AGSMenu  menu);
    }
}
