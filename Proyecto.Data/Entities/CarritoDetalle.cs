using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Data.Entities
{
    public class CarritoDetalle
    {
        public int CarritoDetalleId { get; set; }

        [ForeignKey(nameof(Carrito))]
        public int CarritoId { get; set; }
        public virtual Carrito Carrito { get; set; }

        [ForeignKey(nameof(TipoEntrada))]
        public int TipoEntradaId { get; set; }
        public virtual TipoEntrada TipoEntrada { get; set; }

        public int Cantidad { get; set; }

        // Snapshot del precio al momento de agregar al carrito.
        [Column(TypeName = "decimal")]
        public decimal PrecioUnitario { get; set; }
    }
}
