using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Basics
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {   
            //we have to inject the authenticaiton in order to access the authorization middleware 
            //we see that we specify the login path foreach authorize action in any controller it will redirect to the Authenticate
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                       { 
                           config.Cookie.Name = "Grandmas.Cookie";
                           config.LoginPath = "/Home/Authenticate";
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
            //we apply the routing middleware to activate routing to whicle endpoint we want
            app.UseRouting();

            //means how you are?
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
