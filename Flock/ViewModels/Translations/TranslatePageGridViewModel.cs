using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslatePageGridViewModel
    {
        public List<Page_Language_SelectList_PageResult> Pages { get; set; }
        public List<SiteSection_SelectResult> SiteSections { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public int PagesToTranslateCount { get; set; }

        public TranslatePageGridViewModel()
        {

        }

        public TranslatePageGridViewModel(List<Page_Language_SelectList_PageResult> productPages,
            List<SiteSection_SelectResult> siteSections,
            int pagesToTranslateCount,
            byte? targetLanguageId)
        {
            Pages = productPages;
            SiteSections = siteSections;
            PagesToTranslateCount = pagesToTranslateCount;
            TargetLanguageID = targetLanguageId;
        }
    }
}