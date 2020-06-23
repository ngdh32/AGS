using System;
using System.Collections.Generic;
using AGSIdentity.Models.EntityModels;

namespace AGSIdentity.Repositories
{
    public interface IMenuRepository
    {
        AGSMenuEntity Get(string id);
        List<string> GetAll();
        void Delete(string id);
        // return newly inserted id
        string Create(AGSMenuEntity menu);
        void Update(AGSMenuEntity menu);

        //AGSMenuEntity GetParentMenu(string childMenuId);
        //string GetFunctionClaimIdByMenuId(string menuId);
    }
}
