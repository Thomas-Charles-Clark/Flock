using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class WeeklyNotesViewModel
    {
        public Byte Week { get; set; }
        public Int16 Year { get; set; }
        public Byte PriceListID { get; set; }

        public Boolean FoundRecord { get; set; }
        public String Objectives { get; set; }
        public String Observations { get; set; }
    }
}