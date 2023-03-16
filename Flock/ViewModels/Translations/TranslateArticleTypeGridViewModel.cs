using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateArticleTypeGridViewModel
    {
        public List<ArticleType_Language_SelectList_ArticleTypeResult> ArticleTypes { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateArticleTypeGridViewModel()
        {
            ArticleTypes = new List<ArticleType_Language_SelectList_ArticleTypeResult>();
        }

        public TranslateArticleTypeGridViewModel(
            List<ArticleType_Language_SelectList_ArticleTypeResult> articleTypes,
            byte? targetLanguageID)
        {
            ArticleTypes = articleTypes;
            TargetLanguageID = targetLanguageID;
        }
    }
}