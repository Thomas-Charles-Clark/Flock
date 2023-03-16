using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateUsageGridViewModel
    {
        public List<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult> Usages { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateUsageGridViewModel()
        {
            Usages = new List<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult>();
        }

        public TranslateUsageGridViewModel(
            List<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult> usages,
            byte? targetLanguageId)
        {
            Usages = usages;
            TargetLanguageID = targetLanguageId;
        }
    }
}