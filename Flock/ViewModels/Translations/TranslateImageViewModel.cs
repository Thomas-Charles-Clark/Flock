using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateImageViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public long? ImageID { get; set; }
        public string TitleText { get; set; }
        public string TranslatedTitleText { get; set; }
        public string AltText { get; set; }
        public string TranslatedAltText { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateImageViewModel()
        {

        }
    }
}