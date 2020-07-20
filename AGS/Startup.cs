using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AGS.Data;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using AGS.Services.AGS.Menu;
using AGS.Services.AGS.Menu.AGS;
using AGS.Repositories.Menu;
using AGS.Repositories.Menu.Json;
using AGS.Services.AGS.Localization;
using AGS.Services.AGS.Localization.Json;
using AGS.Services.AGSIdentity;
using AGS.Services.AGSIdentity.API;
using AGS.Services.AGS.CurrentUser.HttpContext;
using AGS.Services.AGS.CurrentUser;

namespace AGS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // add oidc authentication and use cookie to store the token
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = Configuration["auth_url"];
                options.RequireHttpsMetadata = true;
                options.ClientId = AGSCommon.CommonConstant.AGSIdentityConstant.AGSClientIdConstant;
                options.ClientSecret = Configuration["auth_client_secret"];
                options.ResponseType = "id_token token";
                options.SaveTokens = true;
                // options.GetClaimsFromUserInfoEndpoint = true;
                // add the necessary scopes
                options.Scope.Add(AGSCommon.CommonConstant.AGSIdentityConstant.AGSDocumentScopeConstant); // api resource
                options.Scope.Add(AGSCommon.CommonConstant.AGSIdentityConstant.AGSIdentityScopeConstant); // api resource
                options.Scope.Add(IdentityModel.JwtClaimTypes.Profile); // identity resource
                options.Scope.Add(IdentityModel.JwtClaimTypes.Email); // identity resource
                options.Scope.Add("openid"); // this is mandatory for oidc implicit flow
                options.Scope.Add(AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionScopeConstant); // identity resource
            });

            // add authentitcation filter to middleware
            // this is applied to all pages. If there is a public page, then no need to add these lines of code
            services.AddMvcCore(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            // add httpcontextaccessor so that blazor component can access httpcontext
            services.AddHttpContextAccessor();

            services.AddSingleton<ILocalizationService, JsonLocalizationService>();
            services.AddSingleton<IMenuRepository, JsonMenuRepository>();
            services.AddTransient<IMenuService, AGSMenuService>();
            services.AddTransient<IAGSIdentityService, APIAGSIdentityService>();
            services.AddTransient<ICurrentUserService, HttpContextCurrentUserService>();

            services.AddHttpClient(AGSCommon.CommonConstant.AGSConstant.ags_identity_httpclient_name, c =>
            {
                c.BaseAddress = new Uri(Configuration["ags_identity_api_url"]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization(); // needed to be placed between UseRouting() and UseEndpoints()

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"); //adds mvc endpoint
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
