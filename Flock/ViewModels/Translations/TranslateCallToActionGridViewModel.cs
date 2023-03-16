using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateCallToActionGridViewModel
    {
        public List<CallToAction_Language_SelectList_CallToActionResult> CallsToAction { get; set; }
        public byte? TargetLanguageId { get; set; }
        
        public TranslateCallToActionGridViewModel()
        {
            CallsToAction = new List<CallToAction_Language_SelectList_CallToActionResult>();
        }

        public TranslateCallToActionGridViewModel(
            List<CallToAction_Language_SelectList_CallToActionResult> callsToAction,
            byte? targetLanguageId)
        {
            CallsToAction = callsToAction;
            TargetLanguageId = targetLanguageId;
        }
    }
}