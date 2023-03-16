using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateCallToActionViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public long? CallToActionID { get; set; }
        public string Text { get; set; }
        public string TranslatedText { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateCallToActionViewModel()
        {

        }
    }
}