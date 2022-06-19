dotnet ef migrations add InitialApplicationDbContext -c EFDbContext -o Migrations/Application/EFDbContextMigration
dotnet ef database update InitialApplicationDbContext -c EFDbContext