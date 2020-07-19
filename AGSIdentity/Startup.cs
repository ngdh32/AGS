using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AGSIdentity.Models;
using AGSIdentity.Repositories;
using AGSIdentity.Repositories.EF;
using AGSIdentity.Services.ProfileService.Identity;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
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
using IdentityServer4.Services;
using AGSIdentity.Models.EntityModels.EF;
using AGSIdentity.Services.AuthService;
using AGSIdentity.Services.AuthService.Identity;
using AGSIdentity.Data;
using AGSIdentity.Data.EF;

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
            services.AddDbContext<EFApplicationDbContext>(options =>
                options.UseMySql(connectionString));

            // use customized identity user in identity & take application db context as the db for entity framework
            services.AddIdentity<EFApplicationUser, EFApplicationRole>()
                .AddEntityFrameworkStores<EFApplicationDbContext>()
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
            services.AddTransient<IProfileService, IdentityProfileService>(); // customized IProfile servoce

            services.AddIdentityServer(options =>
            {
                // set up the login page url
                options.UserInteraction.LoginUrl = "/login";
                options.UserInteraction.LogoutUrl = "/logout";
            })
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
            .AddAspNetIdentity<EFApplicationUser>() // use asp.net identity user as the user of identity server
            .AddSigningCredential(GetCertificate()) // use the certificate so that the token is still valid after application is rebooted
            .AddProfileService<IdentityProfileService>(); // add the service of customization of token
            #endregion

            #region Setup JWT Bearer Authentication
            // add authentication using bearer 
            services.AddAuthentication(options =>
            {
                // this is for telling asp.net core identity that the user has been logined
                options.DefaultSignInScheme  = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // this is for authenticating when calling API
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 

            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = Configuration["auth_url"];
                options.RequireHttpsMetadata = false;
                options.Audience = AGSCommon.CommonConstant.AGSIdentityConstant.AGSIdentityScopeConstant;
            });
            #endregion

            // add controllers 
            services.AddControllersWithViews();
            services.AddRazorPages();

            // Add API Versioning  
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
            });

            // add repository object
            services.AddHttpContextAccessor();
            services.AddTransient<IAuthService, IdentityAuthService>();
            services.AddTransient<IRepository, EFRepository>();
            services.AddTransient<IFunctionClaimRepository, EFFunctionClaimRepository>();
            services.AddTransient<IGroupRepository, EFGroupRepository>();
            services.AddTransient<IUserRepository, EFUserRepository>();

            // for data initialization
            services.AddTransient<IDataSeed, EFDataSeed>();

            // add authorization policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AGSCommon.CommonConstant.AGSIdentityConstant.AGSPolicyConstant, policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy(AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionClaimEditPolicyConstant, policy =>
                {
                    //policy.RequireClaim(AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant, AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionClaimEditClaimConstant);
                    policy.RequireAuthenticatedUser();
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAssertion(context =>
                    {
                        return context.User.HasClaim(claim =>
                            claim.Type == AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant
                            && claim.Value == AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionClaimEditClaimConstant)
                            || context.User.IsInRole(AGSCommon.CommonConstant.AGSIdentityConstant.AGSAdminName);
                    });
                });
                options.AddPolicy(AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditPolicyConstant, policy =>
                {
                    //policy.RequireClaim(AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant, AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditClaimConstant);
                    policy.RequireAuthenticatedUser();
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAssertion(context =>
                    {
                        return context.User.HasClaim(claim =>
                            claim.Type == AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant
                            && claim.Value == AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditClaimConstant)
                            || context.User.IsInRole(AGSCommon.CommonConstant.AGSIdentityConstant.AGSAdminName);
                    });
                });
                options.AddPolicy(AGSCommon.CommonConstant.AGSIdentityConstant.AGSGroupEditPolicyConstant, policy =>
                {
                    //policy.RequireClaim(AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant, AGSCommon.CommonConstant.AGSIdentityConstant.AGSGroupEditClaimConstant);
                    policy.RequireAuthenticatedUser();
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAssertion(context =>
                    {
                        return context.User.HasClaim(claim =>
                            claim.Type == AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant
                            && claim.Value == AGSCommon.CommonConstant.AGSIdentityConstant.AGSGroupEditClaimConstant)
                            || context.User.IsInRole(AGSCommon.CommonConstant.AGSIdentityConstant.AGSAdminName);
                    });
                });
                options.AddPolicy(AGSCommon.CommonConstant.AGSIdentityConstant.AGSConfigEditPolicyConstant, policy =>
                {
                    //policy.RequireClaim(AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant, AGSCommon.CommonConstant.AGSIdentityConstant.AGSConfigEditClaimConstant);
                    policy.RequireAuthenticatedUser();
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAssertion(context =>
                    {
                        return context.User.HasClaim(claim =>
                            claim.Type == AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant
                            && claim.Value == AGSCommon.CommonConstant.AGSIdentityConstant.AGSConfigEditClaimConstant)
                            || context.User.IsInRole(AGSCommon.CommonConstant.AGSIdentityConstant.AGSAdminName);
                    });
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (Configuration["data_initialization"] == "Y") {
                InitializeDatabase(app);
            }
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }else
            {
                app.UseExceptionHandler("/error");
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


        /// <summary>
        /// This is to initialize the database by coding instead of seeding data or sql scripts. In my opinion,
        /// the former is much easier way to initialize the database than the later two. If later on
        /// the authentication changes to AD authentication, then this function may no longer be useful
        /// </summary>
        /// <param name="app"></param>
        /// <param name="userManager"></param>
        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                //var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<EFApplicationUser>>();
                //var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<EFApplicationRole>>();
                //var applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<EFApplicationDbContext>();
                //var configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                var dataSeed = serviceScope.ServiceProvider.GetRequiredService<IDataSeed>();
                dataSeed.RemoveApplicationData();
                dataSeed.RemoveAuthenticationServerData();
                dataSeed.InitializeApplicationData();
                dataSeed.InitializeAuthenticationServer();
            }
        }
    }
}
