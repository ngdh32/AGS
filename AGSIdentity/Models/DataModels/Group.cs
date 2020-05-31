using System;
using Microsoft.AspNetCore.Identity;

namespace AGSIdentity.Models.DataModels
{
    public class Group
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Group()
        {
        }

        // this constructor is for converting ASP.NET identity Roleto AGS Group Model
        public Group(IdentityRole role)
        {
            if (role != null)
            {
                this.Id = role.Id;
                this.Name = role.Name;
            }
            
        }

        // this constructor is for converting AD group  to AGS Group Model
        // to be done
    }
}
