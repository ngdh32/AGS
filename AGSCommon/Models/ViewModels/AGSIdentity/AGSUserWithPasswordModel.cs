using System;
using AGSCommon.Models.EntityModels.AGSIdentity;

namespace AGSCommon.Models.ViewModels.AGSIdentity
{
    public class ChangeUserPasswordViewModel
    {
        public string UserId { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public ChangeUserPasswordViewModel()
        {
        }
    }
}
