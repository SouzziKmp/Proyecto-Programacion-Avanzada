using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Proyecto.Data.Entities;
using Proyecto_Programacion_Avanzada.Models;

namespace Proyecto_Programacion_Avanzada.Controllers
{
    [Authorize] // hay que estar logeado para usar este controlador
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager =>
            _signInManager ?? (_signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>());

        public ApplicationUserManager UserManager =>
            _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>());

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resultado = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);

            switch (resultado)
            {
                case SignInStatus.Success:
                    await ActualizarUltimoLoginAsync(model.Email);
                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", "Esta cuenta quedo bloqueada temporalmente por varios intentos fallidos.");
                    return View(model);

                default:
                    ModelState.AddModelError("", "Correo o contrasena incorrectos.");
                    return View(model);
            }
        }

        private async Task ActualizarUltimoLoginAsync(string email)
        {
            var usuario = await UserManager.FindByEmailAsync(email);
            if (usuario == null) return;

            usuario.UltimoLogin = DateTime.Now;
            await UserManager.UpdateAsync(usuario);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = new ApplicationUser
            {
                Codigo = "USR-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                Nombre = model.Nombre,
                Apellidos = model.Apellidos,
                Cedula = model.Cedula,
                Telefono = model.Telefono,
                FechaRegistro = DateTime.Now,
                Activo = true
            };

            var resultado = await UserManager.CreateAsync(usuario, model.Password);
            if (resultado.Succeeded)
            {
                await UserManager.AddToRoleAsync(usuario.Id, "Asociado"); // <- aqui se asigna el rol
                await SignInManager.SignInAsync(usuario, isPersistent: false, rememberBrowser: false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in resultado.Errors)
                ModelState.AddModelError("", error);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager?.Dispose();
                _signInManager?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
