using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateTechInfo_UsageGridViewModel
    {
        public List<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult> TechInfo_Usages { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateTechInfo_UsageGridViewModel()
        {
            TechInfo_Usages = new List<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult>();
        }

        public TranslateTechInfo_UsageGridViewModel(
            List<TechInfo_Usage_Language_SelectList_TechInfo_UsageResult> techInfo_Usages,
            byte? targetLanguageId)
        {
            TechInfo_Usages = techInfo_Usages;
            TargetLanguageID = targetLanguageId;
        }
    }
}