using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using NLog;
using System.Web.Script.Serialization;

namespace Flock.Controllers
{
    public class SelectionsController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        //
        // GET: /Selections/
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Overview()
        {
            return View();
        }
        
        public ActionResult SelectionOverviewGridViewPartial()
        {
            return PartialView();
        }

        public ActionResult SelectionOverviewGridViewAddNew(SelectionOverviewViewModel item)
        {
            if (item.OnDate != null && item.OffDate != null)
            {
                if (item.OnDate >= item.OffDate)
                    ModelState.AddModelError("On Date", "On Date must preceed Off Date when both are set.");
            }
            if (item.OnDate == null && item.OffDate != null)
                ModelState.AddModelError("On Date", "On Date must be set if Off Date is set.");

            if (ModelState.IsValid)
            {
                Selection s = new Selection() {
                    Name = item.Name,
                    Heading = item.Heading,
                    SubHeading = item.SubHeading,
                    URLKeywords = item.URLKeywords,
                    OnDate = item.OnDate,
                    OffDate = item.OffDate,
                    CreatedDT = DateTime.Now,
                    ModifiedDT = DateTime.Now
                };

                UnitOfWork.GetRepository<Selection>().Add(s);
                UnitOfWork.Save();

                return PartialView("SelectionOverviewGridViewPartial");
            }
            else
            {
                return PartialView("SelectionOverviewGridViewPartial", item);
            }
        }

        public ActionResult SelectionOverviewGridViewUpdate(SelectionOverviewViewModel item)
        {
            if (item.OnDate != null && item.OffDate != null)
            {
                if (item.OnDate >= item.OffDate)
                    ModelState.AddModelError("On Date", "On Date must preceed Off Date when both are set.");
            }
            if (item.OnDate == null && item.OffDate != null)
                ModelState.AddModelError("On Date", "On Date must be set if Off Date is set.");

            if (ModelState.IsValid)
            {
                Selection s = UnitOfWork.GetRepository<Selection>().GetAll().Where(p => p.SelectionID == item.SelectionID).Single();
                s.Name = item.Name;
                s.Heading = item.Heading;
                s.SubHeading = item.SubHeading;
                s.URLKeywords = item.URLKeywords;
                s.OnDate = item.OnDate;
                s.OffDate = item.OffDate;
                s.ModifiedDT = DateTime.Now;
                UnitOfWork.Save();
                return PartialView("SelectionOverviewGridViewPartial");
            }
            else
            {
                return PartialView("SelectionOverviewGridViewPartial", item);
            }
        }

        public ActionResult SelectionOverviewGridViewDelete(Selection item)
        {
            Selection s = UnitOfWork.GetRepository<Selection>().GetAll().Where(p => p.SelectionID == item.SelectionID).Single();
            s.DeletedDT = DateTime.Now;
            UnitOfWork.Save();
            return PartialView("SelectionOverviewGridViewPartial");
        }

        public ActionResult SelectionBuilder(short? id)
        {
            var Model = new SelectionComposeViewModel();

            if(id != null)
            {
                Model = new SelectionComposeViewModelBuilder(UnitOfWork).Build(id);
                
                if(Model.SelectionID == null)
                {
                    ViewData["InvalidSelectionIDError"] = "Invalid Selection ID, or the selection is marked as deleted. Deleted selection can be retrieved.";
                }
            }

            return View(Model);
        }

        public ActionResult ComposeSelectionGridSelectorPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ChangeSelection(short ID)
        {
            var Model = new SelectionComposeViewModelBuilder(UnitOfWork).Build(ID);
            return PartialView("SelectionComposePartial", Model);
        }

        [HttpPost]
        public ActionResult SaveSelectionDetails(SelectionComposeViewModel item)
        {
            if (item.OnDate != null && item.OffDate != null)
            {
                if (item.OnDate >= item.OffDate)
                    ModelState.AddModelError("On Date", "On Date must preceed Off Date when both are set.");
            }
            if (item.OnDate == null && item.OffDate != null)
                ModelState.AddModelError("On Date", "On Date must be set if Off Date is set.");
            

            if(ModelState.IsValid)
            {
                Selection s = UnitOfWork.GetRepository<Selection>().GetAll().Where(p => p.SelectionID == item.SelectionID).Single();
                s.Name = item.Name;
                s.Heading = item.Heading;
                s.SubHeading = item.SubHeading;
                s.Description = item.Description;
                s.URLKeywords = item.URLKeywords;
                s.TitleTag = item.TitleTag;
                s.MetaDescription = item.MetaDescription;
                s.ModifiedDT = DateTime.Now;
                s.OnDate = item.OnDate;
                s.OffDate = item.OffDate;

                UnitOfWork.Save();
            }

            item.Images = UnitOfWork.GetRepository<Selection_Image>().GetAll().Where(p => p.SelectionID == item.SelectionID).OrderBy(p => p.SortOrder).ToList();

            return PartialView("SelectionComposePartial", item);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateSelectionImageArea(short SelectionID, string Images, byte TemplateImageID)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                List<SortDatum> ImagesList = ser.Deserialize<List<SortDatum>>(Images);

                foreach (SortDatum datum in ImagesList)
                {
                    Selection_Image si = UnitOfWork.GetRepository<Selection_Image>().GetAll().Where(p => p.SelectionID == SelectionID && p.TemplateImageID == TemplateImageID && p.ImageID == datum.ID).SingleOrDefault();
                    if (si == null)
                    {
                        si = new Selection_Image() { SelectionID = SelectionID, TemplateImageID = TemplateImageID, ImageID = datum.ID };
                        UnitOfWork.GetRepository<Selection_Image>().Add(si);
                    }
                    else
                    {
                        UnitOfWork.GetRepository<Selection_Image>().Delete(si);
                        UnitOfWork.Save();
                        si = new Selection_Image() { SelectionID = SelectionID, TemplateImageID = TemplateImageID, ImageID = datum.ID };
                        UnitOfWork.GetRepository<Selection_Image>().Add(si);
                    }
                    UnitOfWork.Save();
                }
            }
            catch (Exception e)
            {
                Response.StatusDescription = e.Message;
                // log e
            }
            List<Selection_Image> Model = UnitOfWork.GetRepository<Selection_Image>().GetAll().Where(p => p.SelectionID == SelectionID && p.TemplateImageID == TemplateImageID).OrderBy(p => p.SortOrder).ToList();
            switch (TemplateImageID)
            {
                default:
                    return PartialView("SelectionPodImagesPartial", Model);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemoveImageFromSelectionArea(short SelectionID, short TemplateImageID, long ImageID)
        {
            try
            {
                Selection_Image CI = UnitOfWork.GetRepository<Selection_Image>().GetAll().Where(p => p.SelectionID == SelectionID && p.ImageID == ImageID && p.TemplateImageID == TemplateImageID).Single();
                UnitOfWork.GetRepository<Selection_Image>().Delete(CI);
                UnitOfWork.Save();
            }
            catch (Exception e)
            {
                Response.StatusDescription = e.Message;
                // log e
            }

            List<Selection_Image> Model = UnitOfWork.GetRepository<Selection_Image>().GetAll().Where(p => p.SelectionID == SelectionID && p.TemplateImageID == TemplateImageID).OrderBy(p => p.SortOrder).ToList();
            switch (TemplateImageID)
            {
                default:
                    return PartialView("SelectionPodImagesPartial", Model);
            }
        }

    }
}