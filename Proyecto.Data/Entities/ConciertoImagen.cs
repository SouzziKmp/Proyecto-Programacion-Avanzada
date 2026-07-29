using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Data.Entities
{
    // Imagen almacenada como BLOB en la BD (Proyecto Final 3.1: "al menos 3 imagenes... almacenadas
  
    public class ConciertoImagen
    {
        [Key]
        public int ImagenId { get; set; }

        [ForeignKey(nameof(Concierto))]
        public int ConciertoId { get; set; }
        public virtual Concierto Concierto { get; set; }

        [Required, MaxLength(255)]
        public string NombreArchivo { get; set; }

        [Required, MaxLength(100)]
        public string TipoContenido { get; set; }

        [Required]
        public byte[] Contenido { get; set; }

        public bool EsPrincipal { get; set; }

        public int Orden { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime FechaCarga { get; set; } = DateTime.Now;
    }
}
