using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateProductPageDetailViewModel
    {
        public int ProductPageID { get; set; }
        public byte TargetLanguageID { get; set; }
        public string TargetLanguage { get; set; }
        public ProductPage_Language_SelectDetail_ProductPageResult English { get; set; }
        public ProductPage_Language_SelectDetail_TranslationResult Translation { get; set; }
        public ProductPage_Language_SelectDetail_ReviewRequiredResult FieldsNeedingReview { get; set; }
        public ProductPage_Language_SelectDetail_ImageResult Image { get; set; }

        public TranslateProductPageDetailViewModel()
        {

        }

        public TranslateProductPageDetailViewModel(
            int productPageID,
            byte targetLanguageId,
            string targetLanguage,
            ProductPage_Language_SelectDetail_ProductPageResult english,
            ProductPage_Language_SelectDetail_TranslationResult translation,
            ProductPage_Language_SelectDetail_ReviewRequiredResult fieldsNeedingReview,
            ProductPage_Language_SelectDetail_ImageResult image)
        {
            ProductPageID = productPageID;
            TargetLanguageID = targetLanguageId;
            TargetLanguage = targetLanguage;
            English = english;
            Translation = translation;
            FieldsNeedingReview = fieldsNeedingReview;
            Image = image;
        }
    }
}