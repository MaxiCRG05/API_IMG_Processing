namespace WebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProyectoCategorias",
                c => new
                    {
                        ProyectoID = c.Int(nullable: false),
                        CategoriaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProyectoID, t.CategoriaID })
                .ForeignKey("dbo.Proyectos", t => t.ProyectoID, cascadeDelete: true)
                .ForeignKey("dbo.Categorias", t => t.CategoriaID, cascadeDelete: true)
                .Index(t => t.ProyectoID)
                .Index(t => t.CategoriaID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProyectoCategorias", "CategoriaID", "dbo.Categorias");
            DropForeignKey("dbo.ProyectoCategorias", "ProyectoID", "dbo.Proyectos");
            DropIndex("dbo.ProyectoCategorias", new[] { "CategoriaID" });
            DropIndex("dbo.ProyectoCategorias", new[] { "ProyectoID" });
            DropTable("dbo.ProyectoCategorias");
        }
    }
}
