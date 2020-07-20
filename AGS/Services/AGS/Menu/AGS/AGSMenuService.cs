using System;
using System.Collections.Generic;
using System.Security.Claims;
using AGS.Models.ViewModels.Common;
using AGS.Repositories.Menu;
using AGS.Services.AGS.Localization;
using AGSCommon.Models.EntityModels.AGSIdentity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace AGS.Services.AGS.Menu.AGS
{
    public class AGSMenuService : IMenuService
    {
        private ILocalizationService _localizationService { get; set; }
        private IMenuRepository _menuRepository { get; set; }

        public AGSMenuService(IMenuRepository menuRepository, ILocalizationService localizationService)
        {
            _menuRepository = menuRepository;
            _localizationService = localizationService;
        }

        public List<MenuOption> GetMenuOptions(List<Claim> functionClaimEntities)
        {
            List<MenuOption> result = new List<MenuOption>();
            var menuOptions = _menuRepository.GetMenuOptions();
            foreach (var menuOption in menuOptions)
            {
                var matchedMenuOption = GetMenuOptions(menuOption, functionClaimEntities);
                if (matchedMenuOption != null)
                {
                    result.Add(matchedMenuOption);
                }
            }

            return result;
        }

        public MenuOption GetMenuOptions(MenuOption menuOption, List<Claim> functionClaimEntities)
        {
            if (functionClaimEntities.Exists(x => x.Value == menuOption.FunctionClaim
                && x.Type == AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant) ||
                string.IsNullOrEmpty(menuOption.FunctionClaim))
            {
                if (menuOption.ChildrenMenus == null)
                {
                    return menuOption;
                }
                else
                {
                    var matchedChildMenuOptions = new List<MenuOption>();
                    foreach (var childMenuOption in menuOption.ChildrenMenus)
                    {
                        var matchedChildMenu = GetMenuOptions(childMenuOption, functionClaimEntities);
                        if (matchedChildMenu != null)
                        {
                            matchedChildMenuOptions.Add(matchedChildMenu);
                        }
                    }

                    menuOption.ChildrenMenus = matchedChildMenuOptions;
                    return menuOption;
                }
                
            }
            else
            {
                return null;
            }
        }

        
    }
}
