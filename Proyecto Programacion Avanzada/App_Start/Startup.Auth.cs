using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Proyecto_Programacion_Avanzada
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(
                Proyecto.Data.ApplicationDbContext.Create
            );

            app.CreatePerOwinContext<
                ApplicationUserManager>(
                ApplicationUserManager.Create
            );

            app.CreatePerOwinContext<
                ApplicationSignInManager>(
                ApplicationSignInManager.Create
            );

            app.UseCookieAuthentication(
                new CookieAuthenticationOptions
                {
                    AuthenticationType =
                        DefaultAuthenticationTypes.ApplicationCookie,

                    LoginPath = new PathString("/Account/Login"),

                    Provider = new CookieAuthenticationProvider
                    {
                        OnValidateIdentity =
                            SecurityStampValidator
                            .OnValidateIdentity<
                                ApplicationUserManager,
                                Proyecto.Data.Entities.ApplicationUser
                            >(
                                validateInterval:
                                    System.TimeSpan.FromMinutes(30),

                                regenerateIdentity:
                                    (manager, user) =>
                                        user.GenerateUserIdentityAsync(manager)
                            )
                    },

                    ExpireTimeSpan =
                        System.TimeSpan.FromMinutes(60),

                    SlidingExpiration = true
                }
            );
        }
    }
}