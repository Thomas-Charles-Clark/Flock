using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateResourceGridViewModel
    {
        public List<Resource_Language_SelectList_ResourceResult> Resources { get; set; }
        public short? TargetCountryLanguageID { get; set; }
        
        public TranslateResourceGridViewModel()
        {
            Resources = new List<Resource_Language_SelectList_ResourceResult>();
        }

        public TranslateResourceGridViewModel(
            List<Resource_Language_SelectList_ResourceResult> resources,
            short? targetCountryLanguageId)
        {
            Resources = resources;
            TargetCountryLanguageID = targetCountryLanguageId;
        }
    }
}