using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslatePageDetailViewModel
    {
        public int PageID { get; set; }
        public byte TargetLanguageID { get; set; }
        public string TargetLanguage { get; set; }
        public Page_Language_SelectDetail_PageResult English { get; set; }
        public Page_Language_SelectDetail_TranslationResult Translation { get; set; }
        public Page_Language_SelectDetail_ReviewRequiredResult FieldsNeedingReview { get; set; }
        public Page_Language_SelectDetail_ImageResult Image { get; set; }

        public TranslatePageDetailViewModel()
        {

        }

        public TranslatePageDetailViewModel(
            int pageID,
            byte targetLanguageId,
            string targetLanguage,
            Page_Language_SelectDetail_PageResult english,
            Page_Language_SelectDetail_TranslationResult translation,
            Page_Language_SelectDetail_ReviewRequiredResult fieldsNeedingReview,
            Page_Language_SelectDetail_ImageResult image)
        {
            PageID = pageID;
            TargetLanguageID = targetLanguageId;
            TargetLanguage = targetLanguage;
            English = english;
            Translation = translation;
            FieldsNeedingReview = fieldsNeedingReview;
            Image = image;
        }
    }
}