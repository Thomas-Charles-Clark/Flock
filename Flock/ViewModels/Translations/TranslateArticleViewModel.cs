using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateArticleViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public int? ArticleID { get; set; }

        public TranslateArticleViewModel()
        {

        }
    }
}