using IdentityExample.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication("CookieAuth")
            //    .AddCookie("CookieAuth", config => {
            //        config.Cookie.Name = "Grandma.Cookie";
            //        config.LoginPath = "/Home/Authenticate/";
            //    });

            services.AddDbContext<AppDbContext>(config => {
                config.UseInMemoryDatabase("IdentityDB");
            });

            // A service that can be used to generate the identites 
            // of user based on specified user class & role class
            // gives access to couple of repositories to do these things.
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
                    config.Cookie.Name = "IdentityExample.Cookie";
                    config.LoginPath = "/Home/Login";
                });


            // Note 1 : inorder to communicate b/w the identity provider and the database
            // need to use Microsoft.AspNetCore.Identity.EntityFrameworkCore
            // which has the essential model classed to be used in dbcontext.


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
