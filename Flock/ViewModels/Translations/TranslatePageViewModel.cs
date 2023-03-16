using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslatePageViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public int? PageID { get; set; }

        public TranslatePageViewModel()
        {

        }
    }
}