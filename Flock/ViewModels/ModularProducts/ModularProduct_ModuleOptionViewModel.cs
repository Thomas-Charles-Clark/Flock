using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ModularProduct_ModuleOptionViewModel
    {
        public Int16 ModularProductID { get; set; }

        public List<ModuleOption> SelectedOptions { get; set; }

        public List<ModuleOption> AvailableOptions { get; set; }
    }
}