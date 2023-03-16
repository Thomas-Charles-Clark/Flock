using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateSiteSectionGridViewModel
    {
        public List<SiteSection_Language_SelectList_SiteSectionResult> SiteSections { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateSiteSectionGridViewModel()
        {
            SiteSections = new List<SiteSection_Language_SelectList_SiteSectionResult>();
        }

        public TranslateSiteSectionGridViewModel(
            List<SiteSection_Language_SelectList_SiteSectionResult> siteSectiones,
            byte? targetLanguageId)
        {
            SiteSections = siteSectiones;
            TargetLanguageID = targetLanguageId;
        }
    }
}