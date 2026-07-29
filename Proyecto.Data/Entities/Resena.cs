using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Data.Entities
{
    public class Resena
    {
        public int ResenaId { get; set; }

        [ForeignKey(nameof(Concierto))]
        public int ConciertoId { get; set; }
        public virtual Concierto Concierto { get; set; }

        [Required, ForeignKey(nameof(Usuario))]
        public string UsuarioId { get; set; }
        public virtual ApplicationUser Usuario { get; set; }

        [Range(1, 5)]
        public byte Calificacion { get; set; }

        [MaxLength(1000)]
        public string Comentario { get; set; }

        // 0 = Pendiente, 1 = Aprobada, 2 = Rechazada
        public byte Estado { get; set; } = 0;

        [Column(TypeName = "datetime2")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime2")]
        public DateTime? FechaModeracion { get; set; }

        [ForeignKey(nameof(ModeradoPor))]
        public string ModeradoPorId { get; set; }
        public virtual ApplicationUser ModeradoPor { get; set; }
    }
}
