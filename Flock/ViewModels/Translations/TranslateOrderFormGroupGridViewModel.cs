using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateOrderFormGroupGridViewModel
    {
        public List<OrderFormGroup_Language_SelectList_OrderFormGroupResult> OrderFormGroups { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateOrderFormGroupGridViewModel()
        {
            OrderFormGroups = new List<OrderFormGroup_Language_SelectList_OrderFormGroupResult>();
        }

        public TranslateOrderFormGroupGridViewModel(
            List<OrderFormGroup_Language_SelectList_OrderFormGroupResult> orderFormGroups,
            byte? targetLanguageId)
        {
            OrderFormGroups = orderFormGroups;
            TargetLanguageID = targetLanguageId;
        }
    }
}