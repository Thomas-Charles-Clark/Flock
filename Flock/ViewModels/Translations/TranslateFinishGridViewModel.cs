using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateFinishGridViewModel
    {
        public List<Finish_Language_SelectList_FinishResult> Finishes { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateFinishGridViewModel()
        {
            Finishes = new List<Finish_Language_SelectList_FinishResult>();
        }

        public TranslateFinishGridViewModel(
            List<Finish_Language_SelectList_FinishResult> finishes,
            byte? targetLanguageId)
        {
            Finishes = finishes;
            TargetLanguageID = targetLanguageId;
        }
    }
}