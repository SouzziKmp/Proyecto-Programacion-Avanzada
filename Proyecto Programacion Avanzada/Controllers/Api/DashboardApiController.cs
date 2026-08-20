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

            var ingresosPorMes = ordenesPagadas
                .GroupBy(o => new { o.FechaOrden.Year, o.FechaOrden.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Total = g.Sum(o => o.Total)
                })
                .ToList()
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .Select(x => new PuntoDashboardDto
                {
                    Etiqueta = x.Month.ToString("00") + "/" + x.Year,
                    Valor = x.Total
                })
                .ToList();

            var conciertosPorCategoria = conciertos
                .GroupBy(c => c.Categoria.Nombre)
                .Select(g => new PuntoDashboardDto
                {
                    Etiqueta = g.Key,
                    Valor = g.Count()
                })
                .ToList();

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
                UsuariosRegistrados = usuarios.Count(),
                IngresosPorMes = ingresosPorMes,
                ConciertosPorCategoria = conciertosPorCategoria
            };

            return Ok(dto);
        }
    }
}