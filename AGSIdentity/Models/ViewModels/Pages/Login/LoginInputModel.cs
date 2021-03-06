﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AGSIdentity.Models.ViewModels.Pages.Login
{
    public class LoginInputModel
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
