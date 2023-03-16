using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateProductPageViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public int? ProductPageID { get; set; }

        public TranslateProductPageViewModel()
        {

        }
    }
}