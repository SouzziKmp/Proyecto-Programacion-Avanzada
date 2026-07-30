using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Data.Entities
{
    public class Categoria
    {
        public int CategoriaId { get; set; }

        [Required, MaxLength(60)]
        public string Nombre { get; set; }

        [MaxLength(250)]
        public string Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        public virtual ICollection<Concierto> Conciertos { get; set; } = new List<Concierto>();
    }
}
