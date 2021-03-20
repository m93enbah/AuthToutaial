using IdentityExample.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace IdentityExample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Entity Framework Part
            //to inject AppDbContext on the whole application to initialize database on the memory
            services.AddDbContext<AppDbContext>(config => 
            {
                config.UseInMemoryDatabase("Memory");
            });

            //Identity bridge Part(used to generate repositories that is collection of abstraction methods)
            //AddDefaultTokenProviders: used to generate Token provider to generate Tokens for reset passwords
            //AddEntityFrameworkStores: used to link the Identity layer with database to communicate
            //AddIdentity : registers the services
            services.AddIdentity<IdentityUser, IdentityRole>(config => {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Home/AccessDenied";
                options.Cookie.Name = "IdentityCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = new TimeSpan(0, 15, 0);
                options.LoginPath = "/Account/Login";
                options.ReturnUrlParameter = "RedirectUrl";
                options.LogoutPath = "/Account/Logout";
            });

            //to apply controllers with views 
            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //we apply the routing middleware to activate routing to whicle endpoint we want
            app.UseRouting();

            //means how are you?
            app.UseAuthentication();

            //we have to make sure that the authorization middleware must set after the routing middleware 
            //we have also to inject the authenticaiton cookie to allow pass the autorization middleware 
            //means : are you allowed?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
