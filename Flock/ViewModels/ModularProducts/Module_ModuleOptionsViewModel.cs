using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class Module_ModuleOptionsViewModel
    {
        public Int16 ModuleID { get; set; }

        public List<ModuleOptionGroup> ModuleOptionGroups { get; set; }
        public List<ModuleOptionSortDisplay> ModuleOptions { get; set; }
    }
}