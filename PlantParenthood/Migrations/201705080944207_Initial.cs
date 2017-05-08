namespace PlantParenthood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SensorDatas", "LightSumDay", c => c.Single(nullable: false));
            AddColumn("dbo.SensorDatas", "SoilMoistureCondition", c => c.Int(nullable: false));
            AddColumn("dbo.SensorDatas", "LightCondition", c => c.Int(nullable: false));
            AddColumn("dbo.SensorDatas", "TemperatureCondition", c => c.Int(nullable: false));
            AddColumn("dbo.SensorDatas", "HumidityCondition", c => c.Int(nullable: false));
            AddColumn("dbo.SensorDatas", "LightSumDayCondition", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SensorDatas", "LightSumDayCondition");
            DropColumn("dbo.SensorDatas", "HumidityCondition");
            DropColumn("dbo.SensorDatas", "TemperatureCondition");
            DropColumn("dbo.SensorDatas", "LightCondition");
            DropColumn("dbo.SensorDatas", "SoilMoistureCondition");
            DropColumn("dbo.SensorDatas", "LightSumDay");
        }
    }
}
