using System;
using AGSCommon.Models.EntityModels.AGSIdentity;

namespace AGSCommon.Models.ViewModels.AGSIdentity
{
    public class AGSUserWithPasswordModel : AGSUserEntity
    {
        public string Password { get; set; }

        public AGSUserWithPasswordModel()
        {
        }
    }
}
