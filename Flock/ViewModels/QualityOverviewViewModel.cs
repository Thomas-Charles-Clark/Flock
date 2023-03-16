using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class QualityOverviewViewModel 
    {
        public List<Quality_Overview_Grid_Record> Records { get; set; }

        public List<Quality_Overview_Generate_ProductTypeResult> ProductTypes { get; set; }

        public List<Quality_Overview_Generate_BrandResult> Brands { get; set; }

        public List<Quality_Overview_Generate_LaunchResult> Launches { get; set; }

        public List<Quality_Overview_Generate_QualityCollectionResult> QualitiesCollections { get; set; }
    }


    public class Quality_Overview_Grid_Record
    {
        public short QualityID { get; set; }

        public string Name { get; set; }

        public byte ProductTypeID { get; set; }
        public string ProductTypeName { get; set; }

        public short FirstLaunchID { get; set; }
        public string FirstLaunchName { get; set; }

        public short LastLaunchID { get; set; }
        public string LastLaunchName { get; set; }

        public byte BrandID { get; set; }
        public string BrandName { get; set; }

        public int ProductCount { get; set; }

        public short CollectionCount { get; set; }

        public string Code { get; set; }


        public double? Width { get; set; }

        public double? Weight { get; set; }

        public double? HorizontalPatternRepeat { get; set; }

        public double? VerticalPatternRepeat { get; set; }

        public string Composition { get; set; }

        public int? Martindale { get; set; }
    }
}