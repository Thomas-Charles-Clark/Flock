using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateReturnReasonGridViewModel
    {
        public List<ReturnReason_Language_SelectList_ReturnReasonResult> ReturnReasons { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateReturnReasonGridViewModel()
        {
            ReturnReasons = new List<ReturnReason_Language_SelectList_ReturnReasonResult>();
        }

        public TranslateReturnReasonGridViewModel(
            List<ReturnReason_Language_SelectList_ReturnReasonResult> returnReasons,
            byte? targetLanguageId)
        {
            ReturnReasons = returnReasons;
            TargetLanguageID = targetLanguageId;
        }
    }
}