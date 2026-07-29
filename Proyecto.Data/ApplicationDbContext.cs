using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Proyecto.Data.Entities;

namespace Proyecto.Data
{
    // Hereda AspNetUsers / AspNetRoles / AspNetUserRoles / AspNetUserClaims / AspNetUserLogins
    // de ASP.NET Identity (RNF-03, RNF-04). El resto de tablas del dominio se agregan aqui.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("name=ProyectoFinalDb")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Artista> Artistas { get; set; }
        public DbSet<Lugar> Lugares { get; set; }
        public DbSet<Concierto> Conciertos { get; set; }
        public DbSet<ConciertoArtista> ConciertoArtistas { get; set; }
        public DbSet<ConciertoImagen> ConciertoImagenes { get; set; }
        public DbSet<TipoEntrada> TiposEntrada { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoDetalle> CarritoDetalles { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<OrdenDetalle> OrdenDetalles { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Resena> Resenas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // ---- Nombres de tabla (coinciden con el diccionario de datos del Avance 1) ----
            modelBuilder.Entity<Categoria>().ToTable("Categoria");
            modelBuilder.Entity<Artista>().ToTable("Artista");
            modelBuilder.Entity<Lugar>().ToTable("Lugar");
            modelBuilder.Entity<Concierto>().ToTable("Concierto");
            modelBuilder.Entity<ConciertoArtista>().ToTable("ConciertoArtista");
            modelBuilder.Entity<ConciertoImagen>().ToTable("ConciertoImagen");
            modelBuilder.Entity<TipoEntrada>().ToTable("TipoEntrada");
            modelBuilder.Entity<Carrito>().ToTable("Carrito");
            modelBuilder.Entity<CarritoDetalle>().ToTable("CarritoDetalle");
            modelBuilder.Entity<Orden>().ToTable("Orden");
            modelBuilder.Entity<OrdenDetalle>().ToTable("OrdenDetalle");
            modelBuilder.Entity<Pago>().ToTable("Pago");
            modelBuilder.Entity<Ticket>().ToTable("Ticket");
            modelBuilder.Entity<Resena>().ToTable("Resena");

            // ---- Precision decimal(10,2) segun diccionario ----
            modelBuilder.Entity<TipoEntrada>().Property(t => t.Precio).HasPrecision(10, 2);
            modelBuilder.Entity<CarritoDetalle>().Property(c => c.PrecioUnitario).HasPrecision(10, 2);
            modelBuilder.Entity<Orden>().Property(o => o.Total).HasPrecision(10, 2);
            modelBuilder.Entity<OrdenDetalle>().Property(d => d.PrecioUnitario).HasPrecision(10, 2);
            modelBuilder.Entity<OrdenDetalle>().Property(d => d.Subtotal).HasPrecision(10, 2);
            modelBuilder.Entity<Pago>().Property(p => p.Monto).HasPrecision(10, 2);

            // ---- Clave compuesta ConciertoArtista (M:N) ----
            modelBuilder.Entity<ConciertoArtista>().HasKey(ca => new { ca.ConciertoId, ca.ArtistaId });

            // ---- Indices unicos ----
            modelBuilder.Entity<Categoria>().Property(c => c.Nombre)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Categoria_Nombre") { IsUnique = true }));

            modelBuilder.Entity<Ticket>().Property(t => t.CodigoUnico)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Ticket_CodigoUnico") { IsUnique = true }));

            modelBuilder.Entity<Pago>().Property(p => p.OrdenId)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Pago_OrdenId") { IsUnique = true }));

            modelBuilder.Entity<ApplicationUser>().Property(u => u.Email)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_AspNetUsers_Email") { IsUnique = true }));

            // ---- Relaciones y politica de borrado ----
            // Regla general: las tablas de catalogo (Concierto, TipoEntrada, AspNetUsers) NO
            // cascadean hacia tablas transaccionales/historicas (Orden*, Ticket, Pago, Resena)
            // para no perder historial de ventas; solo cascadean las relaciones de
            // "objeto dueño <-> detalle" (Carrito->CarritoDetalle, Orden->OrdenDetalle->Ticket,
            // Orden->Pago, Concierto->ConciertoImagen/ConciertoArtista).

            modelBuilder.Entity<Concierto>()
                .HasRequired(c => c.Lugar)
                .WithMany(l => l.Conciertos)
                .HasForeignKey(c => c.LugarId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Concierto>()
                .HasRequired(c => c.Categoria)
                .WithMany(cat => cat.Conciertos)
                .HasForeignKey(c => c.CategoriaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConciertoArtista>()
                .HasRequired(ca => ca.Concierto)
                .WithMany(c => c.ConciertoArtistas)
                .HasForeignKey(ca => ca.ConciertoId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ConciertoArtista>()
                .HasRequired(ca => ca.Artista)
                .WithMany(a => a.ConciertoArtistas)
                .HasForeignKey(ca => ca.ArtistaId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ConciertoImagen>()
                .HasRequired(i => i.Concierto)
                .WithMany(c => c.Imagenes)
                .HasForeignKey(i => i.ConciertoId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TipoEntrada>()
                .HasRequired(t => t.Concierto)
                .WithMany(c => c.TiposEntrada)
                .HasForeignKey(t => t.ConciertoId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Carrito>()
                .HasRequired(c => c.Usuario)
                .WithMany(u => u.Carritos)
                .HasForeignKey(c => c.UsuarioId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CarritoDetalle>()
                .HasRequired(d => d.Carrito)
                .WithMany(c => c.Detalles)
                .HasForeignKey(d => d.CarritoId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<CarritoDetalle>()
                .HasRequired(d => d.TipoEntrada)
                .WithMany(t => t.CarritoDetalles)
                .HasForeignKey(d => d.TipoEntradaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Orden>()
                .HasRequired(o => o.Usuario)
                .WithMany(u => u.Ordenes)
                .HasForeignKey(o => o.UsuarioId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrdenDetalle>()
                .HasRequired(d => d.Orden)
                .WithMany(o => o.Detalles)
                .HasForeignKey(d => d.OrdenId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<OrdenDetalle>()
                .HasRequired(d => d.TipoEntrada)
                .WithMany(t => t.OrdenDetalles)
                .HasForeignKey(d => d.TipoEntradaId)
                .WillCascadeOnDelete(false);

            // 1:1 logico Orden-Pago: Pago conserva su propia PK identity (PagoId) y una FK
            // separada (OrdenId) restringida por indice unico (ver arriba). No se usa el patron
            // de PK compartida de EF6 porque el diccionario del Avance 1 define ambas columnas.
            modelBuilder.Entity<Pago>()
                .HasRequired(p => p.Orden)
                .WithMany()
                .HasForeignKey(p => p.OrdenId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Ticket>()
                .HasRequired(t => t.OrdenDetalle)
                .WithMany(d => d.Tickets)
                .HasForeignKey(t => t.OrdenDetalleId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Resena>()
                .HasRequired(r => r.Concierto)
                .WithMany(c => c.Resenas)
                .HasForeignKey(r => r.ConciertoId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Resena>()
                .HasRequired(r => r.Usuario)
                .WithMany(u => u.Resenas)
                .HasForeignKey(r => r.UsuarioId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Resena>()
                .HasOptional(r => r.ModeradoPor)
                .WithMany()
                .HasForeignKey(r => r.ModeradoPorId)
                .WillCascadeOnDelete(false);
        }
    }
}
