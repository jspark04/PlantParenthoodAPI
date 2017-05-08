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
                        PlantName = "Rosemary",
                        SoilMoisture = 60,
                        Light = 800,
                        Temperature = 70,
                        Humidity = 20,
                        Owned = true,
                        Current = true
                    },
                    new CareData
                    {
                        PlantName = "Lavender",
                        SoilMoisture = 60,
                        Light = 800,
                        Temperature = 60,
                        Humidity = 20,
                        Owned = true,
                        Current = false
                    }
                    );
            
            // Adding seed data for sensors... remove when ready
            context.SensorDatas.AddOrUpdate(p => p.SoilMoisture,
                        new SensorData
                        {
                            CareInfoID = 1,
                            CreatedDate = DateTime.ParseExact("2017-05-07 09:00:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            SoilMoisture = 30,
                            Light = 20,
                            Temperature = 50,
                            Humidity = 10,
                            LightSumDay = 0,
                            SoilMoistureCondition = 1,
                            LightCondition = 1,
                            TemperatureCondition = 1,
                            HumidityCondition = 1,
                            LightSumDayCondition = 1
                        },
                        new SensorData
                        {
                            CareInfoID = 1,
                            CreatedDate = DateTime.ParseExact("2017-05-07 12:00:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            SoilMoisture = 40,
                            Light = 80,
                            Temperature = 50,
                            Humidity = 13,
                            LightSumDay = 0,
                            SoilMoistureCondition = 1,
                            LightCondition = 1,
                            TemperatureCondition = 1,
                            HumidityCondition = 1,
                            LightSumDayCondition = 1
                        },
                        new SensorData
                        {
                            CareInfoID = 1,
                            CreatedDate = DateTime.ParseExact("2017-05-07 18:00:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            SoilMoisture = 60,
                            Light = 40,
                            Temperature = 50,
                            Humidity = 15,
                            LightSumDay = 800,
                            SoilMoistureCondition = 1,
                            LightCondition = 1,
                            TemperatureCondition = 3,
                            HumidityCondition = 2,
                            LightSumDayCondition = 1
                        }
                    );
        }
    }
}
