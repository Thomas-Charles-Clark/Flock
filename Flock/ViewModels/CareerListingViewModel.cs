using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.ViewModels
{
    public class CareerListingViewModel
    {
        public Int16? CareerListingID { get; set; }

        [Required]
        [StringLength(140)]
        public String Title { get; set; }

        [Required(ErrorMessage = "Please provide a Location for this Listing.")]
        [StringLength(140)]
        public String Location { get; set; }

        public DateTime ApplyBeforeDT { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Please provide Contents for this Listing.")]
        public String Contents { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(256)]
        public String ContactEmail { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = "Please provide a full Postal Address (including recipient name) for this Listing.")]
        public String ContactPostal { get; set; }

        public Boolean Salaried { get; set; }

        public Int16 SortOrder { get; set; }

        public DateTime? OnDate { get; set; }

        public DateTime? OffDate { get; set; }

        public DateTime? CreatedDT { get; set; }

        public DateTime? DeletedDT { get; set; }
    }
}