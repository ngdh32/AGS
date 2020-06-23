using System;
using System.Collections.Generic;

namespace AGSIdentity.Models.EntityModels
{
    public class AGSUserEntity
    {
        public string Id { get; set; } = AGSCommon.CommonFunctions.GenerateId();

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<string> GroupIds { get; set; }

        public AGSUserEntity()
        {
        }
    }
}
