using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Data.Entities
{
    public class Carrito
    {
        public int CarritoId { get; set; }

        [Required, ForeignKey(nameof(Usuario))]
        public string UsuarioId { get; set; }
        public virtual ApplicationUser Usuario { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // 1 = Activo, 2 = Convertido, 3 = Abandonado
        public byte Estado { get; set; } = 1;

        public virtual ICollection<CarritoDetalle> Detalles { get; set; } = new List<CarritoDetalle>();
    }
}
