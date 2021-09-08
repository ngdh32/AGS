# AGS Identity
- To work as an identity server for other AGS components with OpenID Connect
- To provide users' information, groups, departments and claims

## To start:
1. Install the database (Any database that has a EF core provider)
2. Instal the .net core SDK 3.1
3. Run the following commands one by one to set up the database
``` sh
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Migrations/IdentityServer/PersistedGrantDb 
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Migrations/IdentityServer/ConfigurationDb
dotnet ef migrations add InitialApplicationDbContext -c EFApplicationDbContext -o Migrations/Application/EFApplicationDb
dotnet ef database update InitialIdentityServerPersistedGrantDbMigration -c  PersistedGrantDbContext
dotnet ef database update InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext
dotnet ef database update InitialApplicationDbContext -c EFApplicationDbContext
```
4. Run the application (Dotnet restore may be needed) 


## Remarks:
- Use data_initialization in appsettings.json to decide whether a data seed is run
- The certificate vgt.pfx is used to preserve the token after the restart of the program. Best practice is to directly retrieve the certificate from the certificate store. The reason why it uses a static certificate file instead is because free tier app service plan of Azure does not offer the store