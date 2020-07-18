using System;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace AGS.Models.ViewModels.Common
{
    public class MenuOption
    {
        public string Id { get; set; }
        public string LabelKey { get; set; }
        public string FunctionClaim { get; set; }
        public string Url { get; set; }
        public string IconUrl { get; set; }
        public List<MenuOption> ChildrenMenus { get; set; }

        public MenuOption()
        {
        }
    }
}
