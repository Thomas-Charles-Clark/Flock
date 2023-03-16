using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class BaseViewModel
    {
        public List<Flock.Models.Market> Markets { get; set; }
        public List<Flock.Models.Brand> Brands { get; set; }
        public List<Flock.Models.Launch> Launches { get; set; }
        public List<Flock.Models.Colour> Colours { get; set; }
        public List<Flock.Models.ProductType> ProductTypes { get; set; }
        public List<Flock.Models.Collection> Collections { get; set; }
        public List<Flock.Models.AspectRatio> AspectRatios { get; set; }
        public List<Flock.Models.ImageType> ImageTypes { get; set; }
        public List<Flock.Models.Perspective> Perspectives { get; set; }
        public List<Flock.Models.Room> Rooms { get; set; }
        public List<Flock.Models.Style> Styles { get; set; }
        public List<Flock.Models.Video> Videos { get; set; }
        //public List<Flock.Models.Brochure> Brochures { get; set; }
        public List<Flock.Models.Design> Designs { get; set; }
    }
}