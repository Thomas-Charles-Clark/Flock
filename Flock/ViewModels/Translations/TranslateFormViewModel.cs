using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateFormViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public int? FormID { get; set; }
        public string Heading { get; set; }
        public string TranslatedHeading { get; set; }
        public string SubHeading { get; set; }
        public string TranslatedSubHeading { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateFormViewModel()
        {

        }
    }
}