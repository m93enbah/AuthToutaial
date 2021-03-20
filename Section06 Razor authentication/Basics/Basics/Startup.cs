using Basics.Authorization;
using Basics.Handlers;
using Basics.Transformer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Claims;

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
                           config.AccessDeniedPath = "/Home/AccessDenied";
                       });

            //add authorization and set the default requirement to default policy that used on each [Authorize] only 
            //services.AddAuthorization(config =>
            //{
            //    var defaultAuthBuilder = new AuthorizationPolicyBuilder();
            //    //it will create two requirements for the policy 
            //    var defaultAuthPolicy = defaultAuthBuilder
            //                            .RequireAuthenticatedUser()
            //                            .RequireClaim(ClaimTypes.DateOfBirth)
            //                            .Build();
            //    config.DefaultPolicy = defaultAuthPolicy;
            //});


            //with using Authorization with custom policy 
            //normal way (manual inject claim types into the custom policy)
            //services.AddAuthorization(config =>
            //{
            //    config.AddPolicy("Claim.Dob", policyBuilder =>
            //    {
            //        policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
            //    });
            //});

            //with custom handler to do this automatically
            //services.AddAuthorization(config =>
            //{
            //    config.AddPolicy("Claim.Dob", policyBuilder =>
            //    {
            //        policyBuilder.AddRequirements(new CustomRequireClaims(ClaimTypes.DateOfBirth));
            //    });
            //});


            //we can make it like extension method on AuthorizationPolicyBuildeExtensions
            //services.AddAuthorization(config =>
            //{
            //    config.AddPolicy("Claim.Dob", policyBuilder =>
            //    {
            //        policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
            //    });
            //    //we can add policy on role also as below
            //    config.AddPolicy("Admin", policyBuilder =>
            //    {
            //        policyBuilder.RequireClaim(ClaimTypes.Role, "Admin");
            //    });
            //});

            //inject the authorization handler 
            services.AddScoped<IAuthorizationHandler, CustomeRequireClaimHandler>();
            //inject the operation authorization handler
            services.AddScoped<IAuthorizationHandler, CookieJarAuthorizationHandler>();
            //inject the claims transformation
            services.AddScoped<IClaimsTransformation, ClaimsTransformer>();

            //services.AddAuthorization(config =>
            //{
            //    config.AddPolicy("Admin", policyBuilder =>
            //    {
            //        policyBuilder.RequireClaim(ClaimTypes.Role);
            //    });
            //});

            //inject the auth policy handler 
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationBasePolicyProvider>();
            services.AddScoped<IAuthorizationHandler, AuthorizationBaseHandler>();

            //to apply controllers with views with apply 
            services.AddControllersWithViews(config => {
                //build global authorization filter (you must set AllowAnonmous attribute on Authenticate action)
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuilder.RequireAuthenticatedUser().Build();
                //config.Filters.Add(new AuthorizeFilter(defaultAuthPolicy));
            });

            //it will set authorization to the razor page /Razor/Secured razor page
            services.AddRazorPages().AddRazorPagesOptions(config => 
            {
                config.Conventions.AuthorizePage("/Razor/Secured");
                config.Conventions.AuthorizePage("/Razor/Secured", "Admin");
            });
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
                endpoints.MapRazorPages();
            });
        }
    }
}
