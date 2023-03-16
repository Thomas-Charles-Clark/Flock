using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateArticleSubTypeGridViewModel
    {
        public List<ArticleSubType_Language_SelectList_ArticleSubTypeResult> ArticleSubTypes { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateArticleSubTypeGridViewModel()
        {
            ArticleSubTypes = new List<ArticleSubType_Language_SelectList_ArticleSubTypeResult>();
        }

        public TranslateArticleSubTypeGridViewModel(
            List<ArticleSubType_Language_SelectList_ArticleSubTypeResult> articleSubType,
            byte? targetLanguageId)
        {
            ArticleSubTypes = articleSubType;
            TargetLanguageID = targetLanguageId;
        }
    }
}