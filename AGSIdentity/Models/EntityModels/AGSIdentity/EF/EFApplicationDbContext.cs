using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AGSIdentity.Models.EntityModels.AGSIdentity.EF
{
    public class EFApplicationDbContext : IdentityDbContext<EFApplicationUser, EFApplicationRole, string>
    {
        public EFApplicationDbContext(DbContextOptions<EFApplicationDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // creating the identity schema first
            base.OnModelCreating(modelBuilder);

            // setup the many-to-many relationship between users and departments
            modelBuilder.Entity<EFApplicationUserDepartment>()
                .HasKey(ud => new { ud.UserId, ud.DepartmentId });
            modelBuilder.Entity<EFApplicationUserDepartment>()
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserDepartments)
                .HasForeignKey(ud => ud.UserId);
            modelBuilder.Entity<EFApplicationUserDepartment>()
                .HasOne(ud => ud.Department)
                .WithMany(d => d.UserDepartments)
                .HasForeignKey(ud => ud.DepartmentId);

        }

        public DbSet<EFFunctionClaim> FunctionClaims { get; set; }

        public DbSet<EFDepartment> Departments { get; set; }

        public DbSet<EFApplicationUserDepartment> UserDepartments { get; set; }
    }
}
