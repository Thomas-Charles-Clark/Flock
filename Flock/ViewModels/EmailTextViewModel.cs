using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class EmailTextViewModel
    {
        public short? EmailTextID { get; set; }
        
        [Required]
        public int? EmailTemplateID { get; set; }
        
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Text { get; set; }
    }
}