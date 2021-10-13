using IdentityServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<AppDbContext>(config => {
                config.UseInMemoryDatabase("IdentityDB");
            });

            services.AddIdentity<IdentityUser, IdentityRole>(config => {
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequiredLength = 4;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AppDbContext>() // Note 1
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Auth/Login";
            });

            // IdentityServer4 is a open-source framework built on top of .net core authentication
            // This internally configures the service & middleware needed for authentication &
            // authorization in a much more developer friendly way.

            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()
                .AddInMemoryApiResources(Configurations.GetResourceScopes)
                .AddInMemoryIdentityResources(Configurations.GetUserScopes)
                .AddInMemoryClients(Configurations.GetClients)
                .AddDeveloperSigningCredential();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
