using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateDeliveryOptionGridViewModel
    {
        public List<DeliveryOption_Language_SelectList_DeliveryOptionResult> DeliveryOptions { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateDeliveryOptionGridViewModel()
        {
            DeliveryOptions = new List<DeliveryOption_Language_SelectList_DeliveryOptionResult>();
        }

        public TranslateDeliveryOptionGridViewModel(
            List<DeliveryOption_Language_SelectList_DeliveryOptionResult> deliveryOptions,
            byte? targetLanguageId)
        {
            DeliveryOptions = deliveryOptions;
            TargetLanguageID = targetLanguageId;
        }
    }
}