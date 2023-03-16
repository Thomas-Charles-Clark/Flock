using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductPageColourwayRow
    {
        public int DesignID { get; set; }
        public string Name { get; set; }
        public byte ProductTypeID { get; set; }
        public short LaunchID { get; set; }
        public int ProductPageCount { get; set; }
        public byte? BrandID { get; set; }
        public object Colourways { get; set; }
        public int ColourwayCount { get; set; }
    }
}