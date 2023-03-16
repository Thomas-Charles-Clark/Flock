using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductPageClassifierHeaderViewModel
    {
        public byte? SelectedProductTypeID { get; set; }
        public byte? SelectedAttributeGroupID { get; set; }
        public short? SelectedAttributeID { get; set; }
        public byte? SelectedToggleState { get; set; } // 0 1 2
    }
}