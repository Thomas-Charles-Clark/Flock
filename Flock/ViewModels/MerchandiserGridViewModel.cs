using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductPageGenerateGridViewModel
    {
        public List<ProductPage_Generate_Select_CoreResult> Merchandisers { get; set; }
        public List<ProductPage_Generate_Select_ProductResult> Products { get; set; }

        public List<Brand_SelectResult> Brands { get; set; }
        public List<ProductType_SelectResult> ProductTypes { get; set; }
        public List<Launch_SelectResult> Launches { get; set; }
        public List<NamingConvention> NamingConventions { get; set; }
    }

    public class ProductPageGenerateDetailGridViewModel
    {
        public Int32 MerchandiserID { get; set; }

        public List<ProductPage_Generate_Select_ProductResult> Products { get; set; }

        public List<Brand_SelectResult> Brands { get; set; }
        public List<ProductType_SelectResult> ProductTypes { get; set; }
        public List<Launch_SelectResult> Launches { get; set; }
        public List<NamingConvention> NamingConventions { get; set; }
    }
}