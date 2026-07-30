using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Data.Entities
{
    public class Ticket
    {
        public int TicketId { get; set; }

        [ForeignKey(nameof(OrdenDetalle))]
        public int OrdenDetalleId { get; set; }
        public virtual OrdenDetalle OrdenDetalle { get; set; }

        // Codigo unico del ticket (para QR). Unico a nivel de BD.
        public Guid CodigoUnico { get; set; } = Guid.NewGuid();

        // 1 = Valido, 2 = Usado, 3 = Anulado
        public byte Estado { get; set; } = 1;

        [Column(TypeName = "datetime2")]
        public DateTime FechaEmision { get; set; } = DateTime.Now;
    }
}
