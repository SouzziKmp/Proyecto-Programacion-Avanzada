namespace Proyecto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarCodigos : DbMigration
    {
        public override void Up()
        {
            // Se agrega como nullable primero para poder rellenar las filas ya
            // existentes con un codigo unico antes de exigir NOT NULL.
            AddColumn("dbo.Concierto", "Codigo", c => c.String(maxLength: 20));
            AddColumn("dbo.AspNetUsers", "Codigo", c => c.String(maxLength: 20));

            Sql(@"
                ;WITH C AS (
                    SELECT ConciertoId, ROW_NUMBER() OVER (ORDER BY ConciertoId) AS rn
                    FROM dbo.Concierto
                )
                UPDATE co SET Codigo = 'EVT-' + RIGHT('000000' + CAST(C.rn AS VARCHAR(10)), 6)
                FROM dbo.Concierto co INNER JOIN C ON co.ConciertoId = C.ConciertoId
            ");

            Sql(@"
                ;WITH U AS (
                    SELECT Id, ROW_NUMBER() OVER (ORDER BY Id) AS rn
                    FROM dbo.AspNetUsers
                )
                UPDATE au SET Codigo = 'USR-' + RIGHT('000000' + CAST(U.rn AS VARCHAR(10)), 6)
                FROM dbo.AspNetUsers au INNER JOIN U ON au.Id = U.Id
            ");

            AlterColumn("dbo.Concierto", "Codigo", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.AspNetUsers", "Codigo", c => c.String(nullable: false, maxLength: 20));

            CreateIndex("dbo.Concierto", "Codigo", unique: true, name: "IX_Concierto_Codigo");
            CreateIndex("dbo.AspNetUsers", "Codigo", unique: true, name: "IX_AspNetUsers_Codigo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUsers", "IX_AspNetUsers_Codigo");
            DropIndex("dbo.Concierto", "IX_Concierto_Codigo");
            DropColumn("dbo.AspNetUsers", "Codigo");
            DropColumn("dbo.Concierto", "Codigo");
        }
    }
}
