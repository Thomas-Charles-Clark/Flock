using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class SalesOverviewResultsViewModel : BaseViewModel
    {
        public SalesOverviewSummary Summary { get; set; }
        public List<SalesOverviewTransaction> Transactions { get; set; }
    }
}