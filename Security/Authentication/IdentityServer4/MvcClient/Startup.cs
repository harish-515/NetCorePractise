using Microsoft.AspNetCore.Authentication;
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

                    // when we load all the claims into the Id token it becomes bulk 
                    // instead of that we can have a second request done after we get
                    // the id token to populate the user claims
                    config.GetClaimsFromUserInfoEndpoint = true;

                    // when we use user endpoint to populate the claims from a json
                    // we need to further set a mapping so that we get the required stuff only
                    config.ClaimActions.DeleteClaim("arm"); 
                    config.ClaimActions.MapUniqueJsonKey("Client.Custom.Claim", "some.custom.claim"); 

                    // this openid connect requests for 2 scope by default
                    // openid & profile

                    // to only request the scopes that are required as per our needs \
                    config.Scope.Clear();
                    config.Scope.Add("openid");
                    // any additional scopes that are needed shoube explicitly requested as follows 
                    config.Scope.Add("custom.scopes");
                    config.Scope.Add("ApiOne");
                    config.Scope.Add("ApiTwo");

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
