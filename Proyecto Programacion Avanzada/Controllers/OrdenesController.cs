using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Proyecto.Service;

namespace Proyecto_Programacion_Avanzada.Controllers
{
    public class OrdenesController : Controller
    {
        private readonly IOrdenService _ordenService;

        public OrdenesController(IOrdenService ordenService)
        {
            _ordenService = ordenService;
        }

        private string ObtenerUsuarioId()
        {
            var usuarioId = User.Identity.GetUserId();

            if (!string.IsNullOrWhiteSpace(usuarioId))
                return usuarioId;

            var connectionString = ConfigurationManager
                .ConnectionStrings["ProyectoFinalDb"]
                .ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("SELECT Id FROM AspNetUsers WHERE Email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", "socio@proyectofinal.com");

                connection.Open();

                var result = command.ExecuteScalar();

                if (result == null)
                    throw new InvalidOperationException("No existe el usuario socio@proyectofinal.com. Ejecute las migraciones/seed primero.");

                return result.ToString();
            }
        }

        public ActionResult Historial()
        {
            var usuarioId = ObtenerUsuarioId();
            var ordenes = _ordenService.ObtenerHistorial(usuarioId);

            return View(ordenes);
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Historial");

            var usuarioId = ObtenerUsuarioId();
            var orden = _ordenService.ObtenerDetalle(usuarioId, id.Value);

            if (orden == null)
                return HttpNotFound();

            return View(orden);
        }
    }
}