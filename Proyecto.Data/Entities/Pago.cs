using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Data.Entities
{
    public class Pago
    {
        public int PagoId { get; set; }

        // 1:1 con Orden (indice unico configurado en el DbContext).
        [ForeignKey(nameof(Orden))]
        public int OrdenId { get; set; }
        public virtual Orden Orden { get; set; }

        [Required, MaxLength(40)]
        public string Metodo { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Monto { get; set; }

        // 1 = Aprobado, 2 = Rechazado, 3 = Reembolsado
        public byte Estado { get; set; } = 1;

        [Column(TypeName = "datetime2")]
        public DateTime FechaPago { get; set; } = DateTime.Now;
    }
}
