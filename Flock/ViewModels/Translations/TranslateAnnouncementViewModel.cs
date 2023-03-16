using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateAnnouncementViewModel : BaseViewModel
    {
        public short? TargetLanguageID { get; set; }
        public int? AnnouncementID { get; set; }
        public string Text { get; set; }
        public string TranslatedText { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateAnnouncementViewModel()
        {

        }
    }
}