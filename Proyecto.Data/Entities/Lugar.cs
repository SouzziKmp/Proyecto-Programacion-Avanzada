using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Data.Entities
{
    public class Lugar
    {
        public int LugarId { get; set; }

        [Required, MaxLength(120)]
        public string Nombre { get; set; }

        [Required, MaxLength(200)]
        public string Direccion { get; set; }

        [Required, MaxLength(80)]
        public string Ciudad { get; set; }

        [MaxLength(80)]
        public string Provincia { get; set; }

        // CHECK > 0 aplicado en la migracion (Data.SqlClient no soporta CHECK via Data Annotations/Fluent API en EF6)
        public int AforoMaximo { get; set; }

        public virtual ICollection<Concierto> Conciertos { get; set; } = new List<Concierto>();
    }
}
