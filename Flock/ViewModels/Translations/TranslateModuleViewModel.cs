using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateModuleViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public Int16? ModuleID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public string Description { get; set; }
        public string TranslatedDescription { get; set; }
        public string ConfirmText { get; set; }
        public string TranslatedConfirmText { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateModuleViewModel()
        {

        }
    }
}