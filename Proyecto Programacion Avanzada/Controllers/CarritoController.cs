using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Proyecto.Service;

namespace Proyecto_Programacion_Avanzada.Controllers
{
    [Authorize(Roles = "Asociado")]
    public class CarritoController : Controller
    {
        private readonly ICarritoService _carritoService;
        private readonly ICompraService _compraService;

        public CarritoController(
            ICarritoService carritoService,
            ICompraService compraService)
        {
            _carritoService = carritoService;
            _compraService = compraService;
        }

        private string ObtenerUsuarioId()
        {
            if (!User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException(
                    "Debe iniciar sesión para acceder al carrito."
                );
            }

            var usuarioId = User.Identity.GetUserId();

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                throw new UnauthorizedAccessException(
                    "No se pudo obtener el usuario autenticado."
                );
            }

            return usuarioId;
        }

        public ActionResult Index()
        {
            var usuarioId = ObtenerUsuarioId();

            var carrito =
                _carritoService.ObtenerCarritoActivo(usuarioId);

            return View(carrito);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(
            int tipoEntradaId,
            int cantidad)
        {
            try
            {
                var usuarioId = ObtenerUsuarioId();

                _carritoService.AgregarEntrada(
                    usuarioId,
                    tipoEntradaId,
                    cantidad
                );

                TempData["Mensaje"] =
                    "Entrada agregada al carrito.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(
                    "Index",
                    "Cartelera"
                );
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Actualizar(
            int carritoDetalleId,
            int cantidad)
        {
            var usuarioId = ObtenerUsuarioId();

            _carritoService.ActualizarCantidad(
                usuarioId,
                carritoDetalleId,
                cantidad
            );

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(
            int carritoDetalleId)
        {
            var usuarioId = ObtenerUsuarioId();

            _carritoService.EliminarDetalle(
                usuarioId,
                carritoDetalleId
            );

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comprar()
        {
            var usuarioId = ObtenerUsuarioId();

            var resultado =
                _compraService.ConfirmarCompra(
                    usuarioId,
                    "Tarjeta"
                );

            if (!resultado.Exitoso)
            {
                TempData["Error"] =
                    resultado.Mensaje;

                return RedirectToAction("Index");
            }

            TempData["Mensaje"] =
                resultado.Mensaje;

            return RedirectToAction(
                "Details",
                "Ordenes",
                new { id = resultado.OrdenId }
            );
        }
    }
}