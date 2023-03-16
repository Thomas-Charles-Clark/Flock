using Flock.DAL;
using Flock.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.ViewModels;
using DevExpress.Web.Mvc;

namespace Flock.Controllers
{
    public class ReportingController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private UnitOfWork<FlockDataContext> UoW = new UnitOfWork<FlockDataContext>();

        public ActionResult Manage()
        {
            var Model = new ReportingManagerViewModel();

            Model.ReportGroups = UoW.GetRepository<ReportGroup>().GetAll().ToList();

            Model.Employees = UoW.GetRepository<Employee>().GetAll().ToList();

            return View(Model);
        }

        public ActionResult ReportGroupGridViewPartial()
        {
            var Model = UoW.GetRepository<ReportGroup>().GetAll().ToList();

            return PartialView("_ReportGroupGridViewPartial", Model);
        }

        public ActionResult ReportGroupGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))]ReportGroup item)
        {
            UoW.GetRepository<ReportGroup>().Add(new ReportGroup() { Name = item.Name });

            UoW.Save();

            var Model = UoW.GetRepository<ReportGroup>().GetAll().ToList();

            return PartialView("_ReportGroupGridViewPartial", Model);
        }

        public ActionResult ReportGroupGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))]ReportGroup item)
        {
            var t = UoW.GetRepository<ReportGroup>().GetAll().Where(p => p.ReportGroupID == item.ReportGroupID).Single();

            t.Name = item.Name;

            UoW.Save();

            var Model = UoW.GetRepository<ReportGroup>().GetAll().ToList();

            return PartialView("_ReportGroupGridViewPartial", Model);
        }

        public ActionResult ReportGroupGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))]ReportGroup item)
        {
            var t = UoW.GetRepository<ReportGroup>().GetAll().Where(p => p.ReportGroupID == item.ReportGroupID).Single();

            UoW.GetRepository<ReportGroup>().Delete(t);

            UoW.Save();

            var Model = UoW.GetRepository<ReportGroup>().GetAll().ToList();

            return PartialView("_ReportGroupGridViewPartial", Model);
        }





        public ActionResult ReportsComboBoxPartial()
        {
            var Model = UoW.GetRepository<Report>().GetAll().ToList();

            return PartialView("_ReportsComboBoxPartial", Model);
        }

        public ActionResult EmployeeListBoxPartial()
        {
            var Model = UoW.GetRepository<Employee>().GetAll().ToList();

            return PartialView("_EmployeeListBoxPartial", Model);
        }

        public ActionResult ReportDetailPartial(Int16 ReportID)
        {
            var Model = new ReportDetailViewModel();

            Model.Report = UoW.GetRepository<Report>().GetAll().Where(p => p.ReportID == ReportID).SingleOrDefault();
            Model.ReportGroups = UoW.GetRepository<ReportGroup>().GetAll().ToList();
            Model.Employees = Model.Report.Report_Employees.Select(p => p.Employee).ToList();

            return PartialView("_ReportDetailsPartial", Model);
        }

        [HttpPost]
        public ActionResult CreateReportPartial()
        {
            var Model = new ReportDetailViewModel();

            Model.ReportGroups = UoW.GetRepository<ReportGroup>().GetAll().ToList();
            
            return PartialView("_ReportDetailsPartial", Model);
        }

        [HttpPost]
        public ActionResult SaveReport(Int16? ReportID, Byte? ReportGroupID, String Name, String Description, List<Int16> EmployeeIDs)
        {
            try
            {
                if (ReportID.HasValue)
                {
                    var r = UoW.GetRepository<Report>().GetAll().Where(p => p.ReportID == ReportID).Single();

                    r.Name = Name;
                    r.ReportGroupID = ReportGroupID.Value;
                    r.Description = Description;

                    foreach (var re in r.Report_Employees)
                        UoW.GetRepository<Report_Employee>().Delete(re);

                    r.Report_Employees.Clear();

                    UoW.Save();

                    foreach (var e in EmployeeIDs)
                        UoW.GetRepository<Report_Employee>().Add(new Report_Employee() { ReportID = ReportID.Value, EmployeeID = e });

                    UoW.Save();
                }
                else
                {
                    var report = new Report();

                    report.Name = Name;
                    report.ReportGroupID = ReportGroupID.Value;
                    report.Description = Description;

                    UoW.GetRepository<Report>().Add(report);

                    UoW.Save();

                    foreach (var e in EmployeeIDs)
                        UoW.GetRepository<Report_Employee>().Add(new Report_Employee() { ReportID = report.ReportID, EmployeeID = e });

                    UoW.Save();

                    ReportID = report.ReportID;
                }

                return RedirectToAction("ReportDetailPartial", new { ReportID = ReportID.Value });  
            }
            catch(Exception ex)
            {
                Response.StatusCode = 500;

                return Content(ex.ToString());
            }
        }
    }
}