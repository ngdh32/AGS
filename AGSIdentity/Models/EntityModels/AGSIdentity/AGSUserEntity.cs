using System;
using System.Collections.Generic;
using System.Linq;

namespace AGSIdentity.Models.EntityModels.AGSIdentity
{
    public class AGSUserEntity : AGSBaseEntity
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Title { get; set; }

        public List<string> GroupIds { get; set; } = new List<string>();

        public List<string> DepartmentIds { get; set; } = new List<string>();

        public AGSUserEntity()
        {
        }
    }
}
