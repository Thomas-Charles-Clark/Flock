using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;
using System.ComponentModel.DataAnnotations;

namespace Flock.ViewModels
{
    public class SlideshowImageOverlayViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public SlideshowImageOverlayViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public SlideshowImageOverlayViewModel Build(long pageSectionID, long imageID)
        {
            SlideshowImageOverlayViewModel Model = new SlideshowImageOverlayViewModel();

            var slideshowID = UoW.GetRepository<PageSection_SlideShow>().GetAll().Where(p => p.PageSectionID == pageSectionID).Select(p => p.SlideShowID).Single();

            SlideShow_Image db = UoW.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == slideshowID && p.ImageID == imageID).Single();

            var arID = "ar" + UoW.GetRepository<Image>().GetAll().Where(p => p.ImageID == imageID).Select(p => p.AspectRatioID).Single();

            Model.Index = (byte)(db.SortOrder + 1);
            Model.ImagesTotal = (byte)UoW.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == slideshowID).Count();

            Model.SlideshowID = slideshowID;
            Model.PageSectionID = pageSectionID;
            Model.ImageID = imageID;
            Model.SortOrder = db.SortOrder;
            Model.ExternalURL = db.ExternalURL;
            Model.NewWindow = db.NewWindow;
            Model.PageID = db.PageID;
            Model.ProductPageID = db.ProductPageID;
            Model.CollectionID = db.CollectionID;

            Model.OverlayTop = db.OverlayTop == null ? 80.0M : db.OverlayTop;
            Model.OverlayLeft = db.OverlayLeft == null ? 0.0M : db.OverlayLeft;
            Model.OverlayWidth = db.OverlayWidth == null ? 100.0M : db.OverlayWidth;
            Model.OverlayHeight = db.OverlayHeight == null ? 20.0M : db.OverlayHeight;
            Model.OverlayBackgroundOpacity = db.OverlayBackgroundOpacity == null ? 0.60M : db.OverlayBackgroundOpacity;
            Model.OverlayBackgroundColour = db.OverlayBackgroundColour == null ? "255, 255, 255" : db.OverlayBackgroundColour;
            Model.OverlayFontColour = db.OverlayFontColour == null ? "0, 0, 0" : db.OverlayFontColour;
            Model.SlideshowOverlayPositionID = db.SlideshowOverlayPositionID == null ? 1 : db.SlideshowOverlayPositionID;
            Model.AutoHideOverlay = db.AutoHideOverlay;
            Model.OverlayHeading = db.OverlayHeading;
            Model.OverlayText = db.OverlayText;
            Model.DisplayOverlay = db.DisplayOverlay;
            Model.ShowHotspots = db.ShowHotspots;
            Model.OverlayFade = db.OverlayFade;

            Model.ARID = arID;
            // Also, get the hotspots displaying on the advanced editor thingie?

            // Need to deliver a list of CallToActionSettings

            Model.CallToActionConfigurationID = db.SlideShow_Image_CallToActionConfigurationID;

            Model.AvailableConfigurations = UoW.GetRepository<SlideShow_Image_CallToActionConfiguration>().GetAll().ToList();

            return Model;
        }

        public SlideshowImageOverlayViewModel Build(int articleID, long imageID)
        {
            SlideshowImageOverlayViewModel Model = new SlideshowImageOverlayViewModel();

            Article_Image db = UoW.GetRepository<Article_Image>().GetAll().Where(p => p.ArticleID == articleID && p.ImageID == imageID).Single();

            var arID = "ar" + UoW.GetRepository<Image>().GetAll().Where(p => p.ImageID == imageID).Select(p => p.AspectRatioID).Single();

            Model.Index = (byte)(db.SortOrder + 1);
            Model.ImagesTotal = (byte)UoW.GetRepository<Article_Image>().GetAll().Where(p => p.ArticleID == articleID).Count();

            Model.ArticleID = articleID;
            Model.SlideshowID = null;
            Model.PageSectionID = null;
            Model.ImageID = imageID;
            Model.SortOrder = db.SortOrder;
            Model.ExternalURL = db.ExternalURL;
            Model.NewWindow = db.NewWindow;
            Model.PageID = db.PageID;
            Model.ProductPageID = db.ProductPageID;
            Model.CollectionID = db.CollectionID;

            Model.OverlayTop = db.OverlayTop == null ? 80.0M : db.OverlayTop;
            Model.OverlayLeft = db.OverlayLeft == null ? 0.0M : db.OverlayLeft;
            Model.OverlayWidth = db.OverlayWidth == null ? 100.0M : db.OverlayWidth;
            Model.OverlayHeight = db.OverlayHeight == null ? 20.0M : db.OverlayHeight;
            Model.OverlayBackgroundOpacity = db.OverlayBackgroundOpacity == null ? 0.60M : db.OverlayBackgroundOpacity;
            Model.OverlayBackgroundColour = db.OverlayBackgroundColour == null ? "255, 255, 255" : db.OverlayBackgroundColour;
            Model.OverlayFontColour = db.OverlayFontColour == null ? "0, 0, 0" : db.OverlayFontColour;
            Model.SlideshowOverlayPositionID = db.SlideshowOverlayPositionID == null ? 1 : db.SlideshowOverlayPositionID;
            Model.AutoHideOverlay = db.AutoHideOverlay;
            Model.OverlayHeading = db.OverlayHeading;
            Model.OverlayText = db.OverlayText;
            Model.DisplayOverlay = db.DisplayOverlay;
            Model.ShowHotspots = db.ShowHotspots;

            Model.ARID = arID;
            // Also, get the hotspots displaying on the advanced editor thingie?

            Model.CallToActionConfigurationID = db.SlideShow_Image_CallToActionConfigurationID;

            Model.AvailableConfigurations = UoW.GetRepository<SlideShow_Image_CallToActionConfiguration>().GetAll().ToList();

            return Model;
        }
    }

    public class SlideshowImageOverlayViewModel
    {
        public byte Index { get; set; }

        public byte ImagesTotal { get; set; }

        public long? SlideshowID { get; set; }

        public long? PageSectionID { get; set; }

        public int? ArticleID { get; set; }

        public long ImageID { get; set; }

        public byte SortOrder { get; set; }

        public string ARID { get; set; }

        [StringLength(200)]
        public string Heading { get; set; }

        [StringLength(500)]
        public string Text { get; set; }

        public int? PageID { get; set; }

        public short? CollectionID { get; set; }

        public int? ProductPageID { get; set; }

        public bool NewWindow { get; set; }

        [StringLength(255)]
        public string ExternalURL { get; set; }

        public decimal? OverlayTop { get; set; }

        public decimal? OverlayLeft { get; set; }

        public decimal? OverlayWidth { get; set; }

        public decimal? OverlayHeight { get; set; }

        public decimal? OverlayBackgroundOpacity  { get; set; }

        public string OverlayBackgroundColour { get; set; }

        public string OverlayFontColour { get; set; }

        public byte? SlideshowOverlayPositionID { get; set; }

        public bool? AutoHideOverlay { get; set; }

        [StringLength(200, MinimumLength=3)]
        public string OverlayHeading { get; set; }

        public string OverlayText { get; set; }

        public bool DisplayOverlay { get; set; }

        public bool ShowHotspots { get; set; }

        public bool OverlayFade { get; set; }

        public short? CallToActionConfigurationID { get; set; }

        public List<SlideShow_Image_CallToActionConfiguration> AvailableConfigurations { get; set; }
    }
}