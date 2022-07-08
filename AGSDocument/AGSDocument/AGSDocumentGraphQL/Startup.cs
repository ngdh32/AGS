using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Authentication;
using System.Threading.Tasks;
using AGSDocumentCore.Interfaces.Repositories;
using AGSDocumentCore.Interfaces.Services;
using AGSDocumentCore.Repositories;
using AGSDocumentCore.Services;
using AGSDocumentInfrastructureEF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using AGSDocumentFileService;

namespace AGSDocumentGraphQL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // this is for ef migration file generation
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<EFDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("Database"), ServerVersion.AutoDetect(Configuration.GetConnectionString("Database")), sql => sql.MigrationsAssembly(migrationsAssembly)));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["AuthUrl"];
                    options.Audience = "ags.document";
                    options.RequireHttpsMetadata = false;
                    options.BackchannelHttpHandler = GetJWTBearerTokenHandler();
                    options.SaveToken = true;
                });

            services.AddTransient<IFolderRepository, EFFolderRepository>();
            services.AddTransient<IFileIndexingService, FileIndexingService>();
            services.AddTransient<IUserRepository, AGSUserRepository>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IAGSDocumentQueryService, AGSDocumentQueryService>();

            services
                .AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType<AGSDocumentQueryType>();

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            // app.UseAuthorization();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });

        }

        private static HttpClientHandler GetJWTBearerTokenHandler()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.SslProtocols = SslProtocols.Tls12;
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            return handler;
        }
    }
}
