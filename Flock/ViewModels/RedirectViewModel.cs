using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class RedirectViewModel
    {
        public int? RedirectID { get; set; }

        [Required]
        [RegularExpression("/^([a-z]|[a-z]-|[a-z]/)+[a-z]$")]
        public string OldURL { get; set; }

        [Required]
        public string NewURL { get; set; }
    }
}