using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using BookStoreApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(BookStoreApi.Startup1))]

namespace BookStoreApi
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // It's Becouse we use Owins.cors
            app.UseCors(CorsOptions.AllowAll);
            
            //use middleware that create token outhantication
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                AllowInsecureHttp=true,
                Provider=new TokenCreate()
            });

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            app.UseWebApi(config);

           
        }
    }

    internal class TokenCreate : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add(" Access - Control - Allow - Origin ",new[] {"*"});
            ApplicationDBContext contextDB = new ApplicationDBContext();
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(contextDB);
            UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = await manager.FindAsync(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("grant_error", " UserName And Password not Valid . ");
            }
            else
            {
                ClaimsIdentity claims = new ClaimsIdentity(context.Options.AuthenticationType);
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                claims.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                if(manager.IsInRole(user.Id,"Admin"))
                {
                    claims.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                }
                context.Validated(claims);
            }
        }

    }
}
