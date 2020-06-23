using System;
using AGSIdentity.Models.EntityModels.EF;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AGSIdentity.Models.EntityModels.EF
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<FunctionClaim> FunctionClaims { get; set; }
    }
}
