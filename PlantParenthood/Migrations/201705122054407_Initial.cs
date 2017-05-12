namespace PlantParenthood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppSettings",
                c => new
                    {
                        AppSettingsID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AppSettingsID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AppSettings");
        }
    }
}
