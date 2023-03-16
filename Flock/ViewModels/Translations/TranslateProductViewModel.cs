using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateProductViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public int? ProductID { get; set; }
        public string OrderFormName { get; set; }
        public string TranslatedOrderFormName { get; set; }
        public string BasketName { get; set; }
        public string TranslatedBasketName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateProductViewModel()
        {

        }
    }
}