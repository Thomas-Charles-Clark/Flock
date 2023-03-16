using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateProductTypeViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public int? ProductTypeID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public string NamePlural { get; set; }
        public string TranslatedNamePlural { get; set; }
        public string URLKeywords { get; set; }
        public string TranslatedURLKeywords { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateProductTypeViewModel()
        {

        }
    }
}