using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ReportingManagerViewModel : BaseViewModel
    {
        public List<ReportGroup> ReportGroups { get; set; }

        public List<Employee> Employees { get; set; }
    }
}