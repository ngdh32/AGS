using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AGSDocumentInfrastructureEF;
using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            AddGraphqlTypes(services);

            services.AddGraphQL(builder => builder
                .AddHttpMiddleware<AGSDocumentGraphQLSchema>()
                .AddSystemTextJson()
                .AddSchema<AGSDocumentGraphQLSchema>()
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();    
            app.UseGraphQL<AGSDocumentGraphQLSchema>();
            app.UseGraphQLPlayground();
            
            // app.UseEndpoints(endpoints =>
            // {
                
            // });
        }

        private void AddGraphqlTypes(IServiceCollection services)
        {
            services.AddTransient<AGSDocumentGraphQLSchema>();
            services.AddTransient<AGSPermissionGraphType>();
            services.AddTransient<AGSDocumentQueryType>();
            services.AddTransient<AGSDocumentMutationType>();
            services.AddTransient<AGSFileQueryViewGraphType>();
            services.AddTransient<AGSFolderQueryViewGraphType>();
        }
    }
}
