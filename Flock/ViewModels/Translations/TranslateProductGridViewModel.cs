using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateProductGridViewModel
    {
        public List<Launch_SelectResult> Launches { get; set; }
        public List<Product_Language_SelectList_ProductResult> Products { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateProductGridViewModel()
        {
            Products = new List<Product_Language_SelectList_ProductResult>();
            Launches = new List<Launch_SelectResult>();
        }

        public TranslateProductGridViewModel(
            List<Product_Language_SelectList_ProductResult> products,
            List<Launch_SelectResult> launches,
            byte? targetLanguageId)
        {
            Products = products;
            Launches = launches;
            TargetLanguageID = targetLanguageId;
        }
    }
}