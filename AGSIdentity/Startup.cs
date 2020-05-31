using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AGSIdentity.Models;
using AGSIdentity.Repositories;
using AGSIdentity.Repositories.EF;
using AGSIdentity.Services;
using AGSIdentity.Services.Auth;
using AGSIdentity.Services.Auth.Identity;
using AGSIdentity.Services.ExceptionFactory;
using AGSIdentity.Services.ExceptionFactory.Json;
//using AGSIdentity.Services.Identity;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AGSIdentity
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
            string connectionString = Configuration.GetConnectionString("Database");

            #region Setup Identity Server
            // use mysql to interact with customized identity db context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString));

            // use customized identity user in identity & take application db context as the db for entity framework
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure asp.net Identity settings
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            // this is for ef migration file generation
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // Add identity server for OAuth 2.0
            services.AddIdentityServer()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                options.EnableTokenCleanup = true;
            })
            .AddAspNetIdentity<ApplicationUser>()
            .AddSigningCredential(GetCertificate()); // use the certificate so that the token is still valid after application is rebooted
            #endregion

            #region Setup JWT Bearer Authentication
            // add authentication using bearer 
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = Configuration["auth_url"];
                options.RequireHttpsMetadata = false;
                options.Audience = "api1";
            });
            #endregion

            // add controllers 
            services.AddControllersWithViews();
            services.AddRazorPages();

            // add repository object
            services.AddSingleton<IExceptionFactory, JsonExceptionFactory>();
            services.AddHttpContextAccessor();
            services.AddTransient<IAuthService, IdentityAuthService>();
            services.AddTransient<IRepository, EFRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }

        /// <summary>
        /// This is to get the certificate by the file path. Normally you will get it from certificate store instead
        /// but the certificate store is not included in azure app free plan, use local file instead
        /// </summary>
        /// <returns></returns>
        private X509Certificate2 GetCertificate()
        {
            X509Certificate2 cert = null;

            string certPath = Configuration["cert_path"];
            if (string.IsNullOrEmpty(certPath))
            {
                certPath = Environment.GetEnvironmentVariable("HOME") + @"\site\wwwroot";
            }

            cert = new X509Certificate2(certPath, Configuration["cert_pw"], X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            return cert;
        }
    }
}
