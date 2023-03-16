using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateHtmlSectionDetailViewModel
    {
        public long HtmlSectionID { get; set; }
        public byte TargetLanguageID { get; set; }
        public string TargetLanguage { get; set; }
        public HtmlSection_Language_SelectDetail_HtmlSectionResult English { get; set; }
        public HtmlSection_Language_SelectDetail_TranslationResult Translation { get; set; }
        public HtmlSection_Language_SelectDetail_ReviewRequiredResult FieldsNeedingReview { get; set; }

        public TranslateHtmlSectionDetailViewModel()
        {

        }

        public TranslateHtmlSectionDetailViewModel(
            long htmlSectionID,
            byte targetLanguageID,
            string targetLanguage,
            HtmlSection_Language_SelectDetail_HtmlSectionResult english,
            HtmlSection_Language_SelectDetail_TranslationResult translation,
            HtmlSection_Language_SelectDetail_ReviewRequiredResult fieldsNeedingReview)
        {
            HtmlSectionID = htmlSectionID;
            TargetLanguageID = targetLanguageID;
            TargetLanguage = targetLanguage;
            English = english;
            Translation = translation;
            FieldsNeedingReview = fieldsNeedingReview;
        }
    }
}