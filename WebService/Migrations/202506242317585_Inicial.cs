namespace WebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvariantesHus",
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
                        Nombre = c.String(),
                        Clasificación = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Pesos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RedNeuronalID = c.Int(nullable: false),
                        Peso = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RedesNeuronales", t => t.RedNeuronalID, cascadeDelete: true)
                .Index(t => t.RedNeuronalID);
            
            CreateTable(
                "dbo.RedesNeuronales",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProyectoID = c.Int(nullable: false),
                        Epocas = c.Int(nullable: false),
                        Arquitectura = c.String(),
                        Alfa = c.Double(nullable: false),
                        ErrorMinimo = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Proyectos", t => t.ProyectoID, cascadeDelete: true)
                .Index(t => t.ProyectoID);
            
            CreateTable(
                "dbo.Proyectos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UsuariosID = c.Int(nullable: false),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Usuarios", t => t.UsuariosID, cascadeDelete: true)
                .Index(t => t.UsuariosID);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Correo = c.String(),
                        Contraseña = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Umbrales",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RedNeuronalID = c.Int(nullable: false),
                        Umbral = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RedesNeuronales", t => t.RedNeuronalID, cascadeDelete: true)
                .Index(t => t.RedNeuronalID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Umbrales", "RedNeuronalID", "dbo.RedesNeuronales");
            DropForeignKey("dbo.Pesos", "RedNeuronalID", "dbo.RedesNeuronales");
            DropForeignKey("dbo.RedesNeuronales", "ProyectoID", "dbo.Proyectos");
            DropForeignKey("dbo.Proyectos", "UsuariosID", "dbo.Usuarios");
            DropForeignKey("dbo.InvariantesHus", "ObjetoID", "dbo.Objetos");
            DropIndex("dbo.Umbrales", new[] { "RedNeuronalID" });
            DropIndex("dbo.Proyectos", new[] { "UsuariosID" });
            DropIndex("dbo.RedesNeuronales", new[] { "ProyectoID" });
            DropIndex("dbo.Pesos", new[] { "RedNeuronalID" });
            DropIndex("dbo.InvariantesHus", new[] { "ObjetoID" });
            DropTable("dbo.Umbrales");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Proyectos");
            DropTable("dbo.RedesNeuronales");
            DropTable("dbo.Pesos");
            DropTable("dbo.Objetos");
            DropTable("dbo.InvariantesHus");
        }
    }
}
