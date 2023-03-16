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
    public partial class CallsToActionController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private UnitOfWork<FlockDataContext> UoW = new UnitOfWork<FlockDataContext>();

        // GET: CallToAction
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult CallToActionGridViewPartial()
        {
            return PartialView("_CallToActionGridViewPartial");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CallToActionGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))]CallToActionEditModel item)
        {
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

        public ActionResult CallToActionEditFormPartial(int CallToActionID)
        {
            var Model = new CallToActionEditModel(UoW, CallToActionID);

            return PartialView("_CallToActionEditFormPartial", Model);
        }

        public ActionResult ArticleIDComboBoxPartial()
        {
            return PartialView("_ArticleIDComboBoxPartial");
        }

        public ActionResult PageIDComboBoxPartial()
        {
            return PartialView("_PageIDComboBoxPartial");
        }

        public ActionResult CollectionIDComboBoxPartial()
        {
            return PartialView("_CollectionIDComboBoxPartial");
        }

        public ActionResult ProductPageIDComboBoxPartial()
        {
            return PartialView("_ProductPageIDComboBoxPartial");
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
                        modelItem.ExternalURL = item.CallToActionExternalURL == null ? null : item.CallToActionExternalURL.ToLowerInvariant().TrimEnd(new[] { '/' });
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

                item.RebuildLists(UoW);
                
                return PartialView("_CallToActionEditFormPartial", item);
            }

            item.RebuildLists(UoW);

            return PartialView("_CallToActionEditFormPartial", item);
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
}