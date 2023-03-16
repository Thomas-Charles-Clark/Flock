using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateArticleTypeViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public byte? ArticleTypeID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateArticleTypeViewModel()
        {

        }
    }
}