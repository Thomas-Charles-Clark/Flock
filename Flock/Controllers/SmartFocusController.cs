using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.Classes.SmartFocus;
using NLog;
using System.Threading.Tasks;
using Flock.ViewModels;
using Flock.DAL;
using Flock.Models;

namespace Flock.Controllers
{
    public class SmartFocusController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private SmartFocusManager SmartFocus = new SmartFocusManager();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        public ActionResult Index()
        {
            return RedirectToAction("overview");
        }

        public ActionResult Overview()
        {
            // WHY am I getting the 405 - Method Not Allowed when trying to update the criteria on this segment...
            string result = SmartFocus.AddTestCriteriaToTestAPISegment().Result;

            return View();
        }

        public ActionResult SmartFocusTestGroupPartial()
        {
            SmartFocusTestViewModelBuilder Builder = new SmartFocusTestViewModelBuilder(SmartFocus);
            SmartFocusTestViewModel Model = Builder.Build().Result;// ConfigureAwait(false);

            return PartialView(Model);
        }

        [HttpPost]
        public ActionResult UpdateTemplateField(int templateID, string field, string value)
        {
            string result = SmartFocus.UpdateTemplateField(templateID, field, value).Result;

            return Content(result);
        }

        [HttpPost]
        public ActionResult UpdateTestMemberEmail(string ExtantEmail, string NewEmail)
        {
            string UpdateResult = SmartFocus.UpdateMemberEmailByEmail(ExtantEmail, NewEmail).Result;

            ViewBag.UpdateResult = UpdateResult;

            SmartFocusTestViewModelBuilder Builder = new SmartFocusTestViewModelBuilder(SmartFocus);
            SmartFocusTestViewModel Model = Builder.Build().Result;// ConfigureAwait(false);

            return PartialView("SmartFocusTestGroupPartial", Model);
        }

        [HttpPost]
        public ActionResult UploadCSV()
        {
            string UploadingResult = SmartFocus.UploadAndMerge().Result;

            SmartFocusTestViewModelBuilder Builder = new SmartFocusTestViewModelBuilder(SmartFocus);
            SmartFocusTestViewModel Model = Builder.Build().Result;//.ConfigureAwait(false);

            ViewBag.UploadingResult = UploadingResult;

            return PartialView("SmartFocusTestGroupPartial", Model);
        }

        [HttpPost]
        public ActionResult GetDistinctMemberCountForSegment(long SegmentID)
        {
            return Content(SmartFocus.GetSegmentDistinctMemberCount(SegmentID).Result.ToString());
        }

        [HttpPost]
        public ActionResult CreateSegment(string Name)
        {
            string CreateResult = SmartFocus.CreateSegment(Name).Result;

            SmartFocusTestViewModelBuilder Builder = new SmartFocusTestViewModelBuilder(SmartFocus);
            SmartFocusTestViewModel Model = Builder.Build().Result;//.ConfigureAwait(false);

            ViewBag.CreateSegmentResult = CreateResult;

            return PartialView("SmartFocusTestGroupPartial", Model);
        }

        [HttpPost]
        public ActionResult CreateTestGroup(string Name)
        {
            string CreateResult = SmartFocus.CreateTestGroup(Name).Result;

            SmartFocusTestViewModelBuilder Builder = new SmartFocusTestViewModelBuilder(SmartFocus);
            SmartFocusTestViewModel Model = Builder.Build().Result;//.ConfigureAwait(false);

            ViewBag.CreateTestGroupResult = CreateResult;

            return PartialView("SmartFocusTestGroupPartial", Model);
        }

        [HttpPost]
        public ActionResult CreateTestMember(string Email)
        {
            TestMember CreateResult = SmartFocus.CreateTestMember(Email).Result;

            SmartFocusTestViewModelBuilder Builder = new SmartFocusTestViewModelBuilder(SmartFocus);
            SmartFocusTestViewModel Model = Builder.Build().Result;//.ConfigureAwait(false);

            ViewBag.NewTestMember = CreateResult;

            return PartialView("SmartFocusTestGroupPartial", Model);
        }

        [HttpPost]
        public ActionResult AddTestMemberToTestGroup(long TestMemberID, long TestGroupID)
        {
            string AddResult = SmartFocus.AddTestMemberToTestGroup(TestMemberID, TestGroupID).Result;

            SmartFocusTestViewModelBuilder Builder = new SmartFocusTestViewModelBuilder(SmartFocus);
            SmartFocusTestViewModel Model = Builder.Build().Result;//.ConfigureAwait(false);

            ViewBag.AddResult = AddResult;

            return PartialView("SmartFocusTestGroupPartial", Model);
        }




        //////////////////////
        //
        //  Search
        //
        //////////////////////
        public ActionResult FetchMemberByEmail(string Email)
        {
            string result = SmartFocus.FetchMemberByEmail(Email).Result;

            return PartialView("SmartFocusMemberDetailPartial", result);
        }



        //////////////////////
        //
        //  Synchronisation
        //
        //////////////////////

        public ActionResult Synchronisation()
        {
            return View();
        }


        //////////////////////
        //
        //  Transactional
        //
        //////////////////////

        public ActionResult Transactional()
        {
            return View();
        }

        public ActionResult TransactionalEmailTemplates()
        {
            SFTransactionalTemplateOverviewViewModelBuilder Builder = new SFTransactionalTemplateOverviewViewModelBuilder(SmartFocus);
            SFTransactionalTemplateOverviewViewModel Model = Builder.Build().Result;
            return View(Model);
        }

        [HttpPost]
        public ActionResult CreateNewTransactionalTemplate(SFTransactionalTemplate item)
        {
            if(ModelState.IsValid)
            {
                var result = SmartFocus.CreateTransactionalTemplate(item).Result;
            }

            var Model = SmartFocus.GetLastTransactionalTemplates().Result;

            return PartialView("TemplatesListPartial", Model);
        }

        [HttpPost]
        public ActionResult CreateNewDynamicContentBlock(SFDynamicContentBlock item)
        {
            if(ModelState.IsValid)
            {
                var result = SmartFocus.CreateDynamicContentBlock(item).Result;

                var Model = SmartFocus.GetLastDynamicContentBlocks().Result;

                return PartialView("DynamicContentBlockListPartial", Model);
            }
            else
            {
                Response.StatusCode = 500;
                return PartialView("CreateDCBFormPartial", item);
            }
        }

        [HttpPost]
        public ActionResult SendTestEmail(long EmailID, long MemberID)
        {
            string Result = SmartFocus.SendTestEmailToMember(EmailID, MemberID).Result;

            return Content(Result);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        //
        //
        //
        //

        public ActionResult EmailText()
        {
            return View("EmailText/Index");
        }

        public ActionResult EmailTextGridViewPartial()
        {
            return PartialView("EmailText/_EmailTextGridViewPartial");
        }

        public ActionResult EmailTextGridViewEditFormPartial(short? EmailTextID)
        {
            EmailTextViewModel etvm = null;

            if(EmailTextID.HasValue)
            {
                var item = UnitOfWork.GetRepository<EmailText>().GetAll().Where(p => p.EmailTextID == EmailTextID.Value && p.DeletedDT == null).SingleOrDefault();

                if (item != null)
                {
                    etvm = new EmailTextViewModel();

                    etvm.EmailTextID = item.EmailTextID;
                    etvm.EmailTemplateID = item.EmailTemplateID;
                    etvm.Text = item.Text;
                }
            }

            return PartialView("EmailText/_EmailTextGridViewEditFormPartial", etvm);
        }

        public ActionResult EmailTextGridViewAddNew([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))]EmailTextViewModel item)
        {
            if(ModelState.IsValid)
            {
                UnitOfWork.GetRepository<EmailText>().Add(new EmailText() { 
                    EmailTemplateID = item.EmailTemplateID.Value,
                    Text = item.Text,
                    ModifiedDT = DateTime.Now,
                    CreatedDT = DateTime.Now,
                    DeletedDT = null
                });

                UnitOfWork.Save();

                UnitOfWork.Context.EmailText_Language_Initialise();
            }

            return PartialView("EmailText/_EmailTextGridViewPartial", item);
        }

        public ActionResult EmailTextGridViewUpdate([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))]EmailTextViewModel item)
        {
            if (ModelState.IsValid)
            {
                var et = UnitOfWork.GetRepository<EmailText>().GetAll().Where(p => p.EmailTextID == item.EmailTextID).SingleOrDefault();

                if(et != null)
                {
                    et.EmailTemplateID = item.EmailTemplateID.Value;
                    et.Text = item.Text;
                    et.ModifiedDT = DateTime.Now;
                    et.CreatedDT = DateTime.Now;
                    et.DeletedDT = null;
                }

                UnitOfWork.Save();
            }

            return PartialView("EmailText/_EmailTextGridViewPartial", item);
        }

        public ActionResult EmailTextGridViewDelete(short emailTextID)
        {
            if(ModelState.IsValid)
            {
                var et = UnitOfWork.GetRepository<EmailText>().GetAll().Where(p => p.EmailTextID == emailTextID).SingleOrDefault();

                if (et != null)
                {
                    et.DeletedDT = DateTime.Now;
                }

                UnitOfWork.Save();
            }

            return PartialView("EmailText/_EmailTextGridViewPartial");
        }
	}
}