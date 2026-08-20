using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Proyecto.Data.Entities;
using System.Web;
using Microsoft.AspNet.Identity.Owin;


namespace Proyecto_Programacion_Avanzada.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
        }

        public ActionResult Index()
        {
            var usuarios = UserManager.Users
                .OrderBy(u => u.Apellidos)
                .ThenBy(u => u.Nombre)
                .ToList();

            return View(usuarios);
        }

        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return HttpNotFound();

            var usuario =
                await UserManager.FindByIdAsync(id);

            if (usuario == null)
                return HttpNotFound();

            return View(usuario);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            UsuarioCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Nombre = model.Nombre,
                Apellidos = model.Apellidos,
                Cedula = model.Cedula,
                Telefono = model.Telefono,
                FechaRegistro = DateTime.Now,
                Activo = true
            };

            var result =
                await UserManager.CreateAsync(
                    usuario,
                    model.Password
                );

            if (result.Succeeded)
            {
                await UserManager.AddToRoleAsync(
                    usuario.Id,
                    model.Rol
                );

                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);

            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return HttpNotFound();

            var usuario =
                await UserManager.FindByIdAsync(id);

            if (usuario == null)
                return HttpNotFound();

            var model = new UsuarioEditViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                Email = usuario.Email,
                Cedula = usuario.Cedula,
                Telefono = usuario.Telefono,
                Activo = usuario.Activo
            };

            var roles =
                await UserManager.GetRolesAsync(
                    usuario.Id
                );

            model.Rol =
                roles.FirstOrDefault()
                ?? "Asociado";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            UsuarioEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario =
                await UserManager.FindByIdAsync(model.Id);

            if (usuario == null)
                return HttpNotFound();

            usuario.Nombre = model.Nombre;
            usuario.Apellidos = model.Apellidos;
            usuario.Email = model.Email;
            usuario.UserName = model.Email;
            usuario.Cedula = model.Cedula;
            usuario.Telefono = model.Telefono;
            usuario.Activo = model.Activo;

            var result =
                await UserManager.UpdateAsync(usuario);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);

                return View(model);
            }

            var roles =
                await UserManager.GetRolesAsync(
                    usuario.Id
                );

            if (roles.Any())
            {
                await UserManager.RemoveFromRolesAsync(
                    usuario.Id,
                    roles.ToArray()
                );
            }

            await UserManager.AddToRoleAsync(
                usuario.Id,
                model.Rol
            );

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return HttpNotFound();

            var usuario =
                await UserManager.FindByIdAsync(id);

            if (usuario == null)
                return HttpNotFound();

            return View(usuario);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return HttpNotFound();

            var usuario =
                await UserManager.FindByIdAsync(id);

            if (usuario == null)
                return HttpNotFound();

            // No eliminamos físicamente al usuario.
            // Lo desactivamos para conservar historial.
            usuario.Activo = false;

            var result =
                await UserManager.UpdateAsync(usuario);

            if (!result.Succeeded)
            {
                TempData["Error"] =
                    string.Join(
                        ", ",
                        result.Errors
                    );
            }

            return RedirectToAction("Index");
        }
    }

    public class UsuarioCreateViewModel
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellidos { get; set; }

        public string Cedula { get; set; }

        public string Telefono { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(
            100,
            MinimumLength = 6
        )]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Rol { get; set; }
    }

    public class UsuarioEditViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellidos { get; set; }

        public string Cedula { get; set; }

        public string Telefono { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Rol { get; set; }

        public bool Activo { get; set; }
    }
}