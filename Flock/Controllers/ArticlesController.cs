using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using DevExpress.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace Flock.Controllers
{
    public class ArticlesController : Controller
    {
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /Articles/
        public ActionResult Index()
        {
            return RedirectToAction("overview");
        }

        public ActionResult Overview()
        {
            return View();
        }

        public ActionResult Compose(int? id)
        {
            ArticleComposeViewModel AVM = new ArticleComposeViewModel();

            if (id != null)
            {
                if (UnitOfWork.GetRepository<Article>().GetAll().Where(p => p.ArticleID == id && p.DeletedDT == null).SingleOrDefault() != null)
                {
                    ArticleViewModelBuilder PageBuilder = new ArticleViewModelBuilder(UnitOfWork);
                    AVM = PageBuilder.Build(id);
                }
                else
                {
                    AVM = new ArticleComposeViewModel();
                    ViewData["InvalidArticleIDError"] = "Invalid Article ID, or the article is marked as deleted. Deleted articles can be retrieved.";
                }
                return View("ComposeArticle", AVM);
            }
            else
            {
                ArticleComposeViewModel ACVM = new ArticleComposeViewModel();
                return View("ComposeArticle", ACVM);
            }
        }

        //settings.Name = "ArticleComposeGridView";
        //settings.CallbackRouteValues = new { Controller = "Articles", Action = "ArticleComposeGridViewPartial" };
        //settings.SettingsEditing.AddNewRowRouteValues = new { Controller = "Articles", Action = "ArticleComposeGridViewAddNew" };
        //settings.SettingsEditing.DeleteRowRouteValues = new { Controller = "Articles", Action = "ArticleComposeGridViewDelete" };
        public ActionResult ArticleComposeGridViewPartial()
        {
            return PartialView();
        }

        public ActionResult ArticleSearchArticleTypeDropDownPartial()
        {
            var Model = UnitOfWork.GetRepository<ArticleType>().GetAll().ToList();
            return PartialView(Model);
        }

        public ActionResult ArticleSearchNameOrIDTextBoxPartial()
        {
            return PartialView();
        }

        public ActionResult ArticleComposeGridViewAddNew([ModelBinder(typeof(DevExpressEditorsBinder))]ArticleCreationModel item)
        {
            if(ModelState.IsValid)
            {
                string subTypeText = UnitOfWork.GetRepository<ArticleSubType>().GetAll().Where(p => p.Deleted == false && p.ArticleSubTypeID == item.ArticleSubTypeID).Select(p => p.Name).Single().ToLower().Replace(" ", "-");

                var a = new Article()
                {
                    Name = item.Name,
                    ArticleTypeID = item.ArticleTypeID,
                    ArticleSubTypeID = item.ArticleSubTypeID,
                    ArticleTemplateID = item.ArticleTemplateID,
                    TitleTag = item.TitleTag,
                    Heading = item.Heading,
                    URLKeywords = "dg-world/" + subTypeText,
                    CreatedDT = DateTime.Now
                };

                UnitOfWork.GetRepository<Article>().Add(a);
                UnitOfWork.Save();

                UnitOfWork.Context.Article_Language_Initialise();

                UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(null, null, null, null, a.ArticleID, false, false);
            }

            return PartialView("ArticleComposeGridViewPartial", item);
        }

        public ActionResult ArticlecomposeGridViewDelete([ModelBinder(typeof(DevExpressEditorsBinder))]Article item)
        {
            if (ModelState.IsValid)
            {
                var modelItem = UnitOfWork.GetRepository<Article>().GetAll().Where(p => p.ArticleID == item.ArticleID).SingleOrDefault();
                if(modelItem != null)
                {
                    modelItem.DeletedDT = DateTime.Now;
                }
                UnitOfWork.Save();
            }

            return PartialView("ArticleComposeGridViewPartial", item);
        }

        [HttpPost]
        public ActionResult ChangeArticle(int ID)
        {
            ArticleViewModelBuilder Builder = new ArticleViewModelBuilder(UnitOfWork);
            ArticleComposeViewModel Model = Builder.Build(ID);

            return PartialView("ArticleComposePartial", Model);
        }


        [HttpPost]
        public ActionResult SaveArticleBasicInfo([ModelBinder(typeof(DevExpressEditorsBinder))]Article item)
        {
            try
            {
                if (item.OnDate == null && item.OffDate != null)
                {
                    ViewData["EditError"] += "Please specify an On Date.";
                    ModelState.AddModelError("OnDate", "Please specify an On Date");
                }
                if (item.OnDate != null && item.OffDate != null)
                {
                    if (item.OffDate <= item.OnDate)
                    {
                        ViewData["EditError"] += "On Date must be before Off Date.";
                        ModelState.AddModelError("OnDate", "On Date must be before Off Date");
                        ModelState.AddModelError("OffDate", "Off Date must be after On Date");
                    }
                }
                if (ModelState.IsValid) 
                { 
                    var modelItem = UnitOfWork.GetRepository<Article>().GetAll().Where(p => p.ArticleID == item.ArticleID).SingleOrDefault();
                    if(modelItem != null)
                    {
                        if (modelItem.ArticleTemplateID != item.ArticleTemplateID)
                            ViewData["Warning"] = "You have changed this Article's Template. You will have to reload this editor page in order to use the features specific to your chosen template. SAVE YOUR WORK FIRST.";

                        modelItem.Name = item.Name;
                        modelItem.OnDate = item.OnDate;
                        modelItem.OffDate = item.OffDate;
                        modelItem.EventDT = item.EventDT;

                        if(modelItem.ArticleTemplateID != item.ArticleTemplateID)
                        {
                            modelItem.ArticleTemplateID = item.ArticleTemplateID;

                            var articleImages = UnitOfWork.GetRepository<Article_Image>().GetAll().Where(p => (p.TemplateImageID == 2 || p.TemplateImageID == 3) && p.ArticleID == item.ArticleID).ToList();
                            
                            UnitOfWork.GetRepository<Article_Image>().DeleteAll(articleImages);
                        }

                        modelItem.ArticleTypeID = item.ArticleTypeID;
                        modelItem.ArticleSubTypeID = item.ArticleSubTypeID;
                        UnitOfWork.Save();
                    }
                }
            }
            catch(Exception e)
            {
                Response.StatusDescription = e.Message;
            }

            ArticleViewModelBuilder Builder = new ArticleViewModelBuilder(UnitOfWork);
            ArticleComposeViewModel Model = Builder.Build(item.ArticleID);

            return PartialView("ArticleBasicInfoPartial", Model);
        }

        [HttpPost]
        public ActionResult SaveArticleDetails([ModelBinder(typeof(DevExpressEditorsBinder))]ArticleComposeViewModel item)
        {
            try 
            {
                if (ModelState.IsValid) 
                { 
                    var modelItem = UnitOfWork.GetRepository<Article>().GetAll().Where(p => p.ArticleID == item.ArticleID).SingleOrDefault();
                    if(modelItem != null)
                    {
                        modelItem.TitleTag = item.TitleTag;
                        modelItem.URLKeywords = item.URLKeywords;
                        modelItem.MetaDescription = item.MetaDescription;
                        modelItem.Heading = item.Heading;
                        modelItem.SubHeading = item.SubHeading;
                        UnitOfWork.Save();
                        //UnitOfWork.Context.ReplaceLocalURLPlaceholders_Article(item.ArticleID, null, null, null, null, null, false, false);
                        UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(null, null, null, null, item.ArticleID, false, false);
                    }
                }
            }
            catch(Exception e)
            {
                Response.StatusCode = 400;
                return Content(e.Message);
            }

            var Model = new ArticleViewModelBuilder(UnitOfWork).Build(item.ArticleID);

            return PartialView("ArticleDetailsPartial", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateArticleImageArea(int ArticleID, string Images, byte TemplateImageID)
        {
            // from Slideshow, coming in bad
            var ArticleTemplateID = UnitOfWork.GetRepository<Article>().GetAll().Where(p => p.ArticleID == ArticleID).Single().ArticleTemplateID;
          
            UnitOfWork.Context.Article_Image_Merge(ArticleID, Images, TemplateImageID);
           
            var Model = new ArticleViewModelBuilder(UnitOfWork).Build(ArticleID);
            
            switch (TemplateImageID)
            {
                case 1:
                    return PartialView("ArticlePodImages", Model);
                case 2:
                    return PartialView("ArticleSlideshowPartial", Model);
                case 3:
                    return PartialView("ArticleSingleImagePartial", Model);
                default:
                    return Content("");
            }
        }


        [ValidateInput(false)]
        public ActionResult ArticleSearch(
            byte? ArticleTypeID, string NameOrID)
        {
            if (NameOrID == "" || NameOrID == "Name, Code or ID")
                NameOrID = null;

            return PartialView("ArticleSearchDataViewPartial", UnitOfWork.Context.Article_Search(ArticleTypeID, NameOrID).ToList());
        }

        [HttpPost]
        public ActionResult UpdateRelatedArticles(int BaseArticleID, string Articles)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> ArticlesList = ser.Deserialize<List<SortDatum>>(Articles);

            foreach (SortDatum d in ArticlesList)
            {
                Article_RelatedArticle ara = UnitOfWork.GetRepository<Article_RelatedArticle>().GetAll().Where(p => p.ArticleID == BaseArticleID && p.RelatedArticleID == d.ID).SingleOrDefault();
                if (ara == null)
                {
                    ara = new Article_RelatedArticle() { ArticleID = BaseArticleID, RelatedArticleID = Convert.ToInt32(d.ID), SortOrder = Convert.ToByte(d.SortOrder) };
                    UnitOfWork.GetRepository<Article_RelatedArticle>().Add(ara);
                }
                else
                {
                    ara.SortOrder = Convert.ToByte(d.SortOrder);
                }
            }

            UnitOfWork.Save();

            List<vwArticle_RelatedArticle> Model = UnitOfWork.GetRepository<vwArticle_RelatedArticle>().GetAll().Where(p => p.ArticleID == BaseArticleID).OrderBy(p => p.SortOrder).ToList();
            return PartialView("ArticleRelatedArticlesPartial", Model);
        }

        [HttpPost]
        public ActionResult RemoveRelatedArticle(int BaseArticleID, int ArticleID)
        {
            try
            {
                Article_RelatedArticle ara = UnitOfWork.GetRepository<Article_RelatedArticle>().GetAll().Where(p => p.ArticleID == BaseArticleID && p.RelatedArticleID == ArticleID).Single();
                UnitOfWork.GetRepository<Article_RelatedArticle>().Delete(ara);
                UnitOfWork.Save();

                List<vwArticle_RelatedArticle> Model = UnitOfWork.GetRepository<vwArticle_RelatedArticle>().GetAll().Where(p => p.ArticleID == BaseArticleID).OrderBy(p => p.SortOrder).ToList();
                return PartialView("ArticleRelatedArticlesPartial", Model);
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                if (e.Message.ToLower().Contains("sequence"))
                    return Content("That article was already removed from the list.");
                return Content(e.Message);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveArticleText(int ArticleID, string ArticleText)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var modelItem = UnitOfWork.GetRepository<Article>().GetAll().Where(p => p.ArticleID == ArticleID).SingleOrDefault();
                    if (modelItem != null)
                    {
                        modelItem.Contents = ArticleText;
                        UnitOfWork.Save();
                        UnitOfWork.Context.ReplaceLocalURLPlaceholders_Article(ArticleID, null, null, null, null, null, false, false);
                        //UnitOfWork.Context.ReplaceLocalURLPlaceholders_All(null, null, null, null, ArticleID, false, false);
                    }
                }
            }
            catch (Exception e)
            {
                Response.StatusDescription = e.Message;
            }
            var Model = new ArticleViewModelBuilder(UnitOfWork).Build(ArticleID);
            return PartialView("ArticleComposePartial", Model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveArticleVideo(int ArticleID, short VideoID)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var modelItem = UnitOfWork.GetRepository<Article>().GetAll().Where(p => p.ArticleID == ArticleID).SingleOrDefault();
                    if(modelItem != null)
                    {
                        modelItem.VideoID = VideoID;
                        UnitOfWork.Save();
                    }
                }
            }
            catch(Exception e)
            {
                Response.StatusDescription = e.Message;
            }
            var Model = new ArticleViewModelBuilder(UnitOfWork).Build(ArticleID);
            return PartialView("ArticleVideoPartial", Model);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateArticleCountryRestrictions(int ArticleID, string Countries)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> CountryRestrictions = ser.Deserialize<List<SortDatum>>(Countries);

            List<Article_Country> Current = UnitOfWork.GetRepository<Article_Country>().GetAll().Where(p => p.ArticleID == ArticleID).ToList();
            List<SortDatum> CurrentCountries = new List<SortDatum>();
            foreach (Article_Country country in Current)
            {
                CurrentCountries.Add(new SortDatum() { ID = country.CountryID, SortOrder = 0 });
            }
            List<SortDatum> Removals = CurrentCountries.Except(CountryRestrictions).ToList();

            foreach (SortDatum datum in Removals)
            {
                Article_Country remove = UnitOfWork.GetRepository<Article_Country>().GetAll().Where(p => p.ArticleID == ArticleID && p.CountryID == datum.ID).Single();
                UnitOfWork.GetRepository<Article_Country>().Delete(remove);
            }
            UnitOfWork.Save();

            //could probably do deletion from here too: if ProductPage_Country table holds record which isn't in Countries passed to this function, then delete those.

            foreach (SortDatum datum in CountryRestrictions)
            {
                Article_Country bc = UnitOfWork.GetRepository<Article_Country>().GetAll().Where(p => p.ArticleID == ArticleID && p.CountryID == datum.ID).SingleOrDefault();

                if (bc == null)
                {
                    bc = new Article_Country() { ArticleID = ArticleID, CountryID = (short)datum.ID };
                    UnitOfWork.GetRepository<Article_Country>().Add(bc);
                }
            }

            UnitOfWork.Save();

            List<short> Linker = UnitOfWork.GetRepository<Article_Country>().GetAll().Where(p => p.ArticleID == ArticleID).Select(p => p.CountryID).ToList();
            List<Country> Model = UnitOfWork.GetRepository<Country>().GetAll().Where(p => Linker.Contains(p.CountryID)).ToList();
            return PartialView("ArticleCountryRestrictionsPartial", Model);
        }

        #region Slideshow Overlay Settings
        //[HttpPost]
        //public ActionResult SaveSlideshowImageOverlaySettings(int ArticleID, SlideshowImageOverlayViewModel slide)
        //{
        //    try
        //    {
        //        Dictionary<string, string> adjustedModelErrors = new Dictionary<string, string>();

        //        foreach (var p in ModelState)
        //        {
        //            if (p.Value.Errors.Count > 0)
        //            {
        //                foreach (var k in p.Value.Errors)
        //                {
        //                    adjustedModelErrors.Add("SlideShow" + p.Key, k.ErrorMessage);
        //                }
        //            }
        //        }

        //        foreach (KeyValuePair<string, string> kvp in adjustedModelErrors)
        //            ModelState.AddModelError(kvp.Key, kvp.Value);

        //        if (ModelState.IsValid)
        //        {
        //            //var i = UnitOfWork.GetRepository<PageSection_SlideShow>().GetAll().Where(p => p.PageSectionID == PageSectionID).Select(p => p.SlideShowID).Single();
        //            var s = UnitOfWork.GetRepository<Article_Image>().GetAll().Where(p => p.ArticleID == ArticleID && p.ImageID == slide.ImageID).Single();

        //            s.ShowHotspots = slide.ShowHotspots;
        //            s.OverlayTop = slide.OverlayTop;
        //            s.OverlayLeft = slide.OverlayLeft;
        //            s.OverlayWidth = slide.OverlayWidth;
        //            s.OverlayHeight = slide.OverlayHeight;
        //            s.OverlayFontColour = slide.OverlayFontColour;
        //            s.OverlayBackgroundColour = slide.OverlayBackgroundColour;
        //            s.OverlayBackgroundOpacity = slide.OverlayBackgroundOpacity;
        //            s.SlideshowOverlayPositionID = slide.SlideshowOverlayPositionID;
        //            s.AutoHideOverlay = slide.AutoHideOverlay;
        //            s.OverlayHeading = slide.OverlayHeading;
        //            s.OverlayText = slide.OverlayText;
        //            s.DisplayOverlay = slide.DisplayOverlay;
        //            s.PageID = slide.PageID;
        //            s.CollectionID = slide.CollectionID;
        //            s.ProductPageID = slide.ProductPageID;
        //            s.ExternalURL = slide.ExternalURL;
        //            s.NewWindow = slide.NewWindow;

        //            slide.Index = (byte)(s.SortOrder + 1);

        //            UnitOfWork.Save();
        //        }

        //        var arID = "ar" + UnitOfWork.GetRepository<Image>().GetAll().Where(p => p.ImageID == slide.ImageID).Select(p => p.AspectRatioID).Single();

        //        slide.ARID = arID;

        //        slide.ImagesTotal = (byte)UnitOfWork.GetRepository<Article_Image>().GetAll().Where(p => p.ArticleID == ArticleID && p.TemplateImageID == 2).Count();

        //        return PartialView("SlideshowImageOverlayPartial", slide);
        //    }
        //    catch (Exception e)
        //    {
        //        Response.StatusCode = 400;
        //        //log e
        //        return Content(e.Message);
        //    }
        //}

        //public ActionResult SlideshowImageOverlayPartial(int ArticleID, long ImageID)
        //{
        //    return PartialView(new SlideshowImageOverlayViewModelBuilder(UnitOfWork).Build(ArticleID, ImageID));
        //}

        //[HttpPost]
        //public ActionResult MergeSlideshowOverlaySetting(long pageSectionID, SlideshowImageOverlayViewModel slide)
        //{
        //    try
        //    {
        //        Dictionary<string, string> adjustedModelErrors = new Dictionary<string, string>();

        //        foreach (var p in ModelState)
        //        {
        //            if (p.Value.Errors.Count > 0)
        //            {
        //                foreach (var k in p.Value.Errors)
        //                {
        //                    adjustedModelErrors.Add("SlideShow" + pageSectionID.ToString() + p.Key, k.ErrorMessage);
        //                }
        //            }
        //        }

        //        foreach (KeyValuePair<string, string> kvp in adjustedModelErrors)
        //            ModelState.AddModelError(kvp.Key, kvp.Value);

        //        if (ModelState.IsValid)
        //        {
        //            var i = UnitOfWork.GetRepository<PageSection_SlideShow>().GetAll().Where(p => p.PageSectionID == pageSectionID).Select(p => p.SlideShowID).Single();
        //            var s = UnitOfWork.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == i && p.ImageID == slide.ImageID).Single();

        //            s.ShowHotspots = slide.ShowHotspots;
        //            s.OverlayTop = slide.OverlayTop;
        //            s.OverlayLeft = slide.OverlayLeft;
        //            s.OverlayWidth = slide.OverlayWidth;
        //            s.OverlayHeight = slide.OverlayHeight;
        //            s.OverlayFontColour = slide.OverlayFontColour;
        //            s.OverlayBackgroundColour = slide.OverlayBackgroundColour;
        //            s.OverlayBackgroundOpacity = slide.OverlayBackgroundOpacity;
        //            s.SlideshowOverlayPositionID = slide.SlideshowOverlayPositionID;
        //            s.AutoHideOverlay = slide.AutoHideOverlay;
        //            s.OverlayHeading = slide.OverlayHeading;
        //            s.OverlayText = slide.OverlayText;
        //            s.DisplayOverlay = slide.DisplayOverlay;
        //            s.PageID = slide.PageID;
        //            s.CollectionID = slide.CollectionID;
        //            s.ProductPageID = slide.ProductPageID;
        //            s.ExternalURL = slide.ExternalURL;
        //            s.NewWindow = slide.NewWindow;

        //            slide.Index = (byte)(s.SortOrder + 1);

        //            UnitOfWork.Save();

        //            UnitOfWork.Context.Slideshow_Image_MergeOverlays(s.SlideShowID, slide.ImageID);
        //        }
        //        else
        //        {
        //            Response.StatusCode = 400;
        //        }

        //        var arID = "ar" + UnitOfWork.GetRepository<Image>().GetAll().Where(p => p.ImageID == slide.ImageID).Select(p => p.AspectRatioID).Single();

        //        slide.ARID = arID;

        //        slide.ImagesTotal = (byte)UnitOfWork.GetRepository<SlideShow_Image>().GetAll().Where(p => p.SlideShowID == slide.SlideshowID).Count();

        //        long SlideShowID = UnitOfWork.GetRepository<PageSection_SlideShow>().GetAll().Where(p => p.PageSectionID == pageSectionID).Select(p => p.SlideShowID).Single();

        //        return PartialView("SlideshowImageOverlayPartial", slide);
        //    }
        //    catch (Exception e)
        //    {
        //        Response.StatusCode = 400;
        //        //log e
        //        return Content(e.Message);
        //    }
        //}

        //function ShowSlideshowSettings() {
        //    var i = $('#slideshow-advanced-settings').detach();
        //    $('#slideshow-advanced-settings-dialog').html("");
        //    $('#slideshow-advanced-settings-dialog').append(i);
        //    $('#slideshow-advanced-settings').show();
        //    $('#slideshow-advanced-settings-dialog').dialog('open');
        //}

        //function ChangeSlideshowDialogImage(ImageID) {
        //    LoadingPanel.Show();

        //    $.ajax({
        //        url: '@Url.Action("SlideshowImageOverlayPartial", "Articles")',
        //        type: 'GET',
        //        traditional: true,
        //        data: {
        //            ArticleID: $("#selectected-image-id").val(),
        //            imageID: ImageID,
        //            pageSectionID: PageSectionID
        //        },
        //        success: function (data) {
        //            $("#slideshow-settings-overlay-container").html(data);
        //            LoadingPanel.Hide();
        //        },
        //        timeout: 20000
        //    });
        //}

        //function SlideshowDialogNext() {
        //    var imagesList = [];

        //    var nextIndex = -1;
        //    $("#page-section-" + PageSectionID + "-slideshow-images").children("div").each(function (index) {
        //        imagesList.push($(this).data("image-id"));
        //    });

        //    var currentIndex = imagesList.indexOf($("#page-section-" + PageSectionID + "-advanced-settings").find(".slideshow-overlay-display-container").data("image-id"));

        //    nextIndex = currentIndex + 1;

        //    if (nextIndex >= imagesList.length)
        //        nextIndex = 0;

        //    var nextID = imagesList[nextIndex];

        //    ChangeSlideshowDialogImage(nextID, PageSectionID);
        //}

        //function SlideshowDialogPrev() {
        //    var imagesList = [];

        //    var prevIndex = -1;
        //    $("#page-section-" + PageSectionID + "-slideshow-images").children("div").each(function (index) {
        //        imagesList.push($(this).data("image-id"));
        //    });

        //    var currentIndex = imagesList.indexOf($("#page-section-" + PageSectionID + "-advanced-settings").find(".slideshow-overlay-display-container").data("image-id"));

        //    prevIndex = currentIndex - 1;

        //    if (prevIndex < 0)
        //        prevIndex = imagesList.length - 1;

        //    var prevID = imagesList[prevIndex];

        //    ChangeSlideshowDialogImage(prevID, PageSectionID);
        //}

        //$("#slideshow-image-dialog").dialog({
        //    title: "Slideshow Image Details",
        //    autoOpen: false,
        //    height: "auto",
        //    width: "auto",
        //    autoResize: true,
        //    modal: true,
        //    buttons: {
        //        "Save": function () {
        //            LoadingPanel.Show();

        //            var PageID = null;
        //            var ProductPageID = null;
        //            var CollectionID = null;
        //            var ExternalURLValue = null;
        //            var NewWindowValue = null;

        //            if (NewWindow.GetStateInput() != null)
        //                NewWindowValue = NewWindow.GetValue();

        //            if (LinkTypeComboBox.GetValue() == 5 && ExternalURL.GetInputElement() != null)
        //                ExternalURLValue = ExternalURL.GetValue();
        //            else {
        //                if (typeof (MenuBuilderPageSelectorGrid) == 'object') {
        //                    if ((MenuBuilderPageSelectorGrid.GetFocusedRowIndex() != -1 || $('#page-id-hidden').val() != 'nothing'))
        //                        PageID = $('#page-id-hidden').val();

        //                    if ((MenuBuilderProductPageSelectorGrid.GetFocusedRowIndex() != -1 || $('#product-page-id-hidden').val() != 'nothing'))
        //                        ProductPageID = $('#product-page-id-hidden').val();

        //                    if ((MenuBuilderCollectionSelectorGrid.GetFocusedRowIndex() != -1 || $('#collection-id-hidden').val() != 'nothing'))
        //                        CollectionID = $('#collection-id-hidden').val();
        //                }
        //            }

        //            $.ajax({
        //                url: '@Url.Action("SaveSlideshowImageTextDetails", "Articles")',
        //                type: 'POST',
        //                traditional: true,
        //                data: {
        //                    ImageID: $("#slideshow-image-dialog").data("image-id"),
        //                    Heading: SlideshowImageHeading.GetValue(),
        //                    Text: SlideshowImageText.GetValue(),
        //                    PageID: PageID,
        //                    ProductPageID: ProductPageID,
        //                    CollectionID: CollectionID,
        //                    ExternalURL: ExternalURLValue,
        //                    NewWindow: NewWindowValue
        //                },
        //                success: function (data) {
        //                    LoadingPanel.Hide();
        //                    $("#slideshow-image-dialog").dialog("close")
        //                },
        //                error: function (e, s, a) {
        //                    $("#slideshow-image-dialog-inner").html(e.responseText);
        //                    LoadingPanel.Hide();
        //                },
        //                timeout: 20000
        //            });

        //            //var i = $("#page-section-advanced-settings-dialog").children(0).hide();
        //            //var i = $("#page-section-advanced-settings-dialog").children(0).detach();
        //            //$(".content-panel-container[data-page-section-id=" + $(this).data("pagesectionid") + "]").append(i);
        //        },
        //        Cancel: function () {
        //            $(this).dialog("close");
        //        }
        //    }
        //});

        //function SaveSlideshowAdvancedSettings() {
        //    LoadingPanel.Show();
        //    var targ = $("#slideshow-advanced-settings").find(".slideshow-overlay").first();

        //    var col = targ.css("background-color");
        //    col = col.substring(col.indexOf('(') + 1, col.lastIndexOf(')')).split(/,\s*/);
        //    col = col.splice(0, 3);
        //    var rgb = col.join(',');

        //    var fCol = targ.css("color");
        //    fCol = fCol.substring(fCol.indexOf('(') + 1, fCol.lastIndexOf(')')).split(/,\s*/);
        //    var fontRGB = fCol.join(',');

        //    var opa = window["SlideShow" + PageSectionID + "OverlayOpacity"].GetValue();
        //    $(targ).parent().hide();
        //    var w = targ.width();
        //    var h = targ.height();
        //    var t = parseFloat(targ[0].style.top);
        //    var l = parseFloat(targ[0].style.left);
        //    $(targ).parent().show();

        //    var imageID = $("#slideshow-advanced-settings").find(".slideshow-overlay-display-container").first().data("image-id");

        //    $.ajax({
        //        url: '@Url.Action("SaveSlideshowImageOverlaySettings", "CMS")',
        //        type: 'POST',
        //        traditional: true,
        //        data: {
        //            SlideshowID: $("#page-section-" + PageSectionID).data("slideshow-id"),
        //            PageSectionID: PageSectionID,
        //            DisplayOverlay: window["SlideShow" + PageSectionID + "DisplayOverlay"].GetValue(),
        //            ShowHotspots: window["SlideShow" + PageSectionID + "ShowHotspots"].GetValue(),
        //            OverlayTop: t,
        //            OverlayLeft: l,
        //            OverlayWidth: w,
        //            OverlayHeight: h,
        //            OverlayBackgroundColour: rgb,
        //            OverlayBackgroundOpacity: opa,
        //            OverlayFontColour: fontRGB,
        //            SlideshowOverlayPositionID: $('#slideshow-' + PageSectionID + '-overlay-position').val(),
        //            AutoHideOverlay: window["SlideShow" + PageSectionID + "AutoHideOverlay"].GetValue(),
        //            PageID: $("#Slideshow" + PageSectionID + "page-id-hidden").val() == "nothing" ? null : $("#Slideshow" + PageSectionID + "page-id-hidden").val(),
        //            ProductPageID: $("#Slideshow" + PageSectionID + "product-page-id-hidden").val() == "nothing" ? null : $("#Slideshow" + PageSectionID + "product-page-id-hidden").val(),
        //            CollectionID: $("#Slideshow" + PageSectionID + "collection-id-hidden").val() == "nothing" ? null : $("#Slideshow" + PageSectionID + "collection-id-hidden").val(),
        //            ExternalURL: window["Slideshow" + PageSectionID + "ExternalURL"].GetValue(),
        //            NewWindow: window["Slideshow" + PageSectionID + "NewWindow"].GetValue(),
        //            OverlayHeading: window["Slideshow" + PageSectionID + "OverlayHeading"].GetValue(),
        //            OverlayText: window["Slideshow" + PageSectionID + "OverlayText"].GetValue(),
        //            ImageID: imageID
        //        },
        //        success: function (data) {

        //            //alert("Ok - now return the actual damned slideshow.");
        //            LoadingPanel.Hide();
        //            $("#slideshow-image-" + PageSectionID + "-overlay-container").html(data);
        //            //ReloadSectionsForEdit();
        //        },
        //        timeout: 20000
        //    });
        //}

        //function RevealLinkSelector(index, selectedValue, pageSectionID) {
        //    $("#page-section-" + pageSectionID + "-advanced-settings").find(".link-selector").hide();
        //    $("#link-selector-" + index + "-" + pageSectionID).show();
        //    switch (Number(index)) {
        //        case 1:
        //            $("#SlideShow" + pageSectionID + "page-id-hidden").val("nothing");
        //            $("#SlideShow" + pageSectionID + "product-page-id-hidden").val('nothing');
        //            $("#SlideShow" + pageSectionID + "collection-id-hidden").val('nothing');
        //            window["Slideshow" + pageSectionID + "ExternalURL"].SetText('');
        //            window["Slideshow" + pageSectionID + "ExternalURL"].SetValue(null);
        //            $("#link-" + pageSectionID + "-test").hide();
        //            break;
        //        case 2:
        //            window["Slideshow" + pageSectionID + "MenuBuilderPageSelectorGrid"].AdjustControl();
        //            //window.setTimeout(function () {
        //            //    if (selectedValue != null)
        //            //        window["Slideshow" + pageSectionID + "MenuBuilderPageSelectorGrid"].AutoFilterByColumn(0, selectedValue);
        //            //}, 2500);
        //            break;
        //        case 3:
        //            window["Slideshow" + pageSectionID + "MenuBuilderProductPageSelectorGrid"].AdjustControl();
        //            //window.setTimeout(function () {
        //            //    if (selectedValue != null)
        //            //        window["Slideshow" + pageSectionID + "MenuBuilderProductPageSelectorGrid"].AutoFilterByColumn(0, selectedValue);
        //            //}, 2500);
        //            break;
        //        case 4:
        //            window["Slideshow" + pageSectionID + "MenuBuilderCollectionSelectorGrid"].AdjustControl();
        //            //window.setTimeout(function () {
        //            //    if (selectedValue != null)
        //            //        window["Slideshow" + pageSectionID + "MenuBuilderCollectionSelectorGrid"].AutoFilterByColumn(0, selectedValue);
        //            //}, 2500);
        //            break;
        //        default: break;
        //    }
        //}

        //function MergeAcrossSlideshow(PageSectionID) {
        //    var targ = $("#page-section-" + PageSectionID + "-advanced-settings").find(".slideshow-overlay").first();

        //    var col = targ.css("background-color");
        //    col = col.substring(col.indexOf('(') + 1, col.lastIndexOf(')')).split(/,\s*/);
        //    col = col.splice(0, 3);
        //    var rgb = col.join(',');

        //    var fCol = targ.css("color");
        //    fCol = fCol.substring(fCol.indexOf('(') + 1, fCol.lastIndexOf(')')).split(/,\s*/);
        //    var fontRGB = fCol.join(',');

        //    var opa = window["SlideShow" + PageSectionID + "OverlayOpacity"].GetValue();
        //    $(targ).parent().hide();
        //    var w = targ.width();
        //    var h = targ.height();
        //    var t = parseFloat(targ[0].style.top);
        //    var l = parseFloat(targ[0].style.left);
        //    $(targ).parent().show();

        //    var imageID = $("#page-section-" + PageSectionID + "-advanced-settings").find(".slideshow-overlay-display-container").first().data("image-id");

        //    $.ajax({
        //        url: '@Url.Action("MergeSlideshowOverlaySetting", "CMS")',
        //        type: 'POST',
        //        traditional: true,
        //        data: {
        //            SlideshowID: $("#page-section-" + PageSectionID).data("slideshow-id"),
        //            PageSectionID: PageSectionID,
        //            DisplayOverlay: window["SlideShow" + PageSectionID + "DisplayOverlay"].GetValue(),
        //            ShowHotspots: window["SlideShow" + PageSectionID + "ShowHotspots"].GetValue(),
        //            OverlayTop: t,
        //            OverlayLeft: l,
        //            OverlayWidth: w,
        //            OverlayHeight: h,
        //            OverlayBackgroundColour: rgb,
        //            OverlayBackgroundOpacity: opa,
        //            OverlayFontColour: fontRGB,
        //            SlideshowOverlayPositionID: $('#slideshow-' + PageSectionID + '-overlay-position').val(),
        //            AutoHideOverlay: window["SlideShow" + PageSectionID + "AutoHideOverlay"].GetValue(),
        //            PageID: $("#Slideshow" + PageSectionID + "page-id-hidden").val() == "nothing" ? null : $("#Slideshow" + PageSectionID + "page-id-hidden").val(),
        //            ProductPageID: $("#Slideshow" + PageSectionID + "product-page-id-hidden").val() == "nothing" ? null : $("#Slideshow" + PageSectionID + "product-page-id-hidden").val(),
        //            CollectionID: $("#Slideshow" + PageSectionID + "collection-id-hidden").val() == "nothing" ? null : $("#Slideshow" + PageSectionID + "collection-id-hidden").val(),
        //            ExternalURL: window["Slideshow" + PageSectionID + "ExternalURL"].GetValue(),
        //            NewWindow: window["Slideshow" + PageSectionID + "NewWindow"].GetValue(),
        //            OverlayHeading: window["Slideshow" + PageSectionID + "OverlayHeading"].GetValue(),
        //            OverlayText: window["Slideshow" + PageSectionID + "OverlayText"].GetValue(),
        //            ImageID: imageID
        //        },
        //        success: function (data) {
        //            LoadingPanel.Hide();
        //            $("#complete-dialog").dialog('open');
        //        },
        //        error: function (e, s, a) {
        //            LoadingPanel.Hide();
        //            $("#slideshow-image-" + PageSectionID + "-overlay-container").html(e.responseText);
        //        },
        //        timeout: 20000
        //    });
        //}
        #endregion

        Flock.Models.FlockDataContext db = new Flock.Models.FlockDataContext();

        [ValidateInput(false)]
        public ActionResult ArticleGridViewPartial()
        {
            var model = db.Articles;
            return PartialView("_ArticleGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ArticleGridViewPartialAddNew(Flock.Models.Article item)
        {
            var model = db.Articles;
            if (ModelState.IsValid)
            {
                try
                {
                    model.InsertOnSubmit(item);
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_ArticleGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ArticleGridViewPartialUpdate(Flock.Models.Article item)
        {
            var model = db.Articles;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ArticleID == item.ArticleID);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SubmitChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_ArticleGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ArticleGridViewPartialDelete(System.Int32 ArticleID)
        {
            var model = db.Articles;
            if (ArticleID >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ArticleID == ArticleID);
                    if (item != null)
                        model.DeleteOnSubmit(item);
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_ArticleGridViewPartial", model);
        }
    }
}