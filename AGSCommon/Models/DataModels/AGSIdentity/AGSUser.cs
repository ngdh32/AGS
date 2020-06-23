using System;
using System.Collections.Generic;

namespace AGSCommon.Models.DataModels.AGSIdentity
{
    public class AGSUser : AGSIdentityDataModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<AGSGroup> Groups { get; set; } = new List<AGSGroup>(); // prevent null reference exception

    }
}
