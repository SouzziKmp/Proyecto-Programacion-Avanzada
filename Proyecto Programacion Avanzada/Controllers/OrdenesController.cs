using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Proyecto.Service;

namespace Proyecto_Programacion_Avanzada.Controllers
{
    [Authorize(Roles = "Asociado")]
    public class OrdenesController : Controller
    {
        private readonly IOrdenService _ordenService;

        public OrdenesController(
            IOrdenService ordenService)
        {
            _ordenService = ordenService;
        }

        private string ObtenerUsuarioId()
        {
            if (!User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException();

            var usuarioId =
                User.Identity.GetUserId();

            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new UnauthorizedAccessException();

            return usuarioId;
        }

        public ActionResult Historial()
        {
            var usuarioId = ObtenerUsuarioId();

            var ordenes =
                _ordenService.ObtenerHistorial(
                    usuarioId
                );

            return View(ordenes);
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Historial");

            var usuarioId = ObtenerUsuarioId();

            var orden =
                _ordenService.ObtenerDetalle(
                    usuarioId,
                    id.Value
                );

            if (orden == null)
                return HttpNotFound();

            return View(orden);
        }
    }
}