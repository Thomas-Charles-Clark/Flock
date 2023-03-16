using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class ModuleOptionViewModel
    {
        public Int32? ModuleOptionID { get; set; }

        public Int16 ModuleID { get; set; }

        public Int16? ModuleOptionGroupID { get; set; }

        [Required]
        [StringLength(100)]
        public String Name { get; set; }

        public Int32? ProductID { get; set; }

        public Int64? ImageID { get; set; }
    }
}