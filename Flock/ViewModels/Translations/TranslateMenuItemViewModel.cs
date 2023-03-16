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
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateMenuItemViewModel()
        {

        }
    }
}