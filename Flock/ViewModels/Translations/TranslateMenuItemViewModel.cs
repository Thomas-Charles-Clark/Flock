using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateMenuItemViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public short? MenuItemID { get; set; }
        public string Name { get; set; }
        public string ExternalURL { get; set; }
        public string QueryStringParameters { get; set; }
        public string TranslatedName { get; set; }
        public string TranslatedExternalURL { get; set; }
        public string TranslatedQueryStringParameters { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateMenuItemViewModel()
        {

        }
    }
}