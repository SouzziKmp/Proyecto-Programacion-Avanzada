using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Proyecto.Data.Entities
{
    // Hereda Id, UserName, Email, PasswordHash, SecurityStamp, Roles, etc. de ASP.NET Identity (RNF-03).
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(80)]
        public string Nombre { get; set; }

        [Required, MaxLength(120)]
        public string Apellidos { get; set; }

        [MaxLength(20)]
        public string Cedula { get; set; }

        [MaxLength(20)]
        public string Telefono { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // "Última fecha y hora de conexión" (rubrica Proyecto Final 3.2)
        public DateTime? UltimoLogin { get; set; }

        public bool Activo { get; set; } = true;

        public virtual ICollection<Carrito> Carritos { get; set; }
            = new List<Carrito>();

        public virtual ICollection<Orden> Ordenes { get; set; }
            = new List<Orden>();

        public virtual ICollection<Resena> Resenas { get; set; }
            = new List<Resena>();

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager)
        {
            var userIdentity =
                await manager.CreateIdentityAsync(
                    this,
                    DefaultAuthenticationTypes.ApplicationCookie
                );

            return userIdentity;
        }
    }
}