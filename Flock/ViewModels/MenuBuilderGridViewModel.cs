using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class MenuBuilderGridViewModel
    {
        public List<MenuBuilder_MenuSelectResult> Menus { get; set; }

        public List<MenuBuilder_MenuCountrySelectResult> CountryRestrictions { get; set; }
    }
}