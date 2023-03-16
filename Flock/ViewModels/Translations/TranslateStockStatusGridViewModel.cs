using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateStockStatusGridViewModel
    {
        public List<StockStatus_Language_SelectList_StockStatusResult> StockStatuses { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateStockStatusGridViewModel()
        {
            StockStatuses = new List<StockStatus_Language_SelectList_StockStatusResult>();
        }

        public TranslateStockStatusGridViewModel(
            List<StockStatus_Language_SelectList_StockStatusResult> stockStatuses,
            byte? targetLanguageId)
        {
            StockStatuses = stockStatuses;
            TargetLanguageID = targetLanguageId;
        }
    }
}