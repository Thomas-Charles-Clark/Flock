using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class TranslateStockStatusViewModel : BaseViewModel
    {
        public byte? TargetLanguageID { get; set; }
        public byte? StockStatusID { get; set; }
        public string StockStatus { get; set; }
        public string TranslatedStockStatus { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }

        public TranslateStockStatusViewModel()
        {

        }
    }
}