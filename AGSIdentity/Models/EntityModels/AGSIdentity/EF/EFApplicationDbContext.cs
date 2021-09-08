using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AGSIdentity.Models.EntityModels.AGSIdentity.EF
{
    public class EFApplicationDbContext : IdentityDbContext<EFApplicationUser, EFApplicationRole, string>
    {
        public EFApplicationDbContext(DbContextOptions<EFApplicationDbContext> options) : base(options) { }

        public DbSet<EFFunctionClaim> FunctionClaims { get; set; }

        public DbSet<EFDepartment> Departments { get; set; }
    }
}
