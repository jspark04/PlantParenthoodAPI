namespace PlantParenthood.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using PlantParenthood.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<PlantParenthood.Models.PlantParenthoodContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PlantParenthood.Models.PlantParenthoodContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // Adding seed data for plant care metrics
            context.CareDatas.AddOrUpdate(p => p.PlantName,
                    new CareData
                    {
                        PlantName = "Warm-season plant",
                        SoilMoisture = 60,
                        Light = 100,
                        Temperature = 70,
                        Humidity = 20,
                        Owned = true,
                        Current = true
                    },
                    new CareData
                    {
                        PlantName = "Cold-season plant",
                        SoilMoisture = 60,
                        Light = 100,
                        Temperature = 60,
                        Humidity = 20,
                        Owned = false,
                        Current = false
                    }
                    );
            
            // Adding seed data for sensors... remove when ready
            context.SensorDatas.AddOrUpdate(p => p.SoilMoisture,
                        new SensorData
                        {
                            CareInfoID = 1,
                            CreatedDate = DateTime.ParseExact("2017-04-01 17:00:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            SoilMoisture = 30,
                            Light = 60,
                            Temperature = 50,
                            Humidity = 10
                        },
                        new SensorData
                        {
                            CareInfoID = 1,
                            CreatedDate = DateTime.ParseExact("2017-04-01 17:00:10", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            SoilMoisture = 40,
                            Light = 60,
                            Temperature = 50,
                            Humidity = 13
                        },
                        new SensorData
                        {
                            CareInfoID = 1,
                            CreatedDate = DateTime.ParseExact("2017-04-01 17:00:20", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            SoilMoisture = 60,
                            Light = 65,
                            Temperature = 50,
                            Humidity = 15
                        }
                    );
        }
    }
}
