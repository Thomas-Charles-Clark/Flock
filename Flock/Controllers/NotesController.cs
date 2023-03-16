using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.ViewModels;
using NLog;

namespace Flock.Controllers
{
    public class NotesController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private UnitOfWork<FlockDataContext> UoW = new UnitOfWork<FlockDataContext>();

        // GET: Weekly
        public ActionResult Weekly()
        {
            return View("Weekly/Index");
        }

        public ActionResult WeeklyPriceListDropDownPartial()
        {
            var Model = UoW.GetRepository<PriceList>().GetAll().ToList();//UoW.GetRepository<Week>().GetAll().ToList();

            return PartialView("~/Views/Shared/SearchFilters/PriceListDropDownPartial.cshtml", Model);
        }

        public ActionResult WeekBeginningDateEdit()
        {
            return PartialView("Weekly/_WeekBeginningDateEdit");
        }

        [HttpPost]
        public ActionResult GetWeeklyNotes(Byte PriceListID, DateTime WeekCommencing)
        {
            var Model = new WeeklyNotesViewModel();

            var w = UoW.GetRepository<Week>().GetAll().Where(p => p.PriceListID == PriceListID && p.WeekCommencing == WeekCommencing).SingleOrDefault();

            Model.FoundRecord = (w != null);

            if(w != null)
            {
                Model.PriceListID = w.PriceListID;
                Model.Year = w.Year;
                Model.Week = w.Week1;
                Model.Objectives = w.Objectives;
                Model.Observations = w.Observations;
            }

            return PartialView("Weekly/_WeeklyNotesPartial", Model);
        }

        [HttpPost]
        public ActionResult SaveWeeklyNotes(WeeklyNotesViewModel item)
        {
            try
            {
                var w = UoW.GetRepository<Week>().GetAll().Where(p => p.Week1 == item.Week && p.Year == item.Year && p.PriceListID == item.PriceListID).Single();

                w.Objectives = item.Objectives;
                w.Observations = item.Observations;

                UoW.Save();
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                Response.StatusCode = 500;
                return Content("failure");
            }

            return Content("ok");
        }
    }
}