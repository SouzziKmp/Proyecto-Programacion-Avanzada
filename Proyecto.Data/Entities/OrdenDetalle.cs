using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Data.Entities
{
    public class OrdenDetalle
    {
        public int OrdenDetalleId { get; set; }

        [ForeignKey(nameof(Orden))]
        public int OrdenId { get; set; }
        public virtual Orden Orden { get; set; }

        [ForeignKey(nameof(TipoEntrada))]
        public int TipoEntradaId { get; set; }
        public virtual TipoEntrada TipoEntrada { get; set; }

        public int Cantidad { get; set; }

        // Precio historico al momento de la compra (no cambia si el precio del TipoEntrada cambia despues).
        [Column(TypeName = "decimal")]
        public decimal PrecioUnitario { get; set; }

        // Columna calculada en la BD: Cantidad * PrecioUnitario PERSISTED (evita redundancia).
        // Definida en la migracion via Sql(); EF solo la lee (DatabaseGeneratedOption.Computed).
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal")]
        public decimal Subtotal { get; set; }

        public virtual System.Collections.Generic.ICollection<Ticket> Tickets { get; set; } = new System.Collections.Generic.List<Ticket>();
    }
}
