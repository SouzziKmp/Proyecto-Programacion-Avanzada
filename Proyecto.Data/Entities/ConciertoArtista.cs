using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Data.Entities
{
    // Relacion M:N Concierto-Artista (clave compuesta)
    public class ConciertoArtista
    {
        [ForeignKey(nameof(Concierto))]
        public int ConciertoId { get; set; }
        public virtual Concierto Concierto { get; set; }

        [ForeignKey(nameof(Artista))]
        public int ArtistaId { get; set; }
        public virtual Artista Artista { get; set; }

        public bool EsPrincipal { get; set; }
    }
}
