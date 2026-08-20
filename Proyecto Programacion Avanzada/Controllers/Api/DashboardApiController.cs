using System.Linq;
using System.Web.Http;
using Proyecto.Data.Entities;
using Proyecto.Repository;
using Proyecto_Programacion_Avanzada.Models.Api;

namespace Proyecto_Programacion_Avanzada.Controllers.Api
{
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("api/dashboard")]
    public class DashboardApiController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Resumen()
        {
            var conciertos = _unitOfWork.Repository<Concierto>().Query();
            var ordenes = _unitOfWork.Repository<Orden>().Query();
            var ordenDetalles = _unitOfWork.Repository<OrdenDetalle>().Query();
            var resenas = _unitOfWork.Repository<Resena>().Query();
            var usuarios = _unitOfWork.Repository<ApplicationUser>().Query();

            var ordenesPagadas = ordenes.Where(o => o.Estado == 2);

            var dto = new DashboardDto
            {
                TotalConciertos = conciertos.Count(),
                TotalConciertosActivos = conciertos.Count(c => c.Estado == 1),
                TotalOrdenesPagadas = ordenesPagadas.Count(),
                TotalIngresos = ordenesPagadas.Sum(o => (decimal?)o.Total) ?? 0,
                TotalEntradasVendidas = ordenDetalles
                    .Where(d => d.Orden.Estado == 2)
                    .Sum(d => (int?)d.Cantidad) ?? 0,
                ResenasPendientes = resenas.Count(r => r.Estado == 0),
                UsuariosRegistrados = usuarios.Count()
            };

            return Ok(dto);
        }
    }
}