using System;
using System.Collections.Generic;
using AGS.Models.ViewModels.Common;

namespace AGS.Repositories.Menu
{
    public interface IMenuRepository
    {
        List<MenuOption> GetMenuOptions();
    }
}
