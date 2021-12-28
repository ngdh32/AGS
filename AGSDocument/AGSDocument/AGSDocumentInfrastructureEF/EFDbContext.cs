using System;
using AGSDocumentInfrastructureEF.Entities;
using Microsoft.EntityFrameworkCore;

namespace AGSDocumentInfrastructureEF
{
    public class EFDbContext : DbContext
    {
        public DbSet<EFAGSFolder> Folders { get; set; }
        public DbSet<EFAGSFile> Files { get; set; }
        public DbSet<EFAGSFolderPermission> FolderPermissions { get; set; }

        public EFDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EFAGSFile>()
                .HasOne<EFAGSFolder>(file => file.EFAGSFolder)
                .WithMany(folder => folder.EFAGSFiles)
                .HasForeignKey(file => file.FolderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EFAGSFolderPermission>()
                .HasOne<EFAGSFolder>(permission => permission.Folder)
                .WithMany(folder => folder.EFAGSFolderPermissions)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}









