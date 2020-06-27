using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using AGSCommon.Models.EntityModels.AGSIdentity;
using System.Linq;
using AGSIdentity.Models.EntityModels.EF;
using AGSIdentity.Models.EntityModels;

namespace AGSIdentity.Repositories.EF
{
    public class EFMenuRepository : IMenuRepository
    {
        private EFApplicationDbContext _applicationDbContext { get; set; }

        public EFMenuRepository(EFApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public string Create(AGSMenuEntity menu)
        {
            var result = this.GetMenu(menu);
            _applicationDbContext.Menus.Add(result);
            return result.Id;
        }

        public void Delete(string id)
        {
            var selected = (from x in _applicationDbContext.Menus
                           where x.Id == id
                           select x).FirstOrDefault();
            if (selected != null)
            {
                _applicationDbContext.Menus.Remove(selected);
            }
        }

        public AGSMenuEntity Get(string id)
        {
            var selected = (from x in _applicationDbContext.Menus
                            where x.Id == id
                            select x).FirstOrDefault();
            if (selected != null)
            {
                var result = this.GetAGSMenuEntity(selected);
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
            foreach(var menu in _applicationDbContext.Menus.ToList())
            {
                result.Add(menu.Id);
            }
            return result;
        }

        public int Update(AGSMenuEntity menu)
        {
            var selected = (from x in _applicationDbContext.Menus
                            where x.Id == menu.Id
                            select x).FirstOrDefault();
            if (selected != null)
            {
                selected = this.GetMenu(menu);
                _applicationDbContext.Menus.Update(selected);

                return 1;
            }else
            {
                return 0;
            }
        }

        public AGSMenuEntity GetParentMenu(string childMenuId)
        {
            var selected = (from x in _applicationDbContext.Menus
                            where x.Id == childMenuId
                            select x).FirstOrDefault();
            if (selected != null)
            {
                var parentSelected = (from x in _applicationDbContext.Menus
                                      where x.Id == selected.ParentId
                                      select x).FirstOrDefault();
                if (parentSelected != null)
                {
                    var result = GetAGSMenuEntity(parentSelected);
                    return result;
                }else
                {
                    return null;
                }
            }else
            {
                return null;
            }
        }

        public string GetFunctionClaimIdByMenuId(string menuId)
        {
            var result = new List<string>();
            var selected = (from x in _applicationDbContext.Menus
                            where x.Id == menuId
                            select x).FirstOrDefault();
            if (selected != null)
            {
                return selected.FunctionClaimId;
            }
            else
            {
                return null;
            }
        }

        public List<AGSMenuEntity> GetAllByParentId(string parentId)
        {
            var selected = (from x in _applicationDbContext.Menus
                            where x.ParentId == parentId
                            select x).ToList();
            if (selected != null)
            {
                var result = new List<AGSMenuEntity>();
                foreach(var menu in selected)
                {
                    var menuEntity = this.GetAGSMenuEntity(menu);
                    result.Add(menuEntity);
                }

                return result;
            }
            else
            {
                return null;
            }
        }

        public AGSMenuEntity GetAGSMenuEntity(EFMenu menu)
        {
            var result = new AGSMenuEntity()
            {
                Id = menu.Id,
                Name = menu.Name,
                DisplayName = menu.DisplayName,
                Order = menu.Order,
                FunctionClaimId = menu.FunctionClaimId,
                ParentId = menu.ParentId
            };

            return result;
        }

        public EFMenu GetMenu(AGSMenuEntity menuEntity)
        {
            var result = new EFMenu()
            {
                Id = menuEntity.Id,
                Name = menuEntity.Name,
                DisplayName = menuEntity.DisplayName,
                Order = menuEntity.Order,
                FunctionClaimId = menuEntity.FunctionClaimId,
                ParentId = menuEntity.ParentId
            };

            return result;
        }
    }
}
