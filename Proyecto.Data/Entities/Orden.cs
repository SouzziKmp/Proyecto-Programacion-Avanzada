using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Data.Entities
{
    public class Orden
    {
        public int OrdenId { get; set; }

        [Required, ForeignKey(nameof(Usuario))]
        public string UsuarioId { get; set; }
        public virtual ApplicationUser Usuario { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime FechaOrden { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal")]
        public decimal Total { get; set; }

        // 1 = Pendiente, 2 = Pagada, 3 = Cancelada
        public byte Estado { get; set; } = 1;

        public virtual ICollection<OrdenDetalle> Detalles { get; set; } = new List<OrdenDetalle>();
    }
}
