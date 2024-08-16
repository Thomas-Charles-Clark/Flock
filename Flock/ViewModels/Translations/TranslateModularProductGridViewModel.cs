using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateModularProductGridViewModel
    {
        public List<ModularProduct_Language_SelectList_ModularProductResult> ModularProducts { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateModularProductGridViewModel()
        {
            ModularProducts = new List<ModularProduct_Language_SelectList_ModularProductResult>();
        }

        public TranslateModularProductGridViewModel(
            List<ModularProduct_Language_SelectList_ModularProductResult> modularProducts,
            byte? targetLanguageId)
        {
            ModularProducts = modularProducts;
            TargetLanguageID = targetLanguageId;
        }
    }
}