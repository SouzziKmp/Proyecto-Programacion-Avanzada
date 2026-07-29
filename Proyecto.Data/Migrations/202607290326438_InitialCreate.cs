namespace Proyecto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artista",
                c => new
                    {
                        ArtistaId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 120),
                        Pais = c.String(maxLength: 60),
                        Biografia = c.String(),
                    })
                .PrimaryKey(t => t.ArtistaId);
            
            CreateTable(
                "dbo.ConciertoArtista",
                c => new
                    {
                        ConciertoId = c.Int(nullable: false),
                        ArtistaId = c.Int(nullable: false),
                        EsPrincipal = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ConciertoId, t.ArtistaId })
                .ForeignKey("dbo.Artista", t => t.ArtistaId, cascadeDelete: true)
                .ForeignKey("dbo.Concierto", t => t.ConciertoId, cascadeDelete: true)
                .Index(t => t.ConciertoId)
                .Index(t => t.ArtistaId);
            
            CreateTable(
                "dbo.Concierto",
                c => new
                    {
                        ConciertoId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 150),
                        Descripcion = c.String(),
                        FechaEvento = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LugarId = c.Int(nullable: false),
                        CategoriaId = c.Int(nullable: false),
                        Estado = c.Byte(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ConciertoId)
                .ForeignKey("dbo.Categoria", t => t.CategoriaId)
                .ForeignKey("dbo.Lugar", t => t.LugarId)
                .Index(t => t.LugarId)
                .Index(t => t.CategoriaId);
            
            CreateTable(
                "dbo.Categoria",
                c => new
                    {
                        CategoriaId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 60),
                        Descripcion = c.String(maxLength: 250),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoriaId)
                .Index(t => t.Nombre, unique: true, name: "IX_Categoria_Nombre");
            
            CreateTable(
                "dbo.ConciertoImagen",
                c => new
                    {
                        ImagenId = c.Int(nullable: false, identity: true),
                        ConciertoId = c.Int(nullable: false),
                        NombreArchivo = c.String(nullable: false, maxLength: 255),
                        TipoContenido = c.String(nullable: false, maxLength: 100),
                        Contenido = c.Binary(nullable: false),
                        EsPrincipal = c.Boolean(nullable: false),
                        Orden = c.Int(nullable: false),
                        FechaCarga = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ImagenId)
                .ForeignKey("dbo.Concierto", t => t.ConciertoId, cascadeDelete: true)
                .Index(t => t.ConciertoId);
            
            CreateTable(
                "dbo.Lugar",
                c => new
                    {
                        LugarId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 120),
                        Direccion = c.String(nullable: false, maxLength: 200),
                        Ciudad = c.String(nullable: false, maxLength: 80),
                        Provincia = c.String(maxLength: 80),
                        AforoMaximo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LugarId);
            
            CreateTable(
                "dbo.Resena",
                c => new
                    {
                        ResenaId = c.Int(nullable: false, identity: true),
                        ConciertoId = c.Int(nullable: false),
                        UsuarioId = c.String(nullable: false, maxLength: 128),
                        Calificacion = c.Byte(nullable: false),
                        Comentario = c.String(maxLength: 1000),
                        Estado = c.Byte(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FechaModeracion = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModeradoPorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ResenaId)
                .ForeignKey("dbo.Concierto", t => t.ConciertoId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ModeradoPorId)
                .ForeignKey("dbo.AspNetUsers", t => t.UsuarioId)
                .Index(t => t.ConciertoId)
                .Index(t => t.UsuarioId)
                .Index(t => t.ModeradoPorId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(nullable: false, maxLength: 80),
                        Apellidos = c.String(nullable: false, maxLength: 120),
                        Cedula = c.String(maxLength: 20),
                        Telefono = c.String(maxLength: 20),
                        FechaRegistro = c.DateTime(nullable: false),
                        UltimoLogin = c.DateTime(),
                        Activo = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true, name: "IX_AspNetUsers_Email")
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.Carrito",
                c => new
                    {
                        CarritoId = c.Int(nullable: false, identity: true),
                        UsuarioId = c.String(nullable: false, maxLength: 128),
                        FechaCreacion = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Estado = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.CarritoId)
                .ForeignKey("dbo.AspNetUsers", t => t.UsuarioId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.CarritoDetalle",
                c => new
                    {
                        CarritoDetalleId = c.Int(nullable: false, identity: true),
                        CarritoId = c.Int(nullable: false),
                        TipoEntradaId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 10, scale: 2),
                    })
                .PrimaryKey(t => t.CarritoDetalleId)
                .ForeignKey("dbo.Carrito", t => t.CarritoId, cascadeDelete: true)
                .ForeignKey("dbo.TipoEntrada", t => t.TipoEntradaId)
                .Index(t => t.CarritoId)
                .Index(t => t.TipoEntradaId);
            
            CreateTable(
                "dbo.TipoEntrada",
                c => new
                    {
                        TipoEntradaId = c.Int(nullable: false, identity: true),
                        ConciertoId = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 60),
                        Precio = c.Decimal(nullable: false, precision: 10, scale: 2),
                        Aforo = c.Int(nullable: false),
                        Disponibles = c.Int(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.TipoEntradaId)
                .ForeignKey("dbo.Concierto", t => t.ConciertoId, cascadeDelete: true)
                .Index(t => t.ConciertoId);
            
            CreateTable(
                "dbo.OrdenDetalle",
                c => new
                    {
                        OrdenDetalleId = c.Int(nullable: false, identity: true),
                        OrdenId = c.Int(nullable: false),
                        TipoEntradaId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 10, scale: 2),
                    })
                .PrimaryKey(t => t.OrdenDetalleId)
                .ForeignKey("dbo.Orden", t => t.OrdenId, cascadeDelete: true)
                .ForeignKey("dbo.TipoEntrada", t => t.TipoEntradaId)
                .Index(t => t.OrdenId)
                .Index(t => t.TipoEntradaId);

            // Columna calculada persistida (evita redundancia, ver diccionario de datos Avance 1).
            // EF solo la lee (DatabaseGeneratedOption.Computed en la entidad OrdenDetalle).
            Sql("ALTER TABLE dbo.OrdenDetalle ADD Subtotal AS (Cantidad * PrecioUnitario) PERSISTED");
            
            CreateTable(
                "dbo.Orden",
                c => new
                    {
                        OrdenId = c.Int(nullable: false, identity: true),
                        UsuarioId = c.String(nullable: false, maxLength: 128),
                        FechaOrden = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Total = c.Decimal(nullable: false, precision: 10, scale: 2),
                        Estado = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.OrdenId)
                .ForeignKey("dbo.AspNetUsers", t => t.UsuarioId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Ticket",
                c => new
                    {
                        TicketId = c.Int(nullable: false, identity: true),
                        OrdenDetalleId = c.Int(nullable: false),
                        CodigoUnico = c.Guid(nullable: false),
                        Estado = c.Byte(nullable: false),
                        FechaEmision = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.TicketId)
                .ForeignKey("dbo.OrdenDetalle", t => t.OrdenDetalleId, cascadeDelete: true)
                .Index(t => t.OrdenDetalleId)
                .Index(t => t.CodigoUnico, unique: true, name: "IX_Ticket_CodigoUnico");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Pago",
                c => new
                    {
                        PagoId = c.Int(nullable: false, identity: true),
                        OrdenId = c.Int(nullable: false),
                        Metodo = c.String(nullable: false, maxLength: 40),
                        Monto = c.Decimal(nullable: false, precision: 10, scale: 2),
                        Estado = c.Byte(nullable: false),
                        FechaPago = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.PagoId)
                .ForeignKey("dbo.Orden", t => t.OrdenId, cascadeDelete: true)
                .Index(t => t.OrdenId, unique: true, name: "IX_Pago_OrdenId");
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            // Restricciones CHECK del diccionario de datos (Avance 1) que EF6 Code First no
            // puede declarar via Data Annotations/Fluent API; se agregan como SQL nativo.
            Sql("ALTER TABLE dbo.Lugar ADD CONSTRAINT CK_Lugar_AforoMaximo CHECK (AforoMaximo > 0)");
            Sql("ALTER TABLE dbo.TipoEntrada ADD CONSTRAINT CK_TipoEntrada_Precio CHECK (Precio >= 0)");
            Sql("ALTER TABLE dbo.TipoEntrada ADD CONSTRAINT CK_TipoEntrada_Aforo CHECK (Aforo > 0)");
            Sql("ALTER TABLE dbo.TipoEntrada ADD CONSTRAINT CK_TipoEntrada_Disponibles CHECK (Disponibles >= 0 AND Disponibles <= Aforo)");
            Sql("ALTER TABLE dbo.CarritoDetalle ADD CONSTRAINT CK_CarritoDetalle_Cantidad CHECK (Cantidad > 0)");
            Sql("ALTER TABLE dbo.Orden ADD CONSTRAINT CK_Orden_Total CHECK (Total >= 0)");
            Sql("ALTER TABLE dbo.OrdenDetalle ADD CONSTRAINT CK_OrdenDetalle_Cantidad CHECK (Cantidad > 0)");
            Sql("ALTER TABLE dbo.Resena ADD CONSTRAINT CK_Resena_Calificacion CHECK (Calificacion BETWEEN 1 AND 5)");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Pago", "OrdenId", "dbo.Orden");
            DropForeignKey("dbo.ConciertoArtista", "ConciertoId", "dbo.Concierto");
            DropForeignKey("dbo.Resena", "UsuarioId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Resena", "ModeradoPorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Carrito", "UsuarioId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CarritoDetalle", "TipoEntradaId", "dbo.TipoEntrada");
            DropForeignKey("dbo.OrdenDetalle", "TipoEntradaId", "dbo.TipoEntrada");
            DropForeignKey("dbo.Ticket", "OrdenDetalleId", "dbo.OrdenDetalle");
            DropForeignKey("dbo.OrdenDetalle", "OrdenId", "dbo.Orden");
            DropForeignKey("dbo.Orden", "UsuarioId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TipoEntrada", "ConciertoId", "dbo.Concierto");
            DropForeignKey("dbo.CarritoDetalle", "CarritoId", "dbo.Carrito");
            DropForeignKey("dbo.Resena", "ConciertoId", "dbo.Concierto");
            DropForeignKey("dbo.Concierto", "LugarId", "dbo.Lugar");
            DropForeignKey("dbo.ConciertoImagen", "ConciertoId", "dbo.Concierto");
            DropForeignKey("dbo.Concierto", "CategoriaId", "dbo.Categoria");
            DropForeignKey("dbo.ConciertoArtista", "ArtistaId", "dbo.Artista");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Pago", "IX_Pago_OrdenId");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Ticket", "IX_Ticket_CodigoUnico");
            DropIndex("dbo.Ticket", new[] { "OrdenDetalleId" });
            DropIndex("dbo.Orden", new[] { "UsuarioId" });
            DropIndex("dbo.OrdenDetalle", new[] { "TipoEntradaId" });
            DropIndex("dbo.OrdenDetalle", new[] { "OrdenId" });
            DropIndex("dbo.TipoEntrada", new[] { "ConciertoId" });
            DropIndex("dbo.CarritoDetalle", new[] { "TipoEntradaId" });
            DropIndex("dbo.CarritoDetalle", new[] { "CarritoId" });
            DropIndex("dbo.Carrito", new[] { "UsuarioId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", "IX_AspNetUsers_Email");
            DropIndex("dbo.Resena", new[] { "ModeradoPorId" });
            DropIndex("dbo.Resena", new[] { "UsuarioId" });
            DropIndex("dbo.Resena", new[] { "ConciertoId" });
            DropIndex("dbo.ConciertoImagen", new[] { "ConciertoId" });
            DropIndex("dbo.Categoria", "IX_Categoria_Nombre");
            DropIndex("dbo.Concierto", new[] { "CategoriaId" });
            DropIndex("dbo.Concierto", new[] { "LugarId" });
            DropIndex("dbo.ConciertoArtista", new[] { "ArtistaId" });
            DropIndex("dbo.ConciertoArtista", new[] { "ConciertoId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Pago");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Ticket");
            DropTable("dbo.Orden");
            DropTable("dbo.OrdenDetalle");
            DropTable("dbo.TipoEntrada");
            DropTable("dbo.CarritoDetalle");
            DropTable("dbo.Carrito");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Resena");
            DropTable("dbo.Lugar");
            DropTable("dbo.ConciertoImagen");
            DropTable("dbo.Categoria");
            DropTable("dbo.Concierto");
            DropTable("dbo.ConciertoArtista");
            DropTable("dbo.Artista");
        }
    }
}
