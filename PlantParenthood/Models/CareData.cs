using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlantParenthood.Models
{
    public class CareData
    {
        [Key]
        public int CareInfoID { get; set; }

        public string PlantName { get; set; }
        public float SoilMoisture { get; set; }
        public float Light { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public bool Owned { get; set; }
        public bool Current { get; set; }
    }
}