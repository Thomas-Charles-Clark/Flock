using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateArticleDetailViewModel
    {
        public short ArticleID { get; set; }
        public byte TargetLanguageID { get; set; }
        public string TargetLanguage { get; set; }
        public Article_Language_SelectDetail_ArticleResult English { get; set; }
        public Article_Language_SelectDetail_TranslationResult Translation { get; set; }
        public Article_Language_SelectDetail_ReviewRequiredResult FieldsNeedingReview { get; set; }
        public Article_Language_SelectDetail_ImageResult Image { get; set; }

        public TranslateArticleDetailViewModel()
        {

        }

        public TranslateArticleDetailViewModel(
            short collectionID,
            byte targetLanguageId,
            string targetLanguage,
            Article_Language_SelectDetail_ArticleResult english,
            Article_Language_SelectDetail_TranslationResult translation,
            Article_Language_SelectDetail_ReviewRequiredResult fieldsNeedingReview,
            Article_Language_SelectDetail_ImageResult image)
        {
            ArticleID = collectionID;
            TargetLanguageID = targetLanguageId;
            TargetLanguage = targetLanguage;
            English = english;
            Translation = translation;
            FieldsNeedingReview = fieldsNeedingReview;
            Image = image;
        }
    }
}