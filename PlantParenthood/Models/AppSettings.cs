using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlantParenthood.Models
{
    public class AppSettings
    {
        [Key]
        public int AppSettingsID { get; set; }

        public string Name { get; set; }
        public bool Value { get; set; }
    }
}