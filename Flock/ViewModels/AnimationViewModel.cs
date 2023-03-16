using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;
using Flock.Classes;

namespace Flock.ViewModels
{
    public class AnimationViewModel : BaseViewModel
    {
    }

    public class AnimationGridModel
    {
        public List<Animation> Animations { get; set; }
        public List<AspectRatio> AspectRatios { get; set; }

        public AnimationGridModel()
        {

        }

        public AnimationGridModel(UnitOfWork<FlockDataContext> UoW)
        {
            Animations = UoW.GetRepository<Animation>().GetAll().Where(p => p.DeletedDT == null).OrderByDescending(p => p.AnimationID).ToList();
            AspectRatios = UoW.GetRepository<AspectRatio>().GetAll().OrderBy(p => p.AspectRatioID).ToList();
        }
    }

    public class AnimationEditModel
    {
        public Int16 AnimationID { get; set; }

        [Required]
        [StringLength(100)]
        public String Name { get; set; }

        [Required]
        public Byte AspectRatioID { get; set; }
    }

    public class AnimationDetailModel
    {
        public Int32? SlideToLoad { get; set; }

        public Animation Animation { get; set; }

        public List<CallToAction> AvailableCallsToAction { get; set; }

        public List<Video> AvailableVideos { get; set; }

        public AnimationDetailModel()
        {

        }

        public AnimationDetailModel(UnitOfWork<FlockDataContext> UoW, Int32 AnimationID) 
        {
            this.Animation = UoW.GetRepository<Animation>().GetAll().Where(p => p.AnimationID == AnimationID).Single();
            this.AvailableCallsToAction = UoW.GetRepository<CallToAction>().GetAll().Where(p => p.DeletedDT == null).OrderByDescending(p => p.CallToActionID).ToList();
            this.AvailableVideos = UoW.GetRepository<Video>().GetAll().Where(p => p.DeletedDT == null).OrderByDescending(p => p.VideoID).ToList();
            this.SlideToLoad = null;
        }

        public AnimationDetailModel(UnitOfWork<FlockDataContext> UoW, Int16 AnimationID, Int32? SlideToLoad)
        {
            this.Animation = UoW.GetRepository<Animation>().GetAll().Where(p => p.AnimationID == AnimationID).Single();
            this.AvailableCallsToAction = UoW.GetRepository<CallToAction>().GetAll().Where(p => p.DeletedDT == null).OrderByDescending(p => p.CallToActionID).ToList();
            this.AvailableVideos = UoW.GetRepository<Video>().GetAll().Where(p => p.DeletedDT == null).OrderByDescending(p => p.VideoID).ToList();
            this.SlideToLoad = SlideToLoad;
        }
    }

    public class SlideEditModel
    {
        public Int32 SlideID { get; set; }

        public Int16 AnimationID { get; set; }

        public Animation Animation { get; set; }

        [Required]
        public Byte SlideTypeID { get; set; }

        [Required]
        public Boolean DisplayCallsToAction { get; set; }

        [Required]
        public Boolean AutoHideCallsToAction { get; set; }

        
        [LinkageMutex("SlideArticleID", "SlidePageID", "SlideCollectionID", "SlideProductPageID", "SlideExternalURL", ErrorMessage = "Mutually Exclusive")]
        public int? SlideArticleID { get; set; }

        [LinkageMutex("SlideArticleID", "SlidePageID", "SlideCollectionID", "SlideProductPageID", "SlideExternalURL", ErrorMessage = "Mutually Exclusive")]
        public int? SlidePageID { get; set; }

        [LinkageMutex("SlideArticleID", "SlidePageID", "SlideCollectionID", "SlideProductPageID", "SlideExternalURL", ErrorMessage = "Mutually Exclusive")]
        public int? SlideProductPageID { get; set; }

        [LinkageMutex("SlideArticleID", "SlidePageID", "SlideCollectionID", "SlideProductPageID", "SlideExternalURL", ErrorMessage = "Mutually Exclusive")]
        public short? SlideCollectionID { get; set; }

        [StringLength(255)]
        [LinkageMutex("SlideArticleID", "SlidePageID", "SlideCollectionID", "SlideProductPageID", "SlideExternalURL", ErrorMessage = "Mutually Exclusive")]
        public string SlideExternalURL { get; set; }

        public Boolean SlideNewWindow { get; set; }

        public Image Image { get; set; }

        public Video Video { get; set; }

        [Required]
        public Boolean DisplayHotspots { get; set; }

        [Required]
        public Boolean AutoHideHotspots { get; set; }

        public Byte SortOrder { get; set; }

        public List<SlideType> AvailableSlideTypes { get; set; }

        public List<Slide_CallToAction> AssignedCallsToAction { get; set; }

        public SlideEditModel()
        {

        }

        public SlideEditModel(UnitOfWork<FlockDataContext> UoW, short AnimationID, int? SlideID = null)
        {
            this.AnimationID = AnimationID;

            this.Animation = UoW.GetRepository<Animation>().GetAll().Where(p => p.AnimationID == AnimationID).Single();

            var s = UoW.GetRepository<Slide>().GetAll().Where(p => p.SlideID == SlideID).SingleOrDefault();

            this.SlideTypeID = 1;
            this.DisplayCallsToAction = true;
            this.DisplayHotspots = true;
            this.AutoHideCallsToAction = false;
            this.AutoHideHotspots = false;

            if (s != null)
            {
                this.SlideID = s.SlideID;
                this.SlideTypeID = s.SlideTypeID;
                this.DisplayCallsToAction = s.DisplayCallsToAction;
                this.AutoHideCallsToAction = s.AutoHideCallsToAction;

                this.SlideArticleID = s.ArticleID;
                this.SlideCollectionID = s.CollectionID;
                this.SlidePageID = s.PageID;
                this.SlideProductPageID = s.ProductPageID;
                this.SlideExternalURL = s.ExternalURL;
                this.SlideNewWindow = s.NewWindow;

                this.Image = s.Image;
                this.Video = s.Video;
                this.DisplayHotspots = s.DisplayHotspots;
                this.AutoHideHotspots = s.AutoHideHotspots;
                this.SortOrder = s.SortOrder;
            }

            this.AvailableSlideTypes = UoW.GetRepository<SlideType>().GetAll().ToList();

            this.AssignedCallsToAction = UoW.GetRepository<Slide_CallToAction>().GetAll().Where(p => p.SlideID == SlideID && p.CallToAction.DeletedDT == null).ToList();
        }

        public void Rebuild(UnitOfWork<FlockDataContext> UoW)
        {
            var s = UoW.GetRepository<Slide>().GetAll().Where(p => p.SlideID == SlideID).SingleOrDefault();

            Animation = s.Animation;

            SlideTypeID = s.SlideTypeID;

            SortOrder = s.SortOrder;

            Image = s.Image;

            Video = s.Video;

            AvailableSlideTypes = UoW.GetRepository<SlideType>().GetAll().ToList();

            AssignedCallsToAction = UoW.GetRepository<Slide_CallToAction>().GetAll().Where(p => p.SlideID == SlideID && p.CallToAction.DeletedDT == null).ToList();
        }
    }
}