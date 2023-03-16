using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateTechInfo_CompositionGridViewModel
    {
        public List<TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult> TechInfo_Compositions { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateTechInfo_CompositionGridViewModel()
        {
            TechInfo_Compositions = new List<TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult>();
        }

        public TranslateTechInfo_CompositionGridViewModel(
            List<TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult> techInfo_Compositions,
            byte? targetLanguageId)
        {
            TechInfo_Compositions = techInfo_Compositions;
            TargetLanguageID = targetLanguageId;
        }
    }
}