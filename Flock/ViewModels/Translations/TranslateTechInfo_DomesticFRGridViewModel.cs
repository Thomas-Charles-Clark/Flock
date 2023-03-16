using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateTechInfo_DomesticFRGridViewModel
    {
        public List<TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult> TechInfo_DomesticFRs { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateTechInfo_DomesticFRGridViewModel()
        {
            TechInfo_DomesticFRs = new List<TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult>();
        }

        public TranslateTechInfo_DomesticFRGridViewModel(
            List<TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult> techInfo_DomesticFRs,
            byte? targetLanguageId)
        {
            TechInfo_DomesticFRs = techInfo_DomesticFRs;
            TargetLanguageID = targetLanguageId;
        }
    }
}