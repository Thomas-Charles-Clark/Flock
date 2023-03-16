using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateSlideShowImageViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public Int64? SlideShowID { get; set; }
        public Int64? ImageID {get;set;}
        public string OverlayHeading { get; set; }
        public string OverlayText { get; set; }
        public string TranslatedOverlayHeading { get; set; }
        public string TranslatedOverlayText { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateSlideShowImageViewModel()
        {

        }
    }
}