using System;
using AGSCommon.Models.DataModels.AGSIdentity;
using Microsoft.AspNetCore.Identity;

namespace AGSIdentity.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
        }

        // this constructor is for converting ASP.NET identity Roleto AGS Group Model
        public AGSGroup GetAGSGroup()
        {
            var group = new AGSGroup()
            {
                Id = this.Id,
                Name = this.Name
            };

            return group;
        }

        // this constructor is for converting AD group  to AGS Group Model
        // to be done
    }
}
