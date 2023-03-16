using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateGiftVoucherValidationGridViewModel
    {
        public List<GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult> GiftVoucherValidations { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateGiftVoucherValidationGridViewModel()
        {
            GiftVoucherValidations = new List<GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult>();
        }

        public TranslateGiftVoucherValidationGridViewModel(
            List<GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult> giftVoucherValidations,
            byte? targetLanguageId)
        {
            GiftVoucherValidations = giftVoucherValidations;
            TargetLanguageID = targetLanguageId;
        }
    }
}