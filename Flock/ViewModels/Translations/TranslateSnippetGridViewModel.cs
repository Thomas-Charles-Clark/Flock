using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateSnippetGridViewModel
    {
        public List<Snippet_Language_SelectList_SnippetResult> Snippets { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public int SnippetsToTranslateCount { get; set; }

        public TranslateSnippetGridViewModel()
        {

        }

        public TranslateSnippetGridViewModel(List<Snippet_Language_SelectList_SnippetResult> snippets,
            int snippetsToTranslateCount,
            byte? targetLanguageID)
        {
            Snippets = snippets;
            SnippetsToTranslateCount = snippetsToTranslateCount;
            TargetLanguageID = targetLanguageID;
        }
    }
}