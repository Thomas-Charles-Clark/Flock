using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;
using System.ComponentModel.DataAnnotations;

namespace Flock.ViewModels
{
    public class SlideshowImageViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public SlideshowImageViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public SlideshowImageViewModel Build(long slideshowID, long imageID)
        {
            SlideshowImageViewModel Model = new SlideshowImageViewModel();

            SlideShow_Image db = UoW.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == slideshowID && p.ImageID == imageID).Single();

            Model.SlideshowID = slideshowID;
            Model.ImageID = imageID;
            Model.Heading = db.Heading;
            Model.SortOrder = db.SortOrder;
            Model.Text = db.Text;
            Model.ExternalURL = db.ExternalURL;
            Model.NewWindow = db.NewWindow;
            Model.PageID = db.PageID;
            Model.ProductPageID = db.ProductPageID;
            Model.CollectionID = db.CollectionID;

            return Model;
        }
    }

    public class SlideshowImageViewModel
    {
        public long SlideshowID { get; set; }

        public long ImageID { get; set; }

        public byte SortOrder { get; set; }

        [StringLength(70)]
        public string Heading { get; set; }

        [StringLength(500)]
        public string Text { get; set; }

        public int? PageID { get; set; }

        public short? CollectionID { get; set; }

        public int? ProductPageID { get; set; }

        public bool NewWindow { get; set; }

        [StringLength(255)]
        public string ExternalURL { get; set; }
    }
}