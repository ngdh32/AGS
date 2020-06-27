using System;
using AGSIdentity.Models.EntityModels.EF;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AGSIdentity.Models.EntityModels.EF
{
    public class EFApplicationDbContext : IdentityDbContext<EFApplicationUser, EFApplicationRole, string>
    {
        public EFApplicationDbContext(DbContextOptions<EFApplicationDbContext> options) : base(options) { }

        public DbSet<EFMenu> Menus { get; set; }

        public DbSet<EFFunctionClaim> FunctionClaims { get; set; }
    }
}
