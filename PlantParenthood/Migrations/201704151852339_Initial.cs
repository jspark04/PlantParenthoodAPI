namespace PlantParenthood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareDatas", "Owned", c => c.Boolean(nullable: false));
            AddColumn("dbo.CareDatas", "Current", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CareDatas", "Current");
            DropColumn("dbo.CareDatas", "Owned");
        }
    }
}
