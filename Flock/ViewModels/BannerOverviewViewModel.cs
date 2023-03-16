using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class BannerOverviewViewModel
    {

    }

    public class BannerSelectGridSettings
    {
        public byte[] BannerLocationIDs { get; set; }
        public byte[] SiteSectionIDs { get; set; }
        public short[] CountryIDs { get; set; }
        public int[] PageIDs { get; set; }
    }
}