using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PlantParenthood.Models
{
    public class SensorData
    {
        [Key]
        public int SensorDataID { get; set; }

        public int CareInfoID { get; set; }
        public DateTime CreatedDate { get; set; }
        public float SoilMoisture { get; set; }
        public float Light { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
    }
}