using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateModuleOptionGroupViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public Int16? ModuleOptionGroupID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public string Description { get; set; }
        public string TranslatedDescription { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateModuleOptionGroupViewModel()
        {

        }
    }
}