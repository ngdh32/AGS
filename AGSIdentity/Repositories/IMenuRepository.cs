using System;
using System.Collections.Generic;
using AGSCommon.Models.EntityModels.AGSIdentity;

namespace AGSIdentity.Repositories
{
    public interface IMenuRepository
    {
        AGSMenuEntity Get(string id);
        List<string> GetAll();
        void Delete(string id);
        // return newly inserted id
        string Create(AGSMenuEntity menu);
        // return how many records are updated.
        int Update(AGSMenuEntity menu);
        List<AGSMenuEntity> GetAllByParentId(string parentId);

        //AGSMenuEntity GetParentMenu(string childMenuId);
        //string GetFunctionClaimIdByMenuId(string menuId);
    }
}
