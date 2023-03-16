using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateTechInfo_PatternMatchGridViewModel
    {
        public List<TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult> TechInfo_PatternMatchs { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateTechInfo_PatternMatchGridViewModel()
        {
            TechInfo_PatternMatchs = new List<TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult>();
        }

        public TranslateTechInfo_PatternMatchGridViewModel(
            List<TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult> techInfo_PatternMatchs,
            byte? targetLanguageId)
        {
            TechInfo_PatternMatchs = techInfo_PatternMatchs;
            TargetLanguageID = targetLanguageId;
        }
    }
}