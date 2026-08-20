using System;
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
            var tiposEntrada = _unitOfWork.Repository<TipoEntrada>().Query();

            var ordenesPagadas = ordenes.Where(o => o.Estado == 2);
            var ahora = DateTime.Now;

            // Eventos activos y proximos cuyo aforo restante (sumado entre tipos de
            // entrada) cae por debajo del 20%: se consideran "proximos a agotarse".
            var eventosBajoAforo = tiposEntrada
                .Where(t => t.Concierto.Estado == 1 && t.Concierto.FechaEvento >= ahora)
                .GroupBy(t => new { t.ConciertoId, t.Concierto.Titulo })
                .Select(g => new
                {
                    g.Key.Titulo,
                    Disponibles = g.Sum(t => t.Disponibles),
                    Aforo = g.Sum(t => t.Aforo)
                })
                .ToList()
                .Where(x => x.Aforo > 0 && (decimal)x.Disponibles / x.Aforo < 0.2m)
                .OrderBy(x => (decimal)x.Disponibles / x.Aforo)
                .Select(x => new EventoBajoAforoDto
                {
                    Titulo = x.Titulo,
                    Disponibles = x.Disponibles,
                    Aforo = x.Aforo,
                    PorcentajeDisponible = x.Aforo == 0 ? 0 : Math.Round((decimal)x.Disponibles / x.Aforo * 100, 1)
                })
                .ToList();

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
                TotalConciertosProximos = conciertos.Count(c => c.Estado == 1 && c.FechaEvento >= ahora),
                TotalOrdenesPagadas = ordenesPagadas.Count(),
                TotalIngresos = ordenesPagadas.Sum(o => (decimal?)o.Total) ?? 0,
                TotalEntradasVendidas = ordenDetalles
                    .Where(d => d.Orden.Estado == 2)
                    .Sum(d => (int?)d.Cantidad) ?? 0,
                ResenasPendientes = resenas.Count(r => r.Estado == 0),
                UsuariosRegistrados = usuarios.Count(),
                UsuariosActivos = usuarios.Count(u => u.Activo),
                UsuariosInactivos = usuarios.Count(u => !u.Activo),
                IngresosPorMes = ingresosPorMes,
                ConciertosPorCategoria = conciertosPorCategoria,
                EventosBajoAforo = eventosBajoAforo
            };

            return Ok(dto);
        }
    }
}