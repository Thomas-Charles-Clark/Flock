using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateEmailTextGridViewModel
    {
        public List<EmailText_Language_SelectList_EmailTextResult> EmailTexts { get; set; }
        public List<EmailText_Language_SelectList_EmailTemplateResult> EmailTemplates { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateEmailTextGridViewModel()
        {
            EmailTexts = new List<EmailText_Language_SelectList_EmailTextResult>();
        }

        public TranslateEmailTextGridViewModel(
            List<EmailText_Language_SelectList_EmailTextResult> emailTexts,
            List<EmailText_Language_SelectList_EmailTemplateResult> emailTemplates,
            byte? targetLanguageId)
        {
            EmailTexts = emailTexts;
            EmailTemplates = emailTemplates;
            TargetLanguageID = targetLanguageId;
        }
    }
}