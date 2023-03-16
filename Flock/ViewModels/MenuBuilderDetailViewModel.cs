using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class MenuBuilderDetailViewModel
    {
        public byte MenuID { get; set; }
        public string Name { get; set; }
        public string MessageHTML { get; set; }
        public byte SortOrder { get; set; }

        public List<Country> CountryRestrictions { get; set; }
    }
}