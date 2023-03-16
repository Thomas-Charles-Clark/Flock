using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateOrderStatusGridViewModel
    {
        public List<OrderStatus_Language_SelectList_OrderStatusResult> OrderStatuses { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateOrderStatusGridViewModel()
        {
            OrderStatuses = new List<OrderStatus_Language_SelectList_OrderStatusResult>();
        }

        public TranslateOrderStatusGridViewModel(
            List<OrderStatus_Language_SelectList_OrderStatusResult> orderStatuses,
            byte? targetLanguageId)
        {
            OrderStatuses = orderStatuses;
            TargetLanguageID = targetLanguageId;
        }
    }
}