using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateSnippetDetailViewModel
    {
        public short SnippetID { get; set; }
        public short TargetLanguageID { get; set; }
        public string TargetLanguage { get; set; }
        public Snippet_Language_SelectDetail_SnippetResult English { get; set; }
        public Snippet_Language_SelectDetail_TranslationResult Translation { get; set; }
        public Snippet_Language_SelectDetail_ReviewRequiredResult FieldsNeedingReview { get; set; }

        public TranslateSnippetDetailViewModel()
        {

        }

        public TranslateSnippetDetailViewModel(
            short snippetID,
            short targetLanguageID,
            string targetLanguage,
            Snippet_Language_SelectDetail_SnippetResult english,
            Snippet_Language_SelectDetail_TranslationResult translation,
            Snippet_Language_SelectDetail_ReviewRequiredResult fieldsNeedingReview)
        {
            SnippetID = snippetID;
            TargetLanguageID = targetLanguageID;
            TargetLanguage = targetLanguage;
            English = english;
            Translation = translation;
            FieldsNeedingReview = fieldsNeedingReview;
        }
    }
}