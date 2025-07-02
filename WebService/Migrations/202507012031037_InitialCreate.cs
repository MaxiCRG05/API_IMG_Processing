namespace WebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.InvariantesHu",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ObjetoID = c.Int(nullable: false),
                        Hu1 = c.Double(nullable: false),
                        Hu2 = c.Double(nullable: false),
                        Hu3 = c.Double(nullable: false),
                        Hu4 = c.Double(nullable: false),
                        Hu5 = c.Double(nullable: false),
                        Hu6 = c.Double(nullable: false),
                        Hu7 = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Objetos", t => t.ObjetoID, cascadeDelete: true)
                .Index(t => t.ObjetoID);
            
            CreateTable(
                "dbo.Objetos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoriasID = c.Int(nullable: false),
                        Nombre = c.String(),
                        Imagen = c.Binary(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categorias", t => t.CategoriasID, cascadeDelete: true)
                .Index(t => t.CategoriasID);
            
            CreateTable(
                "dbo.Pesos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RedNeuronalID = c.Int(nullable: false),
                        Pesos = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Redes_Neuronales", t => t.RedNeuronalID, cascadeDelete: true)
                .Index(t => t.RedNeuronalID);
            
            CreateTable(
                "dbo.Redes_Neuronales",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProyectoID = c.Int(nullable: false),
                        Epocas = c.Int(nullable: false),
                        Arquitectura = c.String(),
                        Alfa = c.Double(nullable: false),
                        ErrorMin = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Proyectos", t => t.ProyectoID, cascadeDelete: true)
                .Index(t => t.ProyectoID);
            
            CreateTable(
                "dbo.Proyectos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UsuarioID = c.Int(nullable: false),
                        Nombre = c.String(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioID, cascadeDelete: true)
                .Index(t => t.UsuarioID);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Correo = c.String(nullable: false),
                        Contraseña = c.String(nullable: false),
                        Rol = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Proyectos_Objetos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProyectoID = c.Int(nullable: false),
                        ObjetoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Objetos", t => t.ObjetoID, cascadeDelete: true)
                .ForeignKey("dbo.Proyectos", t => t.ProyectoID, cascadeDelete: true)
                .Index(t => t.ProyectoID)
                .Index(t => t.ObjetoID);
            
            CreateTable(
                "dbo.Umbrales",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RedNeuronalID = c.Int(nullable: false),
                        Umbrales = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Redes_Neuronales", t => t.RedNeuronalID, cascadeDelete: true)
                .Index(t => t.RedNeuronalID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Umbrales", "RedNeuronalID", "dbo.Redes_Neuronales");
            DropForeignKey("dbo.Proyectos_Objetos", "ProyectoID", "dbo.Proyectos");
            DropForeignKey("dbo.Proyectos_Objetos", "ObjetoID", "dbo.Objetos");
            DropForeignKey("dbo.Pesos", "RedNeuronalID", "dbo.Redes_Neuronales");
            DropForeignKey("dbo.Redes_Neuronales", "ProyectoID", "dbo.Proyectos");
            DropForeignKey("dbo.Proyectos", "UsuarioID", "dbo.Usuarios");
            DropForeignKey("dbo.InvariantesHu", "ObjetoID", "dbo.Objetos");
            DropForeignKey("dbo.Objetos", "CategoriasID", "dbo.Categorias");
            DropIndex("dbo.Umbrales", new[] { "RedNeuronalID" });
            DropIndex("dbo.Proyectos_Objetos", new[] { "ObjetoID" });
            DropIndex("dbo.Proyectos_Objetos", new[] { "ProyectoID" });
            DropIndex("dbo.Proyectos", new[] { "UsuarioID" });
            DropIndex("dbo.Redes_Neuronales", new[] { "ProyectoID" });
            DropIndex("dbo.Pesos", new[] { "RedNeuronalID" });
            DropIndex("dbo.Objetos", new[] { "CategoriasID" });
            DropIndex("dbo.InvariantesHu", new[] { "ObjetoID" });
            DropTable("dbo.Umbrales");
            DropTable("dbo.Proyectos_Objetos");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Proyectos");
            DropTable("dbo.Redes_Neuronales");
            DropTable("dbo.Pesos");
            DropTable("dbo.Objetos");
            DropTable("dbo.InvariantesHu");
            DropTable("dbo.Categorias");
        }
    }
}
