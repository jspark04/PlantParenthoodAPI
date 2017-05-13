namespace PlantParenthood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareDatas", "ImageURL", c => c.String());
            AlterColumn("dbo.AppSettings", "Value", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AppSettings", "Value", c => c.Boolean(nullable: false));
            DropColumn("dbo.CareDatas", "ImageURL");
        }
    }
}
