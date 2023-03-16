using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductTypeAttributeGroupViewModel
    {
        public List<AttributeGroup_ProductType> AttributeGroups { get; set; }
        public List<Attribute_ProductType> Attributes { get; set; }
        public List<AttributeGroup> AvailableGroups { get; set; }
    }
}