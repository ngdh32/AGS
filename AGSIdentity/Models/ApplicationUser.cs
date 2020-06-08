using System;
using AGSCommon.Models.DataModels.AGSIdentity;
using Microsoft.AspNetCore.Identity;


namespace AGSIdentity.Models
{
    public class ApplicationUser : IdentityUser
    {
        // this is for converting ASP.NET Identity user to AGS User model
        public AGSUser GetAGSUser()
        {
            var user = new AGSUser()
            {
                Id = this.Id,
                Email = this.Email,
                Username = this.UserName
            };

            return user;
        }



        // this is for converting Active directory user to AGS User model
        // To be done...
    }
}
