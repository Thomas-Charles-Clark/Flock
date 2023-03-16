using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductPageClassifierViewModel
    {
        public bool IsAttributeGroupMultiSelect { get; set; }
        public bool IsFullConfiguration { get; set; }
        public string ToggleButtonText { get; set; }
        public List<Flock.Models.Attribute> AllAttributes { get; set; }
        public List<ProductPage_Attribute_Generate_ProductPageResult> ProductPages { get; set; }
        public List<ProductPage_Attribute_Generate_AttributeMapResult> Mapping { get; set; }
    }
}