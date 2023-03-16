using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateReturnReasonViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public byte? ReturnReasonID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateReturnReasonViewModel()
        {

        }
    }
}