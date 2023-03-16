using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using System.Web.Mvc;
using Flock.DAL;
using Flock.ViewModels;

namespace Flock.Controllers
{
    public class CalendarController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            CalendarViewModelBuilder Builder = new CalendarViewModelBuilder(UnitOfWork);

            CalendarViewModel Model = Builder.Build(null);

            return View(Model);
        }

        [AllowAnonymous]
        public ActionResult CalendarData()
        {
            string xmlData = GetCalendarDataAsXmlString(); //This method must return the DB result as xml string
            return Content(xmlData, "text/xml");

            //Flock.Models.EventsDataContext data = new Flock.Models.EventsDataContext();
            //return View(data.Event_Select());
        }

        private string GetCalendarDataAsXmlString()
        {
            var result = "";
            FlockDataContext db = new FlockDataContext();

            var ret = db.CalendarEvent_Select(null);

            foreach (CalendarEvent_SelectResult r in ret)
            {
                result += r.XML_F52E2B61_18A1_11d1_B105_00805F49916B.ToString();
            }

            return result;
        }

        [AllowAnonymous]
        public ActionResult CalendarSave(FormCollection actionValues)
        {
            // need to pull in calendarID

            String action_type = actionValues["!nativeeditor_status"];
            Int64 source_id = Int64.Parse(actionValues["id"]);
            Int64 target_id = source_id;

            FlockDataContext db = new FlockDataContext();
            CalendarEvent myevent;
            try
            {
                switch (action_type)
                {
                    case "inserted":
                        /*
                         * NOT GETTING INTO SQL IN THE CORRECT DATE FORMAT.
                         * 
                         * EVENT_CALENDAR_ID IS NOT BEING PASSED THROUGH THE SAVE FUNCTION
                         */


                        myevent = new CalendarEvent();
                        //DateTime start = 
                        //DateTime corrected = DateTime.Parse(start.ToString("yyyy-mm-dd hh:mm"));
                        myevent.StartDT = DateTime.Parse(actionValues["start_date"]);
                        myevent.CreatedDT = DateTime.Now;
                        //DateTime end = 
                        //corrected = DateTime.Parse(end.ToString("yyyy-mm-dd hh:mm"));
                        myevent.EndDT = DateTime.Parse(actionValues["end_date"]);
                        myevent.Subject = actionValues["text"];
                        myevent.Description = actionValues["description"];
                        myevent.CalendarID = Convert.ToByte(actionValues["calendar_id"]);
                        db.CalendarEvents.InsertOnSubmit(myevent);
                        break;
                    case "deleted":
                        myevent = db.CalendarEvents.SingleOrDefault(ev => ev.CalendarEventID == source_id);
                        db.CalendarEvents.DeleteOnSubmit(myevent);
                        break;
                    default: // "updated"
                        myevent = db.CalendarEvents.SingleOrDefault(ev => ev.CalendarEventID == source_id);
                        myevent.StartDT = DateTime.Parse(actionValues["start_date"]);
                        myevent.EndDT = DateTime.Parse(actionValues["end_date"]);
                        myevent.Subject = actionValues["text"];
                        myevent.Description = actionValues["description"];
                        break;
                }
                db.SubmitChanges();
                target_id = myevent.CalendarEventID;
            }
            catch
            {
                action_type = "error";
            }

            return View(new EventCalendarActionResponseModel(action_type, source_id, target_id));
        }
    }

    public class EventCalendarActionResponseModel
    {
        public String Status;
        public Int64 Source_id;
        public Int64 Target_id;
        public EventCalendarActionResponseModel(String status,
               Int64 source_id, Int64 target_id)
        {
            Status = status;
            Source_id = source_id;
            Target_id = target_id;
        }
    }
}
