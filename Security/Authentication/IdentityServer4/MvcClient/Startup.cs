using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config => {
                config.DefaultScheme = "ClientCookie";
                // use this to check if we are allowed to do something.
                config.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("ClientCookie")
                .AddOpenIdConnect("oidc",config => {
                    config.Authority = "https://localhost:44310/";
                    config.ClientId = "client_id_mvc";
                    config.ClientSecret = "client_secret_mvc";
                    config.SaveTokens = true;
                    config.ResponseType = "code";
                    // this openid connect requests for 2 scope by default
                    // openid & profile
                });

            services.AddHttpClient();

            services.AddControllersWithViews();
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
