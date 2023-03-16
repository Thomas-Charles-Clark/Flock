using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateProductTypeGridViewModel
    {
        public List<ProductType_Language_SelectList_ProductTypeResult> ProductTypes { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateProductTypeGridViewModel()
        {
            ProductTypes = new List<ProductType_Language_SelectList_ProductTypeResult>();
        }

        public TranslateProductTypeGridViewModel(
            List<ProductType_Language_SelectList_ProductTypeResult> productTypes,
            byte? targetLanguageId)
        {
            ProductTypes = productTypes;
            TargetLanguageID = targetLanguageId;
        }
    }
}