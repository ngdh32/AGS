using System;
using System.Collections.Generic;
using System.Security.Claims;
using AGS.Models.ViewModels.Common;

namespace AGS.Services.AGS.Menu
{
    public interface IMenuService
    {
        List<MenuOption> GetMenuOptions(List<Claim> functionClaimEntities);
    }
}
