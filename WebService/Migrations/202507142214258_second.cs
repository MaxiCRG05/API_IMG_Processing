namespace WebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "TokenRecuperacion", c => c.String());
            AddColumn("dbo.Usuarios", "ExpiracionToken", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "ExpiracionToken");
            DropColumn("dbo.Usuarios", "TokenRecuperacion");
        }
    }
}
