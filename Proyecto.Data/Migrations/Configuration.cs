using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Proyecto.Data.Entities;

namespace Proyecto.Data.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            SeedRolesYUsuarios(context, out string adminId, out string socioId);
            SeedCatalogo(context, socioId);
        }

        private static void SeedRolesYUsuarios(ApplicationDbContext context, out string adminId, out string socioId)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            foreach (var nombreRol in new[] { "Administrador", "Asociado" })
            {
                if (!roleManager.RoleExists(nombreRol))
                    roleManager.Create(new IdentityRole(nombreRol));
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var admin = userManager.FindByEmail("admin@proyectofinal.com");
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin@proyectofinal.com",
                    Email = "admin@proyectofinal.com",
                    EmailConfirmed = true,
                    Nombre = "Administrador",
                    Apellidos = "Del Sistema",
                    Cedula = "1-1111-1111",
                    Telefono = "8888-0000",
                    FechaRegistro = DateTime.Now,
                    Activo = true
                };
                // Contrasena cifrada por ASP.NET Identity (RNF-03), no se guarda en texto plano.
                userManager.Create(admin, "Admin#2026");
            }
            if (!userManager.IsInRole(admin.Id, "Administrador"))
                userManager.AddToRole(admin.Id, "Administrador");

            var socio = userManager.FindByEmail("socio@proyectofinal.com");
            if (socio == null)
            {
                socio = new ApplicationUser
                {
                    UserName = "socio@proyectofinal.com",
                    Email = "socio@proyectofinal.com",
                    EmailConfirmed = true,
                    Nombre = "Socio",
                    Apellidos = "De Prueba",
                    Cedula = "2-2222-2222",
                    Telefono = "8888-1111",
                    FechaRegistro = DateTime.Now,
                    Activo = true
                };
                userManager.Create(socio, "Socio#2026");
            }
            if (!userManager.IsInRole(socio.Id, "Asociado"))
                userManager.AddToRole(socio.Id, "Asociado");

            adminId = admin.Id;
            socioId = socio.Id;
        }

        private static void SeedCatalogo(ApplicationDbContext context, string socioId)
        {
            // ---- Categorias ----
            var categorias = new[]
            {
                new Categoria { Nombre = "Concierto", Descripcion = "Presentaciones musicales en vivo" },
                new Categoria { Nombre = "Taller", Descripcion = "Talleres practicos" },
                new Categoria { Nombre = "Charla", Descripcion = "Charlas y conferencias" },
                new Categoria { Nombre = "Obra de Teatro", Descripcion = "Presentaciones teatrales" }
            };
            foreach (var c in categorias)
                context.Categorias.AddOrUpdate(x => x.Nombre, c);
            context.SaveChanges();

            var catConcierto = context.Categorias.Single(c => c.Nombre == "Concierto");

            // ---- Artistas ----
            var artistas = new[]
            {
                new Artista { Nombre = "Imagine Dragons", Pais = "Estados Unidos", Biografia = "Banda de rock alternativo formada en Las Vegas." },
                new Artista { Nombre = "Debi Nova", Pais = "Costa Rica", Biografia = "Cantautora costarricense de pop." },
                new Artista { Nombre = "Rawayana", Pais = "Venezuela", Biografia = "Banda de reggae y funk venezolana." }
            };
            foreach (var a in artistas)
                context.Artistas.AddOrUpdate(x => x.Nombre, a);
            context.SaveChanges();

            // ---- Lugares ----
            var lugares = new[]
            {
                new Lugar { Nombre = "Estadio Nacional de Costa Rica", Direccion = "La Sabana", Ciudad = "San Jose", Provincia = "San Jose", AforoMaximo = 35000 },
                new Lugar { Nombre = "Anfiteatro Coca-Cola", Direccion = "Barrio Mexico", Ciudad = "San Jose", Provincia = "San Jose", AforoMaximo = 8000 },
                new Lugar { Nombre = "Parque Viva", Direccion = "Guacima", Ciudad = "Alajuela", Provincia = "Alajuela", AforoMaximo = 15000 }
            };
            foreach (var l in lugares)
                context.Lugares.AddOrUpdate(x => x.Nombre, l);
            context.SaveChanges();

            var estadioNacional = context.Lugares.Single(l => l.Nombre == "Estadio Nacional de Costa Rica");
            var anfiteatro = context.Lugares.Single(l => l.Nombre == "Anfiteatro Coca-Cola");
            var parqueViva = context.Lugares.Single(l => l.Nombre == "Parque Viva");
            var imagineDragons = context.Artistas.Single(a => a.Nombre == "Imagine Dragons");
            var debiNova = context.Artistas.Single(a => a.Nombre == "Debi Nova");
            var rawayana = context.Artistas.Single(a => a.Nombre == "Rawayana");

            // ---- Conciertos ----
            var concierto1 = SeedConcierto(context, "Imagine Dragons - Mercury World Tour",
                "Gira mundial de Imagine Dragons llega por primera vez a Costa Rica.",
                DateTime.Today.AddMonths(2).AddHours(20), estadioNacional, catConcierto, imagineDragons);

            var concierto2 = SeedConcierto(context, "Debi Nova en Concierto",
                "Concierto intimo de Debi Nova presentando su nuevo material.",
                DateTime.Today.AddMonths(1).AddHours(19), anfiteatro, catConcierto, debiNova);

            var concierto3 = SeedConcierto(context, "Rawayana Live",
                "Rawayana en vivo con todos sus exitos.",
                DateTime.Today.AddMonths(3).AddHours(18), parqueViva, catConcierto, rawayana);

            context.SaveChanges();

            // ---- Tipos de entrada (aforo y control de sobreventa, RF-06/RNF-06) ----
            SeedTiposEntrada(context, concierto1, ("General", 45000m, 20000), ("VIP", 95000m, 2000));
            SeedTiposEntrada(context, concierto2, ("General", 25000m, 5000), ("VIP", 55000m, 500));
            SeedTiposEntrada(context, concierto3, ("General", 18000m, 8000), ("Platea", 35000m, 1500));
            context.SaveChanges();

            // ---- Imagenes (al menos 3 por evento, almacenadas como BLOB en la BD) ----
            SeedImagenesPlaceholder(context, concierto1);
            SeedImagenesPlaceholder(context, concierto2);
            SeedImagenesPlaceholder(context, concierto3);
            context.SaveChanges();

            // ---- Resena de ejemplo (una aprobada, una pendiente de moderar) ----
            if (!context.Resenas.Any(r => r.ConciertoId == concierto1.ConciertoId && r.UsuarioId == socioId))
            {
                context.Resenas.Add(new Resena
                {
                    ConciertoId = concierto1.ConciertoId,
                    UsuarioId = socioId,
                    Calificacion = 5,
                    Comentario = "Excelente concierto, muy buena produccion.",
                    Estado = 1, // Aprobada
                    FechaCreacion = DateTime.Now.AddDays(-2),
                    FechaModeracion = DateTime.Now.AddDays(-1)
                });
            }
            if (!context.Resenas.Any(r => r.ConciertoId == concierto2.ConciertoId && r.UsuarioId == socioId))
            {
                context.Resenas.Add(new Resena
                {
                    ConciertoId = concierto2.ConciertoId,
                    UsuarioId = socioId,
                    Calificacion = 4,
                    Comentario = "Muy buen ambiente, pendiente de moderacion.",
                    Estado = 0, // Pendiente
                    FechaCreacion = DateTime.Now
                });
            }
            context.SaveChanges();
        }

        private static Concierto SeedConcierto(ApplicationDbContext context, string titulo, string descripcion,
            DateTime fechaEvento, Lugar lugar, Categoria categoria, Artista artistaPrincipal)
        {
            var concierto = context.Conciertos.FirstOrDefault(c => c.Titulo == titulo);
            if (concierto == null)
            {
                concierto = new Concierto
                {
                    Titulo = titulo,
                    Descripcion = descripcion,
                    FechaEvento = fechaEvento,
                    LugarId = lugar.LugarId,
                    CategoriaId = categoria.CategoriaId,
                    Estado = 1
                };
                context.Conciertos.Add(concierto);
                context.SaveChanges();
            }

            if (!context.ConciertoArtistas.Any(ca => ca.ConciertoId == concierto.ConciertoId && ca.ArtistaId == artistaPrincipal.ArtistaId))
            {
                context.ConciertoArtistas.Add(new ConciertoArtista
                {
                    ConciertoId = concierto.ConciertoId,
                    ArtistaId = artistaPrincipal.ArtistaId,
                    EsPrincipal = true
                });
            }

            return concierto;
        }

        private static void SeedTiposEntrada(ApplicationDbContext context, Concierto concierto,
            params (string Nombre, decimal Precio, int Aforo)[] tipos)
        {
            foreach (var tipo in tipos)
            {
                if (context.TiposEntrada.Any(t => t.ConciertoId == concierto.ConciertoId && t.Nombre == tipo.Nombre))
                    continue;

                context.TiposEntrada.Add(new TipoEntrada
                {
                    ConciertoId = concierto.ConciertoId,
                    Nombre = tipo.Nombre,
                    Precio = tipo.Precio,
                    Aforo = tipo.Aforo,
                    Disponibles = tipo.Aforo
                });
            }
        }

        // Imagen PNG minima (1x1 px) usada como contenido de muestra; se reemplaza con
        // archivos reales cuando el CRUD de eventos con carga de imagenes este implementado.
        private static readonly byte[] ImagenPlaceholder = Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=");

        private static void SeedImagenesPlaceholder(ApplicationDbContext context, Concierto concierto)
        {
            if (context.ConciertoImagenes.Any(i => i.ConciertoId == concierto.ConciertoId))
                return;

            for (int i = 1; i <= 3; i++)
            {
                context.ConciertoImagenes.Add(new ConciertoImagen
                {
                    ConciertoId = concierto.ConciertoId,
                    NombreArchivo = $"placeholder-{concierto.ConciertoId}-{i}.png",
                    TipoContenido = "image/png",
                    Contenido = ImagenPlaceholder,
                    EsPrincipal = i == 1,
                    Orden = i
                });
            }
        }
    }
}
