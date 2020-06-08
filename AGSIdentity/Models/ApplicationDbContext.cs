using System;
using AGSCommon.Models.DataModels.AGSIdentity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AGSIdentity.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AGSMenu> Menus { get; set; }

        public DbSet<AGSFunctionClaim> FunctionClaims { get; set; }
    }
}
