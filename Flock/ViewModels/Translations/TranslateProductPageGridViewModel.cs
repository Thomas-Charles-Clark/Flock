using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateProductPageGridViewModel
    {
        public List<ProductPage_Language_SelectList_ProductPageResult> ProductPages { get; set; }
        public List<Brand_SelectResult> Brands { get; set; }
        public List<Launch_SelectResult> Launches { get; set; }
        public List<ProductType_SelectResult> ProductTypes { get; set; }
        public List<Status_SelectResult> Statuses { get; set; }
        public List<ProductPage_TemplateSelectResult> Templates { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public int ProductPagesToTranslateCount { get; set; }

        public TranslateProductPageGridViewModel()
        {

        }

        public TranslateProductPageGridViewModel(List<ProductPage_Language_SelectList_ProductPageResult> productPages,
            List<Brand_SelectResult> brands,
            List<Launch_SelectResult> launches,
            List<ProductType_SelectResult> productTypes,
            List<Status_SelectResult> statuses,
            List<ProductPage_TemplateSelectResult> templates,
            int productPagesToTranslateCount,
            byte? targetLanguageId)
        {
            ProductPages = productPages;
            Brands = brands;
            Launches = launches;
            ProductTypes = productTypes;
            Statuses = statuses;
            Templates = templates;
            ProductPagesToTranslateCount = productPagesToTranslateCount;
            TargetLanguageID = targetLanguageId;
        }
    }
}