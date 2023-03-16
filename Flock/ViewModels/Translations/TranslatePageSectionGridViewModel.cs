using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslatePageSectionGridViewModel
    {
        public List<PageSection_Language_SelectList_PageSectionResult> PageSections { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslatePageSectionGridViewModel()
        {
            PageSections = new List<PageSection_Language_SelectList_PageSectionResult>();
        }

        public TranslatePageSectionGridViewModel(
            List<PageSection_Language_SelectList_PageSectionResult> pageSections,
            byte? targetLanguageId)
        {
            PageSections = pageSections;
            TargetLanguageID = targetLanguageId;
        }
    }
}