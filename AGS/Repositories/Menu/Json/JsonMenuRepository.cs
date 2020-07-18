using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AGS.Models.ViewModels.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace AGS.Repositories.Menu.Json
{
    public class JsonMenuRepository : IMenuRepository
    {
        private IConfiguration _configuration { get; set; }

        //private List<MenuOption> menuOptions { get; set; }
        private string menuOptionsString { get; set; }

        public JsonMenuRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<MenuOption> GetMenuOptions()
        {
            if (string.IsNullOrEmpty(menuOptionsString))
            {
                string menuJsonFilePath = _configuration["menu_options_file_path"];
                menuOptionsString = File.ReadAllText(menuJsonFilePath);
            }

            return JsonSerializer.Deserialize<List<MenuOption>>(menuOptionsString);
        }
    }
}
