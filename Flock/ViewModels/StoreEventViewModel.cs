using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class StoreEventViewModel
    {
        public Int16? StoreEventID { get; set; }

        [Required]
        [StringLength(100)]
        public String Name { get; set; }

        public List<StoreEventLocation> StoreEventLocations { get; set; }

        public List<StoreEvent_StoreEventLocation> SelectedStoreEventLocations { get; set; }
    }
}