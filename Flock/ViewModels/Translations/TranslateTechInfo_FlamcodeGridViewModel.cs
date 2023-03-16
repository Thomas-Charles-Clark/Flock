using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateTechInfo_FlamcodeGridViewModel
    {
        public List<TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult> TechInfo_Flamcodes { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateTechInfo_FlamcodeGridViewModel()
        {
            TechInfo_Flamcodes = new List<TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult>();
        }

        public TranslateTechInfo_FlamcodeGridViewModel(
            List<TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult> techInfo_Flamcodes,
            byte? targetLanguageId)
        {
            TechInfo_Flamcodes = techInfo_Flamcodes;
            TargetLanguageID = targetLanguageId;
        }
    }
}