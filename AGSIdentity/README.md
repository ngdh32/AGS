﻿Authorization:
Each function should be protected by a function claim.



dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Migrations/IdentityServer/ConfigurationDb
dotnet ef migrations add InitialApplicationDbContext -c EFApplicationDbContext -o Migrations/Application/EFApplicationDb
dotnet ef database update InitialIdentityServerPersistedGrantDbMigration -c  PersistedGrantDbContext
dotnet ef database update InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext
dotnet ef database update InitialApplicationDbContext -c EFApplicationDbContext