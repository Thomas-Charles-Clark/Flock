using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using NLog;

namespace Flock.Controllers
{
    public class DesignsController : Controller
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
            return RedirectToAction("overview");
        }

        public ActionResult Overview()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DesignOverviewGridViewPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult DesignOverviewGridViewPartialCustomCommand(string commandType, string keyString)
        {
            string errorString = "";
            bool hasError = false;

            switch(commandType.ToLower())
            {
                case "delete":
                    List<int> IDs = keyString.Split(',').ToList().ConvertAll(p => Convert.ToInt32(p));

                    List<vwDesignOverview> ToDelete = UnitOfWork.GetRepository<vwDesignOverview>().GetAll().Where(p => IDs.Contains(p.DesignID) && p.ProductAssociations == 0).ToList();

                    List<int> NotDeleted = IDs.Except(ToDelete.Select(p => p.DesignID).ToList()).ToList();

                    if (NotDeleted.Count > 0)
                    {
                        List<vwDesignOverview> NotDeleting = UnitOfWork.GetRepository<vwDesignOverview>().GetAll().Where(p => NotDeleted.Contains(p.DesignID)).ToList();

                        errorString = "The following Designs could not be deleted because they have Products associated to them: \r\n";
                        foreach (short s in NotDeleted)
                        {
                            var pluralisation = "s";

                            if (NotDeleting.Where(p => p.DesignID == s).Select(p => p.ProductAssociations).Single() == 1)
                                pluralisation = "";

                            errorString += "DesignID: " + s.ToString() + " '" + NotDeleting.Where(p => p.DesignID == s).Select(p => p.Name).Single()
                                + "' has " + NotDeleting.Where(p => p.DesignID == s).Select(p => p.ProductAssociations).Single().ToString() + " association"+pluralisation+".\r\n";
                        }
                        hasError = true;
                    }

                    foreach (short s in ToDelete.Select(p => p.DesignID).ToList())
                    {
                        Design d = UnitOfWork.GetRepository<Design>().GetAll().Where(p => p.DesignID == s).Single();
                        UnitOfWork.GetRepository<Design>().Delete(d);
                    }

                    UnitOfWork.Save();
                    break;
                default: break;
            }

            if (hasError)
            {
                Response.StatusCode = 400;
                return Content(errorString);
            }

            return PartialView("DesignOverviewGridViewPartial");
        }

        [HttpPost]
        public ActionResult DesignOverviewDetailGridViewPartial(int DesignID)
        {
            var PreModel = UnitOfWork.GetRepository<Product>().GetAll().Where(p => p.DesignID == DesignID && p.DeletedDT == null).Select(p => p.ProductID).ToList();
            var Model = UnitOfWork.GetRepository<vwProductOverview>().GetAll().Where(p => PreModel.Contains(p.ProductID)).ToList();
            ViewBag.DesignOverviewDetailGridDesignID = DesignID;
            return PartialView(Model);
        }

	}
}