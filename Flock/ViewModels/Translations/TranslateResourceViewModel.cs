using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateResourceViewModel : BaseViewModel
    {
        public short? TargetCountryLanguageID { get; set; }
        public int? ResourceID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string TranslatedValue { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateResourceViewModel()
        {

        }
    }
}