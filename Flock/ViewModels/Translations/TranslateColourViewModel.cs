using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateColourViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public int? ColourID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateColourViewModel()
        {

        }
    }
}