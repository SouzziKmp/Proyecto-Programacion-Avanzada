using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Proyecto.Data;
using Proyecto.Data.Entities;

namespace Proyecto_Programacion_Avanzada
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)),
                    OnApplyRedirect = ctx =>
                    {
                        // Las llamadas a la Web API deben recibir 401/403 planos, no un
                        // redirect HTML, para que el fetch() del cliente los detecte bien.
                        if (ctx.Request.Path.StartsWithSegments(new PathString("/api")))
                            return;

                        // Si ya inicio sesion pero le falta el rol (ej. Administrador
                        // intentando comprar), lo manda a AccessDenied en vez del login.
                        var usuario = ctx.OwinContext.Authentication.User;
                        if (usuario != null && usuario.Identity.IsAuthenticated)
                        {
                            ctx.Response.Redirect("/Account/AccessDenied");
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                    }
                },
                ExpireTimeSpan = TimeSpan.FromMinutes(60),
                SlidingExpiration = true
            });
        }
    }
}
