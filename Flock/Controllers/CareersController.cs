using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.Models;
using Flock.ViewModels;
using Flock.DAL;
using NLog;

namespace Flock.Controllers
{
    public class CareersController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        // GET: Careers
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CareersSelectorGridViewPartial()
        {
            return PartialView();
        }

        public ActionResult CareersSelectorGridEditFormPartial(short? CareerListingID)
        {
            CareerListingViewModel cl = null;

            if (CareerListingID.HasValue)
            {
                var item = UnitOfWork.GetRepository<CareerListing>().GetAll().Where(p => p.CareerListingID == CareerListingID.Value).Single();

                cl = new CareerListingViewModel();

                cl.Title = item.Title;
                cl.Location = item.Location;
                cl.ApplyBeforeDT = (DateTime)item.ApplyBeforeDT;
                cl.Contents = item.Contents;
                cl.ContactEmail = item.ContactEmail;
                cl.ContactPostal = item.ContactPostal;
                cl.SortOrder = item.SortOrder;
                cl.OnDate = item.OnDate;
                cl.OffDate = item.OffDate;
                cl.Salaried = item.Salaried;

            }

            return PartialView("_CareersListingEditFormPartial", cl);
        }

        public ActionResult CareersSelectorGridViewAddNew([ModelBinder(typeof(DevExpressEditorsBinder))]CareerListingViewModel item)
        {
            item.CreatedDT = DateTime.Now;

            if (item.ContactEmail == null && item.ContactPostal == null)
            {
                ModelState.AddModelError("ContactEmail", "Either an Email or a Postal contact method is required.");
                ModelState.AddModelError("ContactPostal", "Either an Email or a Postal contact method is required.");
            }

            if (item.OffDate != null && item.OnDate == null)
            {
                ModelState.AddModelError("OnDate", "Please set an On Date.");
            }
            if (item.OnDate != null && item.OffDate != null)
            {
                if (item.OffDate <= item.OnDate)
                {
                    ModelState.AddModelError("OnDate", "On Date must precede Off Date.");
                    ModelState.AddModelError("OffDate", "Off Date date must not precede On Date.");
                }
            }

            if(ModelState.IsValid)
            {
                var nextSort = (short)(UnitOfWork.GetRepository<CareerListing>().GetAll().Where(p => p.DeletedDT == null).Select(p => p.SortOrder).Max() + 1);

                UnitOfWork.GetRepository<CareerListing>().Add(new CareerListing() { 
                    Title = item.Title,
                    Location = item.Location,
                    ApplyBeforeDT = item.ApplyBeforeDT,
                    Contents = item.Contents,
                    ContactEmail = item.ContactEmail,
                    ContactPostal = item.ContactPostal,
                    SortOrder = nextSort,
                    OnDate = item.OnDate,
                    OffDate = item.OffDate,
                    Salaried = item.Salaried
                });

                UnitOfWork.Context.CareerListing_Sort();

                UnitOfWork.Save();

                UnitOfWork.Context.CareerListing_Language_Initialise();
            }

            return PartialView("CareersSelectorGridViewPartial", item);
        }

        public ActionResult CareersSelectorGridViewUpdate([ModelBinder(typeof(DevExpressEditorsBinder))]CareerListingViewModel item)
        {
            if (item.ContactEmail == null && item.ContactPostal == null)
            {
                ModelState.AddModelError("ContactEmail", "Either an Email or a Postal contact method is required.");
                ModelState.AddModelError("ContactPostal", "Either an Email or a Postal contact method is required.");
            }

            if (item.ContactEmail == null && item.ContactPostal == null)
            {
                ModelState.AddModelError("ContactEmail", "Either an Email or a Postal contact method is required.");
                ModelState.AddModelError("ContactPostal", "Either an Email or a Postal contact method is required.");
            }

            if (item.OffDate != null && item.OnDate == null)
            {
                ModelState.AddModelError("OnDate", "Please set an On Date.");
            }
            if (item.OnDate != null && item.OffDate != null)
            {
                if (item.OffDate <= item.OnDate)
                {
                    ModelState.AddModelError("OnDate", "On Date must precede Off Date.");
                    ModelState.AddModelError("OffDate", "Off Date date must not precede On Date.");
                }
            }

            if (ModelState.IsValid)
            {
                var cl = UnitOfWork.GetRepository<CareerListing>().GetAll().Where(p => p.CareerListingID == item.CareerListingID).Single();
                
                cl.Title = item.Title;
                cl.Location = item.Location;
                cl.ApplyBeforeDT = item.ApplyBeforeDT;
                cl.Contents = item.Contents;
                cl.ContactEmail = item.ContactEmail;
                cl.ContactPostal = item.ContactPostal;
                cl.SortOrder = item.SortOrder;
                cl.OnDate = item.OnDate;
                cl.OffDate = item.OffDate;
                cl.Salaried = item.Salaried;

                UnitOfWork.Save();
            }

            return PartialView("CareersSelectorGridViewPartial", item);
        }

        public ActionResult CareersSelectorGridViewDelete(Int16 CareerListingID)
        {
            var cl = UnitOfWork.GetRepository<CareerListing>().GetAll().Where(p => p.CareerListingID == CareerListingID).Single();

            cl.DeletedDT = DateTime.Now;

            UnitOfWork.Save();
                
            return PartialView("CareersSelectorGridViewPartial");
        }

        [HttpPost]
        public ActionResult CareersSelectorGridViewCustomCommand(string CommandType, short CareerListingID, byte DraggingSI, byte TargetSI)
        {
             try
            {
                switch (CommandType)
                {
                    case "DragRow":
                        var t = UnitOfWork.GetRepository<CareerListing>().GetAll().Where(p => p.DeletedDT == null).ToList();
                        if (t.Count > 0)
                        {
                            if (TargetSI == UnitOfWork.GetRepository<CareerListing>().GetAll().Where(p => p.DeletedDT == null).Select(p => p.SortOrder).Max())
                                UnitOfWork.Context.CareerListing_Move(CareerListingID, TargetSI + 0.5M);
                            else
                                UnitOfWork.Context.CareerListing_Move(CareerListingID, TargetSI - 0.5M);
                            //UnitOfWork.Context.CareerListing_Sort();
                        }
                        else
                        {
                            UnitOfWork.Context.CareerListing_Move(CareerListingID, TargetSI);
                            //UnitOfWork.Context.CareerListing_Sort();
                        }
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
             {
                 Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.Message);
            }

            return PartialView("CareersSelectorGridViewPartial");
        }
    }
}