using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class SlideshowImage
    {
        public List<Flock.Models.ImageHotspot_SelectResult> Hotspots { get; set; }
        public Flock.Models.SlideShow_Image Record { get; set; }
    }
}