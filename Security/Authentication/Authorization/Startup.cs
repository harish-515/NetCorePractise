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
            });
        }
    }
}
