using Authorization.AuthorizationRequirements;
using Authorization.Controllers;
using Authorization.CustomPolicyProvider;
using Authorization.Transformations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Authorization
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config => 
                {
                    config.Cookie.Name = "Grandma.Cookie";
                    config.LoginPath = "/Home/Authenticate/";
                });

            services.AddAuthorization(config => {
                // default constructor does the following
                //var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defaultAuthBuilder
                //.RequireAuthenticatedUser()
                ////.RequireClaim(ClaimTypes.DateOfBirth) // all identifiers must have DOB data
                //.Build();

                //config.DefaultPolicy = defaultAuthPolicy;


                //config.AddPolicy("Claim.DOB", policyBuilder =>
                //{
                //    policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                //});

                config.AddPolicy("Admin", policyBuilder => {
                    policyBuilder.RequireClaim(ClaimTypes.Role, "Admin");
                });

                config.AddPolicy("Claim.DOB", policyBuilder =>
                {
                    policyBuilder.RequiredCustomClaim(ClaimTypes.DateOfBirth);
                });
            });

            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, SecurityLevelRequirmentHandler>();
            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();
            services.AddScoped<IAuthorizationHandler, CookieJarAuthorizationHandler>();
            services.AddScoped<IClaimsTransformation, ClaimTransformtions>();

            services.AddControllersWithViews(config => {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuilder
                .RequireAuthenticatedUser()
                .Build();

                // this is the default authorize filter applied globally to 
                // all controllers without authorize attribute on action / controller
                // config.Filters.Add(new AuthorizeFilter(defaultAuthPolicy));
            });

            // to authorize razor pages we need to add them to the configuration 
            // as follows
            services.AddRazorPages()
                .AddRazorPagesOptions(config => {
                    config.Conventions.AuthorizePage("/Razor/Secure");
                    config.Conventions.AuthorizePage("/Razor/Policy","Admin");
                    // to apply authorization policy on a bunch of pages under a folder
                    config.Conventions.AuthorizeFolder("/Razor/SecuredPages");

                    // Specify an anonymous page in a secure folder
                    config.Conventions.AllowAnonymousToPage("/Razor/SecuredPages/Anonymous");

                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
