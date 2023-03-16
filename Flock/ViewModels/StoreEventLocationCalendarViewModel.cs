using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class StoreEventLocationCalendarViewModel
    {
        public Int16 StoreEventID { get; set; }

        public Int16 StoreEventLocationID { get; set; }

        public List<StoreEvent_Date> SelectedDates { get; set; }
    }
}