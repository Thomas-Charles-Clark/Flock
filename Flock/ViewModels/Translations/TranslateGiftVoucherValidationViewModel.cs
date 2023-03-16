using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateGiftVoucherValidationViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public byte? GiftVoucherValidationID { get; set; }
        public string Message { get; set; }
        public string TranslatedMessage { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateGiftVoucherValidationViewModel()
        {

        }
    }
}