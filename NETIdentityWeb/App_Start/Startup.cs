using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;


[assembly: OwinStartup(typeof(NETIdentityWeb.App_Start.Startup))]

namespace NETIdentityWeb.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string connectionString = 
                ConfigurationManager.ConnectionStrings["DefaultConnection"]
                .ConnectionString; 
            
            app.CreatePerOwinContext(() => new IdentityDbContext(connectionString));
            app.CreatePerOwinContext<UserStore<IdentityUser>>((opt, cont) => 
                    new UserStore<IdentityUser>(cont.Get<IdentityDbContext>())
                    );
            app.CreatePerOwinContext<UserManager<IdentityUser>>((opt, cont) =>
            new UserManager<IdentityUser>(cont.Get<UserStore<IdentityUser>>())
                );

            //take creds, verify and issue cookie 
            app.CreatePerOwinContext<SignInManager<IdentityUser, string>>(
                (opt, cont) =>
                    new SignInManager<IdentityUser, string>
                    (cont.Get<UserManager<IdentityUser>>(), cont.Authentication)
                );



            //add cookie support
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });
                
            


        }
    }
}
