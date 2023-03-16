using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class CacheBusterViewModel : BaseViewModel
    {
        public List<PassCountryLanguage> Sites { get; set; }
    }
}