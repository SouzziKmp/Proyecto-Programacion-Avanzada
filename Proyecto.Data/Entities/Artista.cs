using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Data.Entities
{
    public class Artista
    {
        public int ArtistaId { get; set; }

        [Required, MaxLength(120)]
        public string Nombre { get; set; }

        [MaxLength(60)]
        public string Pais { get; set; }

        public string Biografia { get; set; }

        public virtual ICollection<ConciertoArtista> ConciertoArtistas { get; set; } = new List<ConciertoArtista>();
    }
}
