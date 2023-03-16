using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;

namespace Flock.Controllers
{
    public class FormsController : Controller
    {
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        [AllowAnonymous]
        public ActionResult Compose(long id)
        {
            FormViewModelBuilder FVMBuilder = new FormViewModelBuilder(UnitOfWork);

            FormViewModel Model = FVMBuilder.Build(id);

            return View(Model);
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            FormGridViewModelBuilder Builder = new FormGridViewModelBuilder(UnitOfWork);

            FormGridViewModel Model = Builder.Build(null);

            return View(Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public PartialViewResult ReloadForm(long FormID)
        {
            FormViewModelBuilder Builder = new FormViewModelBuilder(UnitOfWork);
            FormViewModel Model = Builder.Build(FormID);

            return PartialView("FormComposerPartial", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SaveForm(long? FormID, string InternalName, string Title, string Description, byte FormType, long? MarketingPartnerID, string MarketingPartnerText)
        {
            FormViewModelBuilder Builder = new FormViewModelBuilder(UnitOfWork);

            Builder.Update((long)FormID, InternalName, Title, Description, FormType, MarketingPartnerID, MarketingPartnerText);

            return Content("Not Implemented");
        }

        [AllowAnonymous]
        [HttpPost]
        public PartialViewResult ProcessCompetitionForm(CompetitionFormViewModel model)
        {
            // check model.MarketingOptIn -> true == save for this affiliate as well as us, false == don't save for them but do save for us
            
            if (ModelState.IsValid && DevExpress.Web.Mvc.CaptchaExtension.GetIsValid("captcha"))
                return PartialView("CompetitionFormDisplayPartial", model);
            else
                return PartialView("CompetitionFormDisplayPartial", model);
        }

        [AllowAnonymous]
        [HttpPost]
        public PartialViewResult ProcessNewsletterForm(NewsletterFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                    return PartialView("NewsletterFormDisplayPartial", model);
            }
            return PartialView("NewsletterFormDisplayPartial", model);
        }

        [AllowAnonymous]
        public PartialViewResult CompetitionFormDisplayPartial(long FormID)
        {
            FormViewModelBuilder Builder = new FormViewModelBuilder(UnitOfWork);
            
            CompetitionFormViewModel CPVM = (CompetitionFormViewModel)Builder.Build(FormID);
            
            return PartialView(CPVM);
        }

        [AllowAnonymous]
        public PartialViewResult NewsletterFormDisplayPartial(long FormID)
        {
            FormViewModelBuilder Builder = new FormViewModelBuilder(UnitOfWork);

            NewsletterFormViewModel NFVM = (NewsletterFormViewModel)Builder.Build(FormID);

            return PartialView(NFVM);
        }

        [ValidateInput(false)]
        public ActionResult FormGridViewPartial()
        {
            FormGridViewModelBuilder Builder = new FormGridViewModelBuilder(UnitOfWork);

            FormGridViewModel Model = Builder.Build(null);

            return PartialView("_FormGridViewPartial", Model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult FormGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Flock.Models.Form item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    item.FormType = 2; // default to creating Competition Forms - if they want new newsletter forms, let them change the blasted type on the compose page.
                                        // worry about market specificity
                    UnitOfWork.GetRepository<Form>().Add(item);
                    UnitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            FormGridViewModelBuilder Builder = new FormGridViewModelBuilder(UnitOfWork);

            FormGridViewModel Model = Builder.Build(null);

            return PartialView("_FormGridViewPartial", Model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult FormGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Flock.Models.Form item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_FormGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult FormGridViewPartialDelete(System.Int64 FormID)
        {
            var model = new object[0];
   
                try
                {
                    // Insert here a code to delete the item from your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }

            return PartialView("_FormGridViewPartial", model);
        }

        public ActionResult CaptchaPartial()
        {
            return PartialView("_CaptchaPartial");
        }
    }
}
