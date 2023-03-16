using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateFormGridViewModel
    {
        public List<Form_Language_SelectList_FormResult> Forms { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateFormGridViewModel()
        {
            Forms = new List<Form_Language_SelectList_FormResult>();
        }

        public TranslateFormGridViewModel(
            List<Form_Language_SelectList_FormResult> forms,
            byte? targetLanguageId)
        {
            Forms = forms;
            TargetLanguageID = targetLanguageId;
        }
    }
}