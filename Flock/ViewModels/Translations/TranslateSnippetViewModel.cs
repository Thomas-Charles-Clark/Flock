using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateSnippetViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public short? SnippetID { get; set; }

        public TranslateSnippetViewModel()
        {

        }
    }
}