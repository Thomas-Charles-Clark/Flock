using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Flock.DAL;


namespace Flock.Models.ContentItems
{
    public class SlideshowViewModel : ContentItem
    {
        public SlideShow Record;

        public SlideShowSettings Settings { get; set; }

        public PageSection_SlideShow Linker; // do we actually need to store the linker on the SlideShowItem?
    }

    public class SlideshowModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public SlideshowModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public SlideshowViewModel Build(long slideShowID)
        {
            SlideshowViewModel Model = new SlideshowViewModel();
            Model.Record = UoW.GetRepository<SlideShow>().GetAll().Where(p => p.SlideShowID == slideShowID).Single();
            //Model.Linker = UoW.GetRepository<PageSection_SlideShow>().GetAll().Where(p => p.PageSectionID == pageSectionID && p.SlideShowID == slideShowID).Single();

            Model.Settings = new JavaScriptSerializer().Deserialize<SlideShowSettings>(Model.Record.Settings);

            Model.Settings.Images = UoW.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == slideShowID).OrderBy(p => p.SortOrder).ToList();
            
            Model.Settings.SlideShowID = slideShowID;
            
            Model.ItemType = ContentItemType.SlideShow;
           
            return Model;
        }

        public bool Create(long pageSectionID, string className, int sortOrder)
        {
            try
            {
                SlideshowViewModel SSI = new SlideshowViewModel();

                SSI.Record = new SlideShow();
                SSI.ItemType = ContentItemType.SlideShow;
                SSI.SortOrder = sortOrder;
                SSI.Record.Name = "tempName";
                SSI.Settings = new SlideShowSettings();
                SSI.Record.Settings = new JavaScriptSerializer().Serialize(SSI.Settings);
                SSI.Record.SortOrder = sortOrder;
                SSI.Record.Class = className;
                SSI.Record.ShowHotspots = false;
                UoW.GetRepository<SlideShow>().Add(SSI.Record);
                UoW.Save();

                UoW.GetRepository<PageSection_SlideShow>().Add(new PageSection_SlideShow() { SlideShowID = SSI.Record.SlideShowID, PageSectionID = pageSectionID });
                UoW.Save();


            }
            catch (Exception e)
            {
                throw (e);
            }

            return true;
        }

        public bool Save(SlideShowSettings settings)
        {
            try
            {
                SlideShow slideShow = UoW.GetRepository<SlideShow>().GetAll().Where(p => p.SlideShowID == settings.SlideShowID).Single();

                slideShow.Settings = new JavaScriptSerializer().Serialize(settings);
                
                UoW.Save();
            }
            catch (Exception e)
            {
                throw (e);
            }
            return true;
        }
    }

    public class SlideShowSettings
    {
        public long SlideShowID { get; set; }

        public NavigationDirection NavigationDirection { get; set; }
        public NavigationBarPosition NavigationPosition { get; set; }
        public ElementVisibilityMode NavigationThumbnailVisibilityMode { get; set; }
        public NavigationBarMode NavigationBarMode { get; set; }
        public bool ShowNavigationBar { get; set; }
        public int NavigationBarImageSpacing { get; set; }
        public NavigationBarPagingMode NavigationBarPagingMode { get; set; }
        public int NavigationItemsVisibleCount { get; set; }

        public ImageSizeMode ImageSizeMode { get; set; }
        public ElementVisibilityMode NavigationButtonVisibility { get; set; }
        public ElementVisibilityMode ItemTextVisibility { get; set; }

        public AnimationType AnimationMode { get; set; }

        public bool AutoPlay { get; set; }
        public int Interval { get; set; }
        public ElementVisibilityMode PlayPauseButtonVisibility { get; set; }
        public bool StopPlayingWhenPaging { get; set; }
        public bool PausePlayingWhenMouseOver { get; set; }

        public bool EnablePagingGestures { get; set; }
        public AutoBoolean EnablePagingByClick { get; set; }

        public ImageLoadMode ImageLoadMode { get; set; }
        public ExtremeItemClickMode ExtremeItemClickMode { get; set; }
        public bool AllowMouseWheel { get; set; }

        public string Name { get; set; }

        public bool EnableTheming { get; set; }

        public int AnimationDuration { get; set; }

        public List<SlideShow_Image> Images { get; set; }

        public SlideShowSettings()
        {
            AutoPlay = true;
            Interval = 5000;
            PlayPauseButtonVisibility = ElementVisibilityMode.Faded;
            StopPlayingWhenPaging = false;
            PausePlayingWhenMouseOver = false;

            ImageSizeMode = ImageSizeMode.ActualSizeOrFit;
            AnimationMode = AnimationType.Slide;

            NavigationBarMode = NavigationBarMode.Dots;
            NavigationDirection = NavigationDirection.Horizontal;
            NavigationPosition = NavigationBarPosition.Bottom;
            NavigationThumbnailVisibilityMode = ElementVisibilityMode.OnMouseOver;
            NavigationButtonVisibility = ElementVisibilityMode.OnMouseOver;

            ItemTextVisibility = ElementVisibilityMode.OnMouseOver;

            EnablePagingGestures = true;
            EnablePagingByClick = AutoBoolean.Auto;
            ImageLoadMode = ImageLoadMode.DynamicLoadAndCache;
            ExtremeItemClickMode = ExtremeItemClickMode.SelectAndSlide;

            EnablePagingByClick = AutoBoolean.Auto;

            ShowNavigationBar = true;

            AllowMouseWheel = true;

            Name = string.Format("slideshow-{0}", DateTime.UtcNow);
            EnableTheming = true;

            AnimationDuration = 300;

            NavigationBarImageSpacing = 5;

            NavigationItemsVisibleCount = 5;
        }

        ~SlideShowSettings()
        {

        }
    }

    public static class ImageSliderDemoHelper
    {
        public static List<SelectListItem> GetNavigationBarDirections()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = "Horizontal", Value = NavigationDirection.Horizontal.ToString() },
                new SelectListItem() { Text = "Vertical", Value = NavigationDirection.Vertical.ToString() }
            };
        }


        public static List<SelectListItem> GetNavigationBarPositions()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = "Bottom", Value = NavigationBarPosition.Bottom.ToString() },
                new SelectListItem() { Text = "Top", Value = NavigationBarPosition.Top.ToString() },
                new SelectListItem() { Text = "Left", Value = NavigationBarPosition.Left.ToString() },
                new SelectListItem() { Text = "Right", Value = NavigationBarPosition.Right.ToString() }
            };
        }

        public static List<SelectListItem> GetElementVisibilityModes()
        {
            return new List<SelectListItem>() { 
                new SelectListItem() { Text = ElementVisibilityMode.Always.ToString(), Value = ElementVisibilityMode.Always.ToString() },
                new SelectListItem() { Text = ElementVisibilityMode.Faded.ToString(), Value = ElementVisibilityMode.Faded.ToString() },
                new SelectListItem() { Text = "Invisible", Value = ElementVisibilityMode.None.ToString() },
                new SelectListItem() { Text = ElementVisibilityMode.OnMouseOver.ToString(), Value = ElementVisibilityMode.OnMouseOver.ToString() },
            };
        }

        public static List<SelectListItem> GetAnimationTypes()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = AnimationType.Auto.ToString(), Value = AnimationType.Auto.ToString() },
                new SelectListItem() { Text = AnimationType.Fade.ToString(), Value = AnimationType.Fade.ToString() },
                new SelectListItem() { Text = AnimationType.Slide.ToString(), Value = AnimationType.Slide.ToString() },
                new SelectListItem() { Text = AnimationType.None.ToString(), Value = AnimationType.None.ToString() }
            };
        }

        public static List<SelectListItem> GetPredefinedScenarios(string selectedScenario)
        {
            List<SelectListItem> predefinedScenarios = new List<SelectListItem>() {
                new SelectListItem() { Text = "Default", Value = "Default" },
                new SelectListItem() { Text = "Fill and Crop, Dots", Value = "FillAndCropDots" },
                new SelectListItem() { Text = "Vertical Scrolling", Value = "VerticalScrolling" }
            };
            if (!string.IsNullOrEmpty(selectedScenario))
                predefinedScenarios.Find(item => item.Value == selectedScenario).Selected = true;
            return predefinedScenarios;
        }
        public static List<SelectListItem> GetNavigationBarModes()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = "Dots", Value = NavigationBarMode.Dots.ToString() },
                new SelectListItem() { Text = "Thumbnails", Value = NavigationBarMode.Thumbnails.ToString() }
            };
        }

        public static List<SelectListItem> GetSlideShowIntervals()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = "1000", Value = "1000" },
                new SelectListItem() { Text = "2000", Value = "2000" },
                new SelectListItem() { Text = "4000", Value = "4000" },
                new SelectListItem() { Text = "3000", Value = "3000" },
                new SelectListItem() { Text = "5000", Value = "5000", Selected = true }
            };
        }
        public static List<SelectListItem> GetVisibleItemsCount()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = "3", Value = "3" },
                new SelectListItem() { Text = "5", Value = "5", Selected = true }
            };
        }
    }
}