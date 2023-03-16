using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateArticleGridViewModel
    {
        public List<Article_Language_SelectList_ArticleResult> Articles { get; set; }
        public List<Brand_SelectResult> Brands { get; set; }
        public List<ArticleSubType_SelectResult> ArticleSubTypes { get; set; }
        public List<Status_SelectResult> Statuses { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public int ArticlesToTranslateCount { get; set; }

        public TranslateArticleGridViewModel()
        {

        }

        public TranslateArticleGridViewModel(List<Article_Language_SelectList_ArticleResult> collections,
            List<Brand_SelectResult> brands,
            List<ArticleSubType_SelectResult> articleSubTypes,
            List<Status_SelectResult> statuses,
            int articlesToTranslateCount,
            byte? targetCountryLanguageID)
        {
            Articles = collections;
            Brands = brands;
            ArticleSubTypes = articleSubTypes;
            Statuses = statuses;
            ArticlesToTranslateCount = articlesToTranslateCount;
            TargetLanguageID = targetCountryLanguageID;
        }
    }
}