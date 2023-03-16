using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.ViewModels
{
    public class AnnouncementEditModel
    {
        public int? AnnouncementID { get; set; }

        [Required]
        [AllowHtml]
        public string Text { get; set; }

        public DateTime? OnDate { get; set; }

        public DateTime? OffDate { get; set; }
    }
}