using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class StoreEvent_Date_ViewModel
    {
        public Int16 StoreEventID { get; set; }

        public Int16 StoreEventLocationID { get; set; }

        public Int32 StoreEvent_DateID { get; set; }

        public List<StoreEvent_Date_TimeSlot> TimeSlots { get; set; }
    }
}