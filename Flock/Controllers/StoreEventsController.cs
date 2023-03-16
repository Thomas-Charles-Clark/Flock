using Flock.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Flock.Models;
using NLog;
using Flock.DAL;

namespace Flock.Controllers
{
    public class StoreEventsController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        public ActionResult Index()
        {
            return View("StoreEvents", new BaseViewModel());
        }

        [ValidateInput(false)]
        public ActionResult StoreEventGridViewPartial()
        {
            return PartialView("_StoreEventGridViewPartial");
        }

        public ActionResult StoreEventGridEditFormPartial(short? StoreEventID)
        {
            StoreEventViewModel se = new StoreEventViewModel();

            if (StoreEventID.HasValue)
            {
                var item = UnitOfWork.GetRepository<StoreEvent>().GetAll().Where(p => p.StoreEventID == StoreEventID.Value).Single();

                se = new StoreEventViewModel();

                se.Name = item.Name;
                se.SelectedStoreEventLocations = item.StoreEvent_StoreEventLocations.ToList();
            }

            se.StoreEventLocations = UnitOfWork.GetRepository<StoreEventLocation>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();

            return PartialView("_StoreEventGridEditFormPartial", se);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult StoreEventGridViewPartialAddNew([ModelBinderAttribute(typeof(DevExpressEditorsBinder))]StoreEventViewModel item, short[] selectedStoreEventLocationIDs)
        {
            if (selectedStoreEventLocationIDs == null)
                ModelState.AddModelError("StoreEventLocationsListBox", "You must select at least one location.");

            if (ModelState.IsValid)
            {
                try
                {
                    var se = new StoreEvent();
                    se.Name = item.Name;

                    UnitOfWork.GetRepository<StoreEvent>().Add(se);
                    UnitOfWork.Save();

                    foreach (var s in selectedStoreEventLocationIDs)
                    {
                        UnitOfWork.GetRepository<StoreEvent_StoreEventLocation>().Add(new StoreEvent_StoreEventLocation() { StoreEventID = se.StoreEventID, StoreEventLocationID = s });
                    }

                    UnitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = "Please, correct all errors.";

            }

            item.StoreEventLocations = UnitOfWork.GetRepository<StoreEventLocation>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
            
            return PartialView("_StoreEventGridViewPartial", item);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StoreEventGridViewPartialUpdate([ModelBinderAttribute(typeof(DevExpressEditorsBinder))]StoreEventViewModel item, short[] selectedStoreEventLocationIDs)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(selectedStoreEventLocationIDs == null) // can be null with none selected
                        selectedStoreEventLocationIDs = new short[0];

                    var modelItem = UnitOfWork.GetRepository<StoreEvent>().GetAll().Where(it => it.StoreEventID == item.StoreEventID).FirstOrDefault();

                    if (modelItem != null)
                    {
                        modelItem.Name = item.Name;

                        var newLocations = selectedStoreEventLocationIDs.Except(modelItem.StoreEvent_StoreEventLocations.Select(p => p.StoreEventLocationID)).ToList();
                        var removeLocations = modelItem.StoreEvent_StoreEventLocations.Select(p => p.StoreEventLocationID).Except(selectedStoreEventLocationIDs).ToList();

                        foreach (var sl in newLocations)
                        {
                            UnitOfWork.GetRepository<StoreEvent_StoreEventLocation>().Add(new StoreEvent_StoreEventLocation() { StoreEventLocationID = sl, StoreEventID = modelItem.StoreEventID });
                        }
                        foreach (var sl in removeLocations)
                        {
                            var t = modelItem.StoreEvent_StoreEventLocations.Where(p => p.StoreEventLocationID == sl).SingleOrDefault();

                            if (t != null)
                            {
                                UnitOfWork.GetRepository<StoreEvent_StoreEventLocation>().Delete(t);
                            }
                        }

                        UnitOfWork.Save();
                    }

                    return PartialView("_StoreEventGridViewPartial", item);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;

                    return PartialView("_StoreEventGridViewPartial", item);
                }
            }
            else
            {
                ViewData["EditError"] = "Please, correct all errors.";


                return PartialView("_StoreEventGridViewPartial", item);
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StoreEventGridViewPartialDelete(System.Int16 StoreEventID)
        {
            try
            {
                var item = UnitOfWork.GetRepository<StoreEvent>().GetAll().Where(it => it.StoreEventID == StoreEventID).FirstOrDefault();
                if (item != null)
                    item.DeletedDT = DateTime.Now;
                UnitOfWork.Save();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return PartialView("_StoreEventGridViewPartial");
        }

        [HttpPost]
        public ActionResult LoadStoreEventDetailPartial(Int16 StoreEventID)
        {
            var item = UnitOfWork.GetRepository<StoreEvent>().GetAll().Where(p => p.StoreEventID == StoreEventID).Single();

            var Model = new StoreEventViewModel();

            Model.StoreEventID = StoreEventID;
            Model.Name = item.Name;
            Model.StoreEventLocations = UnitOfWork.GetRepository<StoreEventLocation>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
            Model.SelectedStoreEventLocations = item.StoreEvent_StoreEventLocations.ToList();

            return PartialView("_StoreEventDetailPartial", Model);
        }

        public ActionResult StoreEventLocationCalendarPartial(Int16 StoreEventID, Int16 StoreEventLocationID)
        {
            var item = UnitOfWork.GetRepository<StoreEvent>().GetAll().Where(p => p.StoreEventID == StoreEventID).Single();
            
            var Model = new StoreEventLocationCalendarViewModel()
            {
                StoreEventID = StoreEventID,
                StoreEventLocationID = StoreEventLocationID,
                SelectedDates = item.StoreEvent_StoreEventLocations.Where(p => p.StoreEventLocationID == StoreEventLocationID).Single().StoreEvent_Dates.ToList()
            };

            return PartialView("_StoreEventLocationCalendarPartial", Model);
        }

        [HttpPost]
        public ActionResult AddDaysToStoreEventLocation(Int16 StoreEventID, Int16 StoreEventLocationID, DateTime[] SelectedDates)
        {
            if (SelectedDates == null)
                SelectedDates = new DateTime[0];
            var item = UnitOfWork.GetRepository<StoreEvent>().GetAll().Where(p => p.StoreEventID == StoreEventID).Single();
            var storeEvent_storeEventLocationID = item.StoreEvent_StoreEventLocations.Where(p => p.StoreEventLocationID == StoreEventLocationID).Select(p => p.StoreEvent_StoreEventLocationID).Single();
            var newDates = SelectedDates
                                .Except(
                                    item.StoreEvent_StoreEventLocations
                                        .Where(p => p.StoreEventLocationID == StoreEventLocationID).Single()
                                            .StoreEvent_Dates.Select(p => p.EventDate))
                                .ToList();

            foreach (var sl in newDates)
            {
                item.StoreEvent_StoreEventLocations.Where(p => p.StoreEvent_StoreEventLocationID == storeEvent_storeEventLocationID).Single().StoreEvent_Dates.Add(new StoreEvent_Date() { 
                    StoreEvent_StoreEventLocationID = storeEvent_storeEventLocationID,
                    EventDate = sl
                });
            }

            UnitOfWork.Save();

            var Model = new StoreEventViewModel();
            Model.StoreEventID = StoreEventID;
            Model.Name = item.Name;
            Model.StoreEventLocations = UnitOfWork.GetRepository<StoreEventLocation>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
            Model.SelectedStoreEventLocations = item.StoreEvent_StoreEventLocations.ToList();

            return PartialView("_StoreEventDetailPartial", Model);
        }

        [HttpPost]
        public ActionResult removeDaysFromStoreEventLocation(Int16 StoreEventID, Int16 StoreEventLocationID, DateTime[] SelectedDates)
        {
            if (SelectedDates == null)
                SelectedDates = new DateTime[0];
            var item = UnitOfWork.GetRepository<StoreEvent>().GetAll().Where(p => p.StoreEventID == StoreEventID).Single();
            var storeEvent_storeEventLocationID = item.StoreEvent_StoreEventLocations.Where(p => p.StoreEventLocationID == StoreEventLocationID).Select(p => p.StoreEvent_StoreEventLocationID).Single();
            var removalDates = item.StoreEvent_StoreEventLocations
                                    .Where(p => p.StoreEventLocationID == StoreEventLocationID).Single().StoreEvent_Dates
                                    .Where(p => SelectedDates.Select(q => q.Date).Contains(p.EventDate.Date)).ToList();

            UnitOfWork.GetRepository<StoreEvent_Date>().DeleteAll(removalDates);

            //foreach (var sl in removalDates)
            //{
            //    item.StoreEvent_StoreEventLocations
            //        .Where(p => p.StoreEvent_StoreEventLocationID == storeEvent_storeEventLocationID)
            //        .Single().StoreEvent_Dates.Remove(sl);
            //}

            UnitOfWork.Save();

            var Model = new StoreEventViewModel();
            Model.StoreEventID = StoreEventID;
            Model.Name = item.Name;
            Model.StoreEventLocations = UnitOfWork.GetRepository<StoreEventLocation>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
            Model.SelectedStoreEventLocations = item.StoreEvent_StoreEventLocations.ToList();

            return PartialView("_StoreEventDetailPartial", Model);
        }

        [HttpPost]
        public ActionResult RemoveDayFromStoreEventLocation(Int16 StoreEventID, Int16 StoreEventLocationID, Int32 StoreEvent_DateID)
        {
            //delete from dbo.StoreEvent_Date_TimeSlot where ID == ID
            try
            {
                UnitOfWork.GetRepository<StoreEvent_Date_TimeSlot>().DeleteAll(UnitOfWork.GetRepository<StoreEvent_Date_TimeSlot>().GetAll().Where(p => p.StoreEvent_DateID == StoreEvent_DateID).ToList());

                var thisDate = UnitOfWork.GetRepository<StoreEvent_Date>().GetAll().Where(p => p.StoreEvent_DateID == StoreEvent_DateID).SingleOrDefault();

                if (thisDate != null)
                    UnitOfWork.GetRepository<StoreEvent_Date>().Delete(thisDate);

                UnitOfWork.Save();
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }

            var item = UnitOfWork.GetRepository<StoreEvent>().GetAll().Where(p => p.StoreEventID == StoreEventID).Single();
             
            var Model = new StoreEventViewModel();
            Model.StoreEventID = StoreEventID;
            Model.Name = item.Name;
            Model.StoreEventLocations = UnitOfWork.GetRepository<StoreEventLocation>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.Name).ToList();
            Model.SelectedStoreEventLocations = item.StoreEvent_StoreEventLocations.ToList();
            
            return PartialView("_StoreEventDetailPartial", Model);
        }

        [HttpPost]
        public ActionResult EventDateTimePartial(Int16 StoreEventID, Int16 StoreEventLocationID, Int32 StoreEvent_DateID)
        {
            var item = UnitOfWork.GetRepository<StoreEvent_Date>().GetAll().Where(p => p.StoreEvent_DateID == StoreEvent_DateID).SingleOrDefault();

            var Model = new StoreEvent_Date_ViewModel();
            
            Model.TimeSlots = new List<StoreEvent_Date_TimeSlot>();

            if(item != null)
            {
                Model.TimeSlots = item.StoreEvent_Date_TimeSlots.ToList();
            }

            Model.StoreEventID = StoreEventID;
            Model.StoreEventLocationID = StoreEventLocationID;
            Model.StoreEvent_DateID = StoreEvent_DateID;

            return PartialView("_EventDateTimePartial", Model);
        }

        [ValidateInput(false)]
        public ActionResult StoreEventDateTimeSlotGridViewPartial(Int32 StoreEvent_DateID)
        {
            var modelItem = UnitOfWork.GetRepository<StoreEvent_Date>().GetAll().Where(p => p.StoreEvent_DateID == StoreEvent_DateID).SingleOrDefault();

            var Model = new StoreEvent_Date_ViewModel();

            Model.TimeSlots = new List<StoreEvent_Date_TimeSlot>();

            if (modelItem != null)
            {
                Model.TimeSlots = modelItem.StoreEvent_Date_TimeSlots.ToList();
            }

            Model.StoreEvent_DateID = StoreEvent_DateID;

            return PartialView("_StoreEventDateTimeSlotGridViewPartial", Model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StoreEventDateTimeSlotGridViewPartialAddNew(Flock.Models.StoreEvent_Date_TimeSlot item, Int32 StoreEvent_DateID)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    UnitOfWork.GetRepository<StoreEvent_Date_TimeSlot>().Add(new StoreEvent_Date_TimeSlot() { StoreEvent_DateID = item.StoreEvent_DateID, TimeSlot = item.TimeSlot });
                    UnitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var modelItem = UnitOfWork.GetRepository<StoreEvent_Date>().GetAll().Where(p => p.StoreEvent_DateID == StoreEvent_DateID).SingleOrDefault();

            var Model = new StoreEvent_Date_ViewModel();

            Model.TimeSlots = new List<StoreEvent_Date_TimeSlot>();

            if (modelItem != null)
            {
                Model.TimeSlots = modelItem.StoreEvent_Date_TimeSlots.ToList();
            }

            Model.StoreEvent_DateID = StoreEvent_DateID;

            return PartialView("_StoreEventDateTimeSlotGridViewPartial", Model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult StoreEventDateTimeSlotGridViewPartialUpdate(Flock.Models.StoreEvent_Date_TimeSlot item, Int32 StoreEvent_DateID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var thisTimeSlot = UnitOfWork.GetRepository<StoreEvent_Date_TimeSlot>().GetAll().Where(p => p.StoreEvent_Date_TimeSlotID == item.StoreEvent_Date_TimeSlotID).SingleOrDefault();
                    if (thisTimeSlot != null)
                    {
                        thisTimeSlot.TimeSlot = item.TimeSlot;
                        UnitOfWork.Save();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var modelItem = UnitOfWork.GetRepository<StoreEvent_Date>().GetAll().Where(p => p.StoreEvent_DateID == StoreEvent_DateID).SingleOrDefault();

            var Model = new StoreEvent_Date_ViewModel();

            Model.TimeSlots = new List<StoreEvent_Date_TimeSlot>();

            if (modelItem != null)
            {
                Model.TimeSlots = modelItem.StoreEvent_Date_TimeSlots.ToList();
            }

            Model.StoreEvent_DateID = StoreEvent_DateID;

            return PartialView("_StoreEventDateTimeSlotGridViewPartial", Model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StoreEventDateTimeSlotGridViewPartialDelete(System.Int64 StoreEvent_Date_TimeSlotID, Int32 StoreEvent_DateID)
        {
            if (StoreEvent_Date_TimeSlotID >= 0)
            {
                try
                {
                    var thisTimeSlot = UnitOfWork.GetRepository<StoreEvent_Date_TimeSlot>().GetAll().Where(p => p.StoreEvent_Date_TimeSlotID == StoreEvent_Date_TimeSlotID).SingleOrDefault();
                    if (thisTimeSlot != null)
                    {
                        UnitOfWork.GetRepository<StoreEvent_Date_TimeSlot>().Delete(thisTimeSlot);
                        UnitOfWork.Save();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            var modelItem = UnitOfWork.GetRepository<StoreEvent_Date>().GetAll().Where(p => p.StoreEvent_DateID == StoreEvent_DateID).SingleOrDefault();

            var Model = new StoreEvent_Date_ViewModel();

            Model.TimeSlots = new List<StoreEvent_Date_TimeSlot>();

            if (modelItem != null)
            {
                Model.TimeSlots = modelItem.StoreEvent_Date_TimeSlots.ToList();
            }

            Model.StoreEvent_DateID = StoreEvent_DateID;

            return PartialView("_StoreEventDateTimeSlotGridViewPartial", Model);
        }

        [HttpPost]
        public ActionResult CopyTimeSlotsToSelectedDates(CopyTimeSlotRequestObject objRequest)
        {
            var result = new CopyTimeSlotResult() { Message = "", Success = false };

            try
            {
                var modelItem = UnitOfWork.GetRepository<StoreEvent_Date>().GetAll().Where(p => p.StoreEvent_DateID == objRequest.StoreEvent_DateID).SingleOrDefault();

                var toUpdate = UnitOfWork.GetRepository<StoreEvent_Date>().GetAll().Where(p => p.StoreEvent_DateID != objRequest.StoreEvent_DateID && objRequest.TargetDateIDs.Contains(p.StoreEvent_DateID)).ToList();

                foreach(var u in toUpdate)
                {
                    UnitOfWork.GetRepository<StoreEvent_Date_TimeSlot>().DeleteAll(u.StoreEvent_Date_TimeSlots);
                }

                UnitOfWork.Save();

                foreach(var c in modelItem.StoreEvent_Date_TimeSlots)
                {
                    foreach(var u in toUpdate)
                    {
                        UnitOfWork.GetRepository<StoreEvent_Date_TimeSlot>().Add(new StoreEvent_Date_TimeSlot()
                        {
                            StoreEvent_DateID = u.StoreEvent_DateID,
                            TimeSlot = c.TimeSlot
                        });
                    }
                }

                UnitOfWork.Save();

                result.Message = "Done";
                result.Success = true;
            }
            catch(Exception ex)
            {
                result.Message = ex.ToString();
            }

            return Json(result);
        }
    }

    public class CopyTimeSlotRequestObject
    {
        public Int32 StoreEvent_DateID { get; set; }
        public Int32[] TargetDateIDs { get; set; }
    }

    public class CopyTimeSlotResult
    {
        public String Message { get; set; }
        public Boolean Success { get; set; }
    }
}