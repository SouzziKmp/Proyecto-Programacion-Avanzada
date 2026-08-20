using System.Collections.Generic;

namespace Proyecto_Programacion_Avanzada.Models.Api
{
    public class DashboardDto
    {
        public int TotalConciertos { get; set; }
        public int TotalConciertosActivos { get; set; }
        public int TotalConciertosProximos { get; set; }
        public int TotalOrdenesPagadas { get; set; }
        public decimal TotalIngresos { get; set; }
        public int TotalEntradasVendidas { get; set; }
        public int ResenasPendientes { get; set; }
        public int UsuariosRegistrados { get; set; }
        public int UsuariosActivos { get; set; }
        public int UsuariosInactivos { get; set; }

        public List<PuntoDashboardDto> IngresosPorMes { get; set; }
        public List<PuntoDashboardDto> ConciertosPorCategoria { get; set; }
        public List<EventoBajoAforoDto> EventosBajoAforo { get; set; }
    }

    public class PuntoDashboardDto
    {
        public string Etiqueta { get; set; }
        public decimal Valor { get; set; }
    }

    public class EventoBajoAforoDto
    {
        public string Titulo { get; set; }
        public int Disponibles { get; set; }
        public int Aforo { get; set; }
        public decimal PorcentajeDisponible { get; set; }
    }
}