using System;
using AGS.Models.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AGS.Pages
{
    public class HostModel : PageModel
    {
        public InitialAppStateModel initialAppState { get; set; }

        public HostModel()
        {
        }

        public void OnGet()
        {
            initialAppState = new InitialAppStateModel(HttpContext);
        }
    }
}
