using System;

namespace AGSIdentity.Models.ViewModels.API.Users
{
    public class ChangePasswordRequestModel
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public ChangePasswordRequestModel()
        {
        }
    }
}
