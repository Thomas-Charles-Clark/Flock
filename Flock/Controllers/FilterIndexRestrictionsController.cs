using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using Flock.ViewModels;
using Flock.Models;
using NLog;
using DevExpress.Web.Mvc;

namespace Flock.Controllers
{
    public class FilterIndexRestrictionsController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            return View("FilterIndexRestrictions", new FilterIndexRestrictionsViewModel(1));
        }

        public ActionResult FilterIndexRestrictionsTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<FilterIndexRestriction_Language>().GetAll().OrderBy(p => p.LanguageID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID)).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("~/Views/Translations/Shared/_TargetLanguageDropDown.cshtml", Model);
        }

        public ActionResult GridViewPartial(byte? targetLanguageId)
        {
            return PartialView("_FilterIndexRestrictionsGridView", new FilterIndexRestrictionsGridSettings(targetLanguageId));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] FilterIndexRestriction_LanguageEditModel item)
        {
            var db = new FlockDataContext();

            var model = db.FilterIndexRestriction_Languages;

            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.FilterIndexRestrictionID == item.FilterIndexRestrictionID && it.LanguageID == item.LanguageID);

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

            return PartialView("_FilterIndexRestrictionsGridView", new FilterIndexRestrictionsGridSettings(item.LanguageID));
        }
    }
}