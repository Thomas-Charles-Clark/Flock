using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateHtmlSectionGridViewModel
    {
        public List<HtmlSection_Language_SelectList_HtmlSectionResult> HtmlSections { get; set; }
        public byte? TargetLanguageID { get; set; }

        public List<Status> Statuses { get; set; }
        
        public int HtmlSectionsToTranslateCount { get; set; }

        public TranslateHtmlSectionGridViewModel()
        {

        }

        public TranslateHtmlSectionGridViewModel(List<HtmlSection_Language_SelectList_HtmlSectionResult> htmlSections,
            List<Status> statuses,
            int htmlSectionsToTranslateCount,
            byte? targetLanguageID)
        {
            HtmlSections = htmlSections;
            Statuses = statuses;
            HtmlSectionsToTranslateCount = htmlSectionsToTranslateCount;
            TargetLanguageID = targetLanguageID;
        }
    }
}