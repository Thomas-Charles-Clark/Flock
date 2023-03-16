using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.ViewModels;
using Flock.Models;
using Flock.DAL;
using DevExpress.Web.Mvc;

namespace Flock.Controllers
{
    public class AnimationsController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private UnitOfWork<FlockDataContext> UoW = new UnitOfWork<FlockDataContext>();

        // GET: Animation
        public ActionResult Index(Int16? AnimationID)
        {
            var Model = new AnimationViewModel();

            return View(Model);
        }

        public ActionResult Edit(Int16 AnimationID)
        {
            var Model = new AnimationDetailModel(UoW, AnimationID);

            return View("_AnimationEditFormPartial", Model);
        }

        [ValidateInput(false)]
        public ActionResult _AnimationSelectorGridViewPartial()
        {
            var model = new AnimationGridModel(UoW);
            return PartialView("_AnimationSelectorGridViewPartial", model);
        }

        public ActionResult _AnimationSelectorGridAddNew(AnimationEditModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UoW.GetRepository<Animation>().Add(new Animation() { 
                        Name = item.Name,
                        AspectRatioID = item.AspectRatioID
                    });

                    UoW.Save();
                }
            }
            catch(Exception ex)
            {
                ViewData["EditError"] = ex.ToString();
            }

            var model = new AnimationGridModel(UoW);

            return PartialView("_AnimationSelectorGridViewPartial", model);
        }

        public ActionResult _AnimationSelectorGridUpdate(AnimationEditModel item)
        {
            try
            {
                var modelItem = UoW.GetRepository<Animation>().GetAll().Where(p => p.AnimationID == item.AnimationID).SingleOrDefault();

                if(modelItem != null)
                {
                    modelItem.Name = item.Name;
                }

                UoW.Save();
            }
            catch (Exception ex)
            {
                ViewData["EditError"] = ex.ToString();
            }

            var model = new AnimationGridModel(UoW);

            return PartialView("_AnimationSelectorGridViewPartial", model);
        }

        public ActionResult _AnimationSelectorGridViewDelete(AnimationEditModel item)
        {
            try
            {
                var modelItem = UoW.GetRepository<Animation>().GetAll().Where(p => p.AnimationID == item.AnimationID).SingleOrDefault();

                if (modelItem != null)
                {
                    modelItem.DeletedDT = DateTime.Now;
                }

                UoW.Save();
            }
            catch(Exception ex)
            {
                ViewData["EditError"] = ex.ToString();
            }

            var model = new AnimationGridModel(UoW);

            return PartialView("_AnimationSelectorGridViewPartial", model);
        }

        public ActionResult AnimationEditFormPartial(int AnimationID)
        {
            var Model = new AnimationDetailModel(UoW, AnimationID);

            return PartialView("_AnimationEditFormPartial", Model);
        }

        public ActionResult _AddSlidePartial(short AnimationID)
        {
            var Model = new SlideEditModel(UoW, AnimationID);

            return PartialView("_AddSlidePartial", Model);
        }

        [HttpPost]
        public ActionResult SaveSlideDetails(SlideEditModel item)
        {
            item.Rebuild(UoW);

            try
            {
                if(ModelState.IsValid) 
                {
                    
                    var modelList = UoW.GetRepository<Slide>().GetAll().Where(p => p.AnimationID == item.AnimationID && p.SlideTypeID != 2).ToList();

                    foreach(var mi in modelList)
                    {
                        mi.DisplayCallsToAction = item.DisplayCallsToAction;
                        mi.AutoHideCallsToAction = item.AutoHideCallsToAction;
                        mi.DisplayHotspots = item.DisplayHotspots;
                        mi.AutoHideHotspots = item.AutoHideHotspots;
                    }

                    var modelItem = UoW.GetRepository<Slide>().GetAll().Where(p => p.SlideID == item.SlideID).Single();

                    modelItem.ArticleID = item.SlideArticleID;
                    modelItem.CollectionID = item.SlideCollectionID;
                    modelItem.PageID = item.SlidePageID;
                    modelItem.ProductPageID = item.SlideProductPageID;
                    modelItem.ExternalURL = item.SlideExternalURL == null ? null : item.SlideExternalURL.ToLowerInvariant().TrimEnd(new[] { '/' });
                    modelItem.NewWindow = item.SlideNewWindow;

                    UoW.Save();

                    return PartialView("_SlideDetailPartial", item);
                }
                else
                {
                    return PartialView("_SlideDetailPartial", item);
                }
            }
            catch(Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult RemoveSlide(short AnimationID, int SlideID)
        {
            try
            {
                var modelItem = UoW.GetRepository<Slide>().GetAll().Where(p => p.SlideID == SlideID).Single();

                modelItem.DeletedDT = DateTime.Now;

                UoW.Save();
            }
            catch(Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.ToString());
            }

            var Model = new AnimationDetailModel(UoW, AnimationID);

            return PartialView("_EditFormPartOne", Model);
        }

        public ActionResult VideoComboBoxPartial()
        {
            var Model = UoW.GetRepository<Video>().GetAll().Where(p => p.DeletedDT == null).OrderByDescending(p => p.VideoID).ToList(); 

            return PartialView("~/Views/Shared/VideoComboBoxPartial.cshtml", Model);
        }

        public ActionResult CallToActionGridViewPartial()
        {
            return PartialView("_CallToActionGridViewPartial");
        }

        public ActionResult LoadSlideDetail(short AnimationID, int? SlideID)
        {
            var Model = new SlideEditModel(UoW, AnimationID, SlideID);

            return PartialView("_SlideDetailPartial", Model);
        }

        [HttpPost]
        public ActionResult AssignCallToActionToSlideArea(short AnimationID, int CallToActionID, int SlideID, Byte AreaID)
        {
            try
            {
                // get all calls to action in this slide, if non extant then create

                var extant = UoW.GetRepository<Slide_CallToAction>().GetAll().Where(p => p.SlideID == SlideID && p.CallToActionID == CallToActionID).SingleOrDefault();

                var sortOrders = UoW.GetRepository<Slide_CallToAction>().GetAll().Where(p => p.SlideID == SlideID && p.SlideAreaID == AreaID).Select(p => p.SortOrder).ToList();
                byte nextSortOrder = 1;

                if (sortOrders.Count > 0)
                {
                    nextSortOrder = sortOrders.Max();
                    nextSortOrder++;
                }

                if (extant == null)
                {
                    UoW.GetRepository<Slide_CallToAction>().Add(new Slide_CallToAction()
                    {
                        CallToActionID = CallToActionID,
                        SlideID = SlideID,
                        SlideAreaID = AreaID,
                        SortOrder = nextSortOrder
                    });

                    UoW.Save();
                }
                else
                {
                    extant.SlideAreaID = AreaID;

                    UoW.Save();
                }
            }
            catch(Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.ToString());
            }

            var Model = new SlideEditModel(UoW, AnimationID, SlideID);

            return PartialView("_SlideDetailPartial", Model);
        }

        [HttpPost]
        public ActionResult RemoveCallToActionFromSlide(short AnimationID, int CallToActionID, int SlideID)
        {
            try
            {
                var extant = UoW.GetRepository<Slide_CallToAction>().GetAll().Where(p => p.SlideID == SlideID && p.CallToActionID == CallToActionID).Single();

                UoW.GetRepository<Slide_CallToAction>().Delete(extant);

                UoW.Save();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.ToString());
            }

            var Model = new SlideEditModel(UoW, AnimationID, SlideID);

            return PartialView("_SlideDetailPartial", Model);
        }

        [HttpPost]
        public ActionResult AssignImageToSlide(short AnimationID, int SlideID, Int64 ImageID)
        {
            try
            {
                var extant = UoW.GetRepository<Slide>().GetAll().Where(p => p.SlideID == SlideID).SingleOrDefault();

                if (extant == null)
                {
                    extant = new Slide()
                    {
                        AnimationID = AnimationID,
                        ImageID = ImageID,
                        SlideTypeID = (byte)AnimationSlideTypes.Image,
                        DisplayHotspots = true,
                        AutoHideHotspots = true,
                        DisplayCallsToAction = true,
                        AutoHideCallsToAction = true,
                        SortOrder = 0,
                        CreatedDT = DateTime.Now
                    };
                }
                else
                {
                    extant.ImageID = ImageID;
                    extant.VideoID = null;
                }

                UoW.Save();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.ToString());
            }
            
            var Model = new AnimationDetailModel(UoW, AnimationID, SlideID);

            return PartialView("_EditFormPartOne", Model);
        }

        [HttpPost]
        public ActionResult AssignVideoToSlide(short AnimationID, int SlideID, short VideoID)
        {
            try
            {
                var extant = UoW.GetRepository<Slide>().GetAll().Where(p => p.SlideID == SlideID).SingleOrDefault();

                if (extant == null)
                {
                    extant = new Slide()
                    {
                        AnimationID = AnimationID,
                        VideoID = VideoID,
                        SlideTypeID = (byte)AnimationSlideTypes.Video,
                        DisplayHotspots = false,
                        AutoHideHotspots = true,
                        DisplayCallsToAction = false,
                        AutoHideCallsToAction = true,
                        SortOrder = 0,
                        CreatedDT = DateTime.Now
                    };
                }
                else
                {
                    extant.ImageID = null;
                    extant.VideoID = VideoID;
                }

                UoW.Save();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.ToString());
            }

            var Model = new AnimationDetailModel(UoW, AnimationID, SlideID);

            return PartialView("_EditFormPartOne", Model);
        }

        [HttpPost]
        public ActionResult UpdateCallToActionSortOrders(passCallToActionSortOrderUpdate objUpdate)
        {
            try 
            {
                foreach(CallToActionDatum datum in objUpdate.Calls)
                {
                    Slide_CallToAction call = UoW.GetRepository<Slide_CallToAction>().GetAll().Where(p => p.CallToActionID == datum.CallToActionID && p.SlideID == objUpdate.SlideID).Single();

                    call.SortOrder = datum.SortOrder;
                }

                UoW.Save();
            }
            catch(Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message);
            }

            return null;
        }

        [HttpPost]
        public ActionResult UpdateSlideSortOrders(passSlideSortOrderUpdate objUpdate)
        {
            var result = new slideSortOrderUpdateReturn() { message = "" };

            Slide newSlidePlaceholder = null;

            try
            {
                foreach (SlideDatum datum in objUpdate.Slides)
                {
                    Slide s = UoW.GetRepository<Slide>().GetAll().Where(p => p.SlideID == datum.SlideID).SingleOrDefault();

                    if (s != null)
                    {
                        s.SortOrder = Convert.ToByte(datum.SortOrder);
                    }
                    else
                    {
                        newSlidePlaceholder = new Slide()
                        {
                            AnimationID = objUpdate.AnimationID,
                            SlideTypeID = datum.VideoID == null ? (byte)1 : (byte)2,
                            ImageID = datum.ImageID,
                            VideoID = datum.VideoID,
                            DisplayHotspots = true,
                            AutoHideHotspots = true,
                            DisplayCallsToAction = datum.VideoID == null ? true : false,
                            AutoHideCallsToAction = true,
                            SortOrder = datum.SortOrder,
                            CreatedDT = DateTime.Now
                        };

                        UoW.GetRepository<Slide>().Add(newSlidePlaceholder);
                    }
                }

                UoW.Save();

                if (objUpdate.SlideToLoad == null)
                {
                    if (newSlidePlaceholder != null)
                    {
                        objUpdate.SlideToLoad = newSlidePlaceholder.SlideID;
                    }
                }
            }
            catch(Exception ex)
            {
                result.message = ex.ToString();
                Response.StatusCode = 500;
                return Json(result);
            }

            var Model = new AnimationDetailModel(UoW, objUpdate.AnimationID, objUpdate.SlideToLoad);

            return PartialView("_EditFormPartOne", Model);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult CallToActionGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))]CallToActionEditModel item)
        {
            item.RebuildLists(UoW);

            if (ModelState.IsValid)
            {
                try
                {
                    UoW.GetRepository<CallToAction>().Add(new CallToAction()
                    {
                        Name = item.Name,
                        Text = item.Text,
                        ArticleID = item.CallToActionArticleID,
                        PageID = item.CallToActionPageID,
                        CollectionID = item.CallToActionCollectionID,
                        ProductPageID = item.CallToActionProductPageID,
                        ExternalURL = item.CallToActionExternalURL == null ? null : item.CallToActionExternalURL.ToLowerInvariant().TrimEnd(new[] { '/' }),
                        StyleID = 1,
                        CallToActionWidthModeID = 1,
                        AppearanceDelay = item.AppearanceDelay,
                        VisibilityDuration = item.VisibilityDuration,
                        CreatedDT = DateTime.Now
                    });

                    UoW.Save();

                    UoW.Context.CallToAction_Language_Initialise();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = "Please, correct all errors.";
                return PartialView("_CallToActionGridViewPartial", item);
            }

            return PartialView("_CallToActionGridViewPartial");
        }

        public ActionResult CallToActionEditFormPartial(int? CallToActionID)
        {
            var Model = new CallToActionEditModel(UoW, CallToActionID);

            return PartialView("_CallToActionEditFormPartial", Model);
        }

        public ActionResult CallToActionArticleIDComboBoxPartial()
        {
            return PartialView("_CallToActionArticleIDComboBoxPartial");
        }

        public ActionResult CallToActionPageIDComboBoxPartial()
        {
            return PartialView("_CallToActionPageIDComboBoxPartial");
        }

        public ActionResult CallToActionCollectionIDComboBoxPartial()
        {
            return PartialView("_CallToActionCollectionIDComboBoxPartial");
        }

        public ActionResult CallToActionProductPageIDComboBoxPartial()
        {
            return PartialView("_CallToActionProductPageIDComboBoxPartial");
        }


        public ActionResult SlideArticleIDComboBoxPartial()
        {
            return PartialView("_SlideArticleIDComboBoxPartial");
        }

        public ActionResult SlidePageIDComboBoxPartial()
        {
            return PartialView("_SlidePageIDComboBoxPartial");
        }

        public ActionResult SlideCollectionIDComboBoxPartial()
        {
            return PartialView("_SlideCollectionIDComboBoxPartial");
        }

        public ActionResult SlideProductPageIDComboBoxPartial()
        {
            return PartialView("_SlideProductPageIDComboBoxPartial");
        }



        public ActionResult StyleIDComboBoxPartial()
        {
            return PartialView("_StyleIDComboBoxPartial");
        }

        public ActionResult WidthModeIDComboBoxPartial()
        {
            return PartialView("_WidthModeIDComboBoxPartial");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CallToActionGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))]CallToActionEditModel item)
        {
            item.RebuildLists(UoW);

            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = UoW.GetRepository<CallToAction>().GetAll().FirstOrDefault(it => it.CallToActionID == item.CallToActionID);
                    if (modelItem != null)
                    {
                        modelItem.Name = item.Name;
                        modelItem.Text = item.Text;
                        modelItem.ArticleID = item.CallToActionArticleID;
                        modelItem.PageID = item.CallToActionPageID;
                        modelItem.CollectionID = item.CallToActionCollectionID;
                        modelItem.ProductPageID = item.CallToActionProductPageID;
                        modelItem.ExternalURL = item.CallToActionExternalURL == null ? null : item.CallToActionExternalURL.ToLowerInvariant().TrimEnd(new[] { '/' }); ;
                        modelItem.NewWindow = item.NewWindow;
                        modelItem.StyleID = item.CallToActionStyleID.Value; // Guaranteed from ModelState.IsValid
                        modelItem.CallToActionWidthModeID = item.CallToActionWidthModeID.Value; // as above
                        modelItem.AppearanceDelay = item.AppearanceDelay;
                        modelItem.VisibilityDuration = item.VisibilityDuration;
                        UoW.Save();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                Response.StatusCode = 400;
                ViewData["EditError"] = "Please, correct all errors.";

                return PartialView("_CallToActionGridViewPartial", item);
            }

            return PartialView("_CallToActionGridViewPartial", item);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CallToActionGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))]CallToActionEditModel item)
        {
            try
            {
                var modelItem = UoW.GetRepository<CallToAction>().GetAll().Where(it => it.CallToActionID == item.CallToActionID).FirstOrDefault();

                if (modelItem != null)
                    modelItem.DeletedDT = DateTime.Now;

                UoW.Save();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return PartialView("_CallToActionGridViewPartial");
        }
    }

    public class slideSortOrderUpdateReturn
    {
        public String message { get; set; }
    }

    public class SlideDatum
    {
        public Int32 SlideID { get; set; }
        public Int64? ImageID { get; set; }
        public Int16? VideoID { get; set; }
        public Byte SortOrder { get; set; }
    }

    public class CallToActionDatum
    {
        public Int32 CallToActionID { get; set; }
        public Byte SortOrder { get; set; }
    }

    public class passSlideSortOrderUpdate
    {
        public short AnimationID { get; set; }

        public Int32? SlideToLoad { get; set; }

        public List<SlideDatum> Slides { get; set; }
    }

    public class passCallToActionSortOrderUpdate
    {
        public short AnimationID { get; set; }

        public Int32 SlideID { get; set; }

        public List<CallToActionDatum> Calls { get; set; }
    }
}