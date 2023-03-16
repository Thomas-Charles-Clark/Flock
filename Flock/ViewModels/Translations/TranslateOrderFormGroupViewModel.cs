using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateOrderFormGroupViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public short? OrderFormGroupID { get; set; }
        public string OrderFormGroup { get; set; }
        public string TranslatedOrderFormGroup { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateOrderFormGroupViewModel()
        {

        }
    }
}