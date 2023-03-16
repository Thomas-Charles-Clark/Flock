using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class PageSection_ProductPageViewModel
    {
        public List<PageSection_ProductPage_Select_CoreResult> CoreResults { get; set; }
        public List<PageSection_ProductPage_Select_RelatedProductPageResult> RelatedProductPages { get; set; }
        public List<PageSection_ProductPage_Select_SecondaryProductResult> SecondaryProducts { get; set; }
        public List<PageSection_ProductPage_Select_PriceStockResult> PriceAndStock { get; set; }
        public List<PageSection_ProductPage_Select_ColourResult> Colours { get; set; }
        public List<PageSection_ProductPage_Select_LaunchResult> Launches { get; set; }
        public List<PageSection_ProductPage_Select_StatusResult> Statuses { get; set; }

        public Int64 PageSectionID { get; set; }
        public string CurrencySymbol { get; set; }
    }
}