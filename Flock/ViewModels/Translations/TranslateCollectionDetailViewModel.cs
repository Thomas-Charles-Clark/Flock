using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateCollectionDetailViewModel
    {
        public short CollectionID { get; set; }
        public byte TargetLanguageID { get; set; }
        public string TargetLanguage { get; set; }
        public Collection_Language_SelectDetail_CollectionResult English { get; set; }
        public Collection_Language_SelectDetail_TranslationResult Translation { get; set; }
        public Collection_Language_SelectDetail_ReviewRequiredResult FieldsNeedingReview { get; set; }
        public Collection_Language_SelectDetail_ImageResult Image { get; set; }

        public TranslateCollectionDetailViewModel()
        {

        }

        public TranslateCollectionDetailViewModel(
            short collectionId,
            byte targetLanguageId,
            string targetLanguage,
            Collection_Language_SelectDetail_CollectionResult english,
            Collection_Language_SelectDetail_TranslationResult translation,
            Collection_Language_SelectDetail_ReviewRequiredResult fieldsNeedingReview,
            Collection_Language_SelectDetail_ImageResult image)
        {
            CollectionID = collectionId;
            TargetLanguageID = targetLanguageId;
            TargetLanguage = targetLanguage;
            English = english;
            Translation = translation;
            FieldsNeedingReview = fieldsNeedingReview;
            Image = image;
        }
    }
}