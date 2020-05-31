using System;
using System.Collections.Generic;
using AGSIdentity.Models.DataModels;

namespace AGSIdentity.Repositories
{
    public interface IMenuRepository
    {
        Menu  Get(int id);
        List<Menu> GetAll();
        void Delete(int id);
        void Create(Menu  menu);
        void Update(Menu  menu);
    }
}
