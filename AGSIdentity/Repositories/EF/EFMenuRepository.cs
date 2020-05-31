using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using AGSIdentity.Models.DataModels;
using System.Linq;

namespace AGSIdentity.Repositories.EF
{
    public class EFMenuRepository : IMenuRepository
    {
        private ApplicationDbContext _applicationDbContext { get; set; }

        public EFMenuRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Create(Menu  menu)
        {
            _applicationDbContext.Menus.Add(menu);
        }

        public void Delete(int id)
        {
            var selected = (from x in _applicationDbContext.Menus
                           where x.Id == id
                           select x).FirstOrDefault();
            if (selected != null)
            {
                
                _applicationDbContext.Menus.Remove(selected);
            }
        }

        public Menu Get(int id)
        {
            var selected = (from x in _applicationDbContext.Menus
                            where x.Id == id
                            select x).FirstOrDefault();
            return selected;
        }

        public List<Menu> GetAll()
        {
            return _applicationDbContext.Menus.ToList();
        }

        public void Update(Menu menu)
        {
            _applicationDbContext.Menus.Update(menu);
        }
    }
}
