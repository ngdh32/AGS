using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AGSIdentity.Models.EntityModels.AGSIdentity.EF
{
    public class EFApplicationUser : IdentityUser
    {
        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Title { get; set; }

        public ICollection<EFApplicationUserDepartment> UserDepartments { get; set; }
    }
}
