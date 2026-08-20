using System.Collections.Generic;

namespace Proyecto_Programacion_Avanzada.Models.Api
{
    public class DashboardDto
    {
        public int TotalConciertos { get; set; }
        public int TotalConciertosActivos { get; set; }
        public int TotalOrdenesPagadas { get; set; }
        public decimal TotalIngresos { get; set; }
        public int TotalEntradasVendidas { get; set; }
        public int ResenasPendientes { get; set; }
        public int UsuariosRegistrados { get; set; }

        public List<PuntoDashboardDto> IngresosPorMes { get; set; }
        public List<PuntoDashboardDto> ConciertosPorCategoria { get; set; }
    }

    public class PuntoDashboardDto
    {
        public string Etiqueta { get; set; }
        public decimal Valor { get; set; }
    }
}