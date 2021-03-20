using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;
using WebApiTokenAuthentication.Providers;

[assembly: OwinStartup(typeof(WebApiTokenAuthentication.Startup))]

namespace WebApiTokenAuthentication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            //enable cros origin request
            app.UseCors(CorsOptions.AllowAll);

            //configure oAuth server configuration
            var myProvider = new MyAuthorizationServerProvider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions 
            {
                AllowInsecureHttp = true,
                //this is the path that the user can request Token 
                TokenEndpointPath = new PathString("/token"),
                //the time that the Token will be valid
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                //pass the provider we created 
                Provider = myProvider
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


            //register HttpConfiguration
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}
