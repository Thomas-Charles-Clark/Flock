using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateHtmlSectionViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public long? HtmlSectionID { get; set; }

        public TranslateHtmlSectionViewModel()
        {

        }
    }
}