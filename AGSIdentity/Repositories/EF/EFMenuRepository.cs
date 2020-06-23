using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using AGSCommon.Models.DataModels.AGSIdentity;
using System.Linq;
using AGSIdentity.Models.EntityModels.EF;
using AGSIdentity.Models.EntityModels;

namespace AGSIdentity.Repositories.EF
{
    public class EFMenuRepository : IMenuRepository
    {
        private ApplicationDbContext _applicationDbContext { get; set; }

        public EFMenuRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public string Create(AGSMenuEntity menu)
        {
            var result = new Menu()
            {
                Id = menu.Id,
                Name = menu.Name,
                Order = menu.Order,
                ParentId = menu.ParentId,
                FunctionClaimId = menu.FunctionClaimId
            };
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
                var result = new AGSMenuEntity()
                {
                    Id  = selected.Id,
                    Name = selected.Name,
                    Order = selected.Order,
                    ParentId = selected.ParentId,
                    FunctionClaimId = selected.FunctionClaimId
                };

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

        public void Update(AGSMenuEntity menu)
        {
            var selected = (from x in _applicationDbContext.Menus
                            where x.Id == menu.Id
                            select x).FirstOrDefault();
            if (selected != null)
            {
                selected.FunctionClaimId = menu.FunctionClaimId;
                selected.Name = menu.Name;
                selected.Order = menu.Order;
                selected.ParentId = menu.ParentId;
                _applicationDbContext.Menus.Update(selected);
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
                    var result = new AGSMenuEntity()
                    {
                        Id = parentSelected.Id,
                        Name = parentSelected.Name,
                        Order = parentSelected.Order,
                        FunctionClaimId = parentSelected.FunctionClaimId,
                        ParentId = parentSelected.ParentId
                    };

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
    }
}
