using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateTechInfo_PatternMatchViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public int? PatternMatchID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateTechInfo_PatternMatchViewModel()
        {

        }
    }
}