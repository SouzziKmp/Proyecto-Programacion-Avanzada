using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Data.Entities
{
    public class Concierto
    {
        public int ConciertoId { get; set; }

        [Required, MaxLength(150)]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime FechaEvento { get; set; }

        [ForeignKey(nameof(Lugar))]
        public int LugarId { get; set; }
        public virtual Lugar Lugar { get; set; }

        [ForeignKey(nameof(Categoria))]
        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }

        // 0 = Inactivo, 1 = Activo
        public byte Estado { get; set; } = 1;

        [Column(TypeName = "datetime2")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public virtual ICollection<ConciertoArtista> ConciertoArtistas { get; set; } = new List<ConciertoArtista>();
        public virtual ICollection<ConciertoImagen> Imagenes { get; set; } = new List<ConciertoImagen>();
        public virtual ICollection<TipoEntrada> TiposEntrada { get; set; } = new List<TipoEntrada>();
        public virtual ICollection<Resena> Resenas { get; set; } = new List<Resena>();
    }
}
