using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Data.Entities
{
    public class TipoEntrada
    {
        public int TipoEntradaId { get; set; }

        [ForeignKey(nameof(Concierto))]
        public int ConciertoId { get; set; }
        public virtual Concierto Concierto { get; set; }

        [Required, MaxLength(60)]
        public string Nombre { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Precio { get; set; }

        public int Aforo { get; set; }

        // Cupo restante. Control de sobreventa (RNF-06): se decrementa dentro de una
        // transaccion al confirmar la compra, nunca al agregar al carrito.
        public int Disponibles { get; set; }

        // Concurrencia optimista (RNF-06): EF agrega este token a la clausula WHERE del UPDATE.
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<CarritoDetalle> CarritoDetalles { get; set; } = new List<CarritoDetalle>();
        public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();
    }
}
