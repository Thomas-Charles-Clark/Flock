using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslatePatternGridViewModel
    {
        public List<Pattern_Language_SelectList_PatternResult> Patterns { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslatePatternGridViewModel()
        {
            Patterns = new List<Pattern_Language_SelectList_PatternResult>();
        }

        public TranslatePatternGridViewModel(
            List<Pattern_Language_SelectList_PatternResult> patterns,
            byte? targetLanguageId)
        {
            Patterns = patterns;
            TargetLanguageID = targetLanguageId;
        }
    }
}