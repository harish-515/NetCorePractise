using API.AuthenticationHandlers;
using API.JwtRequirements;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            // When ever an anonymous user tries to access a protected resource. Which internally 
            // hits the autorization services as the user is not authenticated. The request is CHALLENGED
            // .i.e. its (httpcontext) set back to authentication services to check for authentication
            // if authentic, then again the authorization checks for the mentions requirments as per 
            // authorization policy 
            // if user is not authentic, the request is denied there it self in the authentication services.

            services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("DefaultAuthScheme", null);

            services.AddAuthorization(config => {
                var defaultPolicy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new JwtRequirement())
                    .Build();

                config.DefaultPolicy = defaultPolicy;
            });
            services.AddScoped<IAuthorizationHandler,JwtRequirementHandler>();

            services.AddHttpClient()
                .AddHttpContextAccessor();

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
