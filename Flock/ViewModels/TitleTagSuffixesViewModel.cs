using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TitleTagSuffixViewModel
    {
        public short Country_Language_ID { get; set; }

        [StringLength(50)]
        public string TitleTagSuffix { get; set; }

        public DateTime CreatedDT { get; set; }
        public DateTime ModifiedDT { get; set; }
        public DateTime? DeletedDT { get; set; }
    }
}