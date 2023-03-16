using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using NLog;
using System.Web.Script.Serialization;

namespace Flock.Controllers
{
    public class eCommerceController : Controller
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
            return RedirectToAction("overview", "ecommerce");
        }

        public ActionResult Overview()
        {
            MembersOverviewViewModelBuilder Builder = new MembersOverviewViewModelBuilder(UnitOfWork);

            MembersOverviewViewModel Model = Builder.Build(null);

            return View(Model);
        }

        public ActionResult Reviews()
        {
            return View();
        }

        public ActionResult GiftVouchers(short? id)
        {
            if (id != null)
            {
                GiftVoucherViewModel GVVM = null;
                if (UnitOfWork.GetRepository<GiftVoucher>().GetAll().Where(p => p.GiftVoucherID == id).SingleOrDefault() != null)
                {
                    GiftVoucherViewModelBuilder Builder = new GiftVoucherViewModelBuilder(UnitOfWork);
                    GVVM = Builder.Build(id);
                }
                else
                {
                    GVVM = new GiftVoucherViewModel();
                    ViewData["InvalidGiftVoucherIDError"] = "Invalid Gift Voucher ID.";
                }
                return View("GiftVouchers", GVVM);
            }
            else
            {
                GiftVoucherViewModel GVVM = new GiftVoucherViewModel();
                return View("GiftVouchers", GVVM);
            }
        }

        public ActionResult GiftVoucherGridViewPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ChangeGiftVoucher(short? GiftVoucherID)
        {
            var Model = new GiftVoucherViewModelBuilder(UnitOfWork).Build(GiftVoucherID);

            return PartialView("GiftVoucherComposePartial", Model);
        }

        public ActionResult GiftVoucherGridViewPartialAddNew(GiftVoucherViewModel item)
        {
            item.CreatedDT = DateTime.Now;
            item.PriceListID = 1;

            if (ModelState.IsValid)
            {
                GiftVoucher gv = new GiftVoucher();
                gv.PriceListID = item.PriceListID;
                gv.Code = item.Code;
                gv.Value = item.Value;

                UnitOfWork.GetRepository<GiftVoucher>().Add(gv);
                UnitOfWork.Save();
            }

            return PartialView("GiftVoucherGridViewPartial");
        }

        [HttpPost]
        public ActionResult SaveGiftVoucherDetails(GiftVoucherViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    GiftVoucher gv = UnitOfWork.GetRepository<GiftVoucher>().GetAll().Where(p => p.GiftVoucherID == item.GiftVoucherID).Single();
                    gv.PriceListID = item.PriceListID;
                    gv.Value = item.Value;
                    gv.Code = item.Code;

                    UnitOfWork.Save();
                }
                catch (Exception e)
                {
                    Logger.ErrorException("", e);
                    ViewData["EditError"] = e.Message;
                    Response.StatusCode = 400;
                }
            }
            else
            {
                Response.StatusCode = 400;
                ViewData["EditError"] = "Please, correct all errors.";
            }

            GiftVoucherViewModelBuilder Builder = new GiftVoucherViewModelBuilder(UnitOfWork);
            GiftVoucherViewModel Model = Builder.Build(item.GiftVoucherID);

            return PartialView("GiftVoucherComposePartial", Model);
        }

        #region DeliveryOptions (REDACTED)
        //public ActionResult DeliveryOptions(short? id)
        //{
        //    if (id != null)
        //    {
        //        DeliveryOptionViewModel DOVM = null;
        //        if (UnitOfWork.GetRepository<Delivery>().GetAll().Where(p => p.DeliveryID == id && p.DeletedDT == null).SingleOrDefault() != null)
        //        {
        //            DeliveryOptionViewModelBuilder Builder = new DeliveryOptionViewModelBuilder(UnitOfWork);
        //            DOVM = Builder.Build(id);
        //        }
        //        else
        //        {
        //            DOVM = new DeliveryOptionViewModel();
        //            ViewData["InvalidDeliveryIDError"] = "Invalid Delivery Option ID, or the delivery option is marked as deleted. Deleted delivery options can be retrieved.";
        //        }
        //        return View(DOVM);
        //    }
        //    else
        //    {
        //        DeliveryOptionViewModel DOVM = new DeliveryOptionViewModel();
        //        return View(DOVM);
        //    }
        //}

        //public ActionResult ChangeDeliveryOption(short DeliveryID)
        //{
        //    var Model = new DeliveryOptionViewModelBuilder(UnitOfWork).Build(DeliveryID);

        //    return PartialView("DeliveryOptionComposePartial", Model);
        //}

        //public ActionResult DeliveryOptionGridViewPartial()
        //{
        //    return PartialView("DeliveryOptionGridViewPartial");
        //}

        //public ActionResult DeliveryOptionGridViewAddNew(DeliveryOptionViewModel item)
        //{
        //    bool hasError = false;

        //    if (item.OffDate != null && item.OnDate == null)
        //    {
        //        hasError = true;
        //        ModelState.AddModelError("OnDate", "Please set an On Date.");
        //    }
        //    if (item.OnDate != null && item.OffDate != null)
        //    {
        //        if (item.OffDate <= item.OnDate)
        //        {
        //            hasError = true;
        //            ModelState.AddModelError("OnDate", "On Date must precede Off Date.");
        //            ModelState.AddModelError("OffDate", "Off Date date must not precede On Date.");
        //        }
        //    }

        //    if(item.MinThreshold != null && item.MaxThreshold != null)
        //    {
        //        if (item.MinThreshold < 0)
        //            ModelState.AddModelError("MinThreshold", "Min Threshold must not be negative");
        //        if (item.MaxThreshold < 0)
        //            ModelState.AddModelError("MaxThreshold", "Max Threshold must not be negative");
        //        if(item.MinThreshold >= item.MaxThreshold)
        //        {
        //            hasError = true;
        //            ModelState.AddModelError("MinThreshold", "Min Threshold must be less than Max Threshold");
        //            ModelState.AddModelError("MaxThreshold", "Max Threshold must be greater than Min Threshold");
        //        }
        //    }

        //    if (!hasError)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                Delivery d = new Delivery();
        //                d.DeliveryServiceID = item.DeliveryServiceID;
        //                d.Name = item.Name;
        //                d.HAULCODE = item.HaulCode;
        //                d.CARRCODE = item.CarrCode;
        //                d.PostCodeRestrictions = item.PostCodeRestrictions;
        //                d.MinThreshold = item.MinThreshold;
        //                d.MaxThreshold = item.MaxThreshold;
        //                d.OnDate = item.OnDate;
        //                d.OffDate = item.OffDate;
        //                d.PricePerItem = item.PricePerItem;
        //                d.PricePerDispatch = item.PricePerDispatch;

        //                UnitOfWork.GetRepository<Delivery>().Add(d);

        //                UnitOfWork.Save();
        //            }
        //            catch (Exception e)
        //            {
        //                Logger.ErrorException("", e);
        //                ViewData["EditError"] = e.Message;
        //            }
        //        }
        //        else
        //        {
        //            ViewData["EditError"] = "Please, correct all errors.";
        //        }
        //    }
        //    else
        //    {
        //        ViewData["EditError"] = "Please, correct all errors.";
        //    }

        //    return PartialView("DeliveryOptionGridViewPartial");
        //}

        //public ActionResult DeliveryOptionGridViewDelete()
        //{
        //    return PartialView("DeliveryOptionGridViewPartial");
        //}

        //public ActionResult DeliveryOptionGridViewPartialCustomCommand(string commandType, string keyString)
        //{
        //    string errorString = "";
        //    bool hasError = false;
        //    List<short> keys = new List<short>();
        //    if (keyString != null && keyString != "")
        //        keys = keyString.Split(',').ToList().ConvertAll(d => Convert.ToInt16(d));

        //    switch (commandType.ToLower())
        //    {
        //        case "delete":
        //            try
        //            {
        //                List<Delivery> toDelete = UnitOfWork.GetRepository<Delivery>().GetAll().Where(p => keys.Contains(p.DeliveryID)).ToList();
        //                foreach (Delivery d in toDelete)
        //                    d.DeletedDT = DateTime.Now;
        //                UnitOfWork.Save();
        //            }
        //            catch (Exception e)
        //            {
        //                ViewData["EditError"] = e.Message;
        //                // log e
        //                hasError = true;
        //            }
        //            break;
        //        default:
        //            break;
        //    }

        //    if (hasError)
        //    {
        //        Response.StatusCode = 400;
        //        return Content(errorString);
        //    }

        //    return PartialView("DeliveryOptionGridViewPartial");
        //}

        //[HttpPost]
        //public ActionResult UpdateDeliveryOptionCountryRestrictions(short DeliveryID, string Countries)
        //{
        //    JavaScriptSerializer ser = new JavaScriptSerializer();
        //    List<SortDatum> CountryRestrictions = ser.Deserialize<List<SortDatum>>(Countries);

        //    List<Delivery_Country> Current = UnitOfWork.GetRepository<Delivery_Country>().GetAll().Where(p => p.DeliveryID == DeliveryID).ToList();
        //    List<SortDatum> CurrentCountries = new List<SortDatum>();
        //    foreach (Delivery_Country country in Current)
        //    {
        //        CurrentCountries.Add(new SortDatum() { ID = country.CountryID, SortOrder = 0 });
        //    }
        //    List<SortDatum> Removals = CurrentCountries.Except(CountryRestrictions).ToList();

        //    foreach (SortDatum datum in Removals)
        //    {
        //        Delivery_Country remove = UnitOfWork.GetRepository<Delivery_Country>().GetAll().Where(p => p.DeliveryID == DeliveryID && p.CountryID == datum.ID).Single();
        //        UnitOfWork.GetRepository<Delivery_Country>().Delete(remove);
        //    }
        //    UnitOfWork.Save();

        //    //could probably do deletion from here too: if ProductPage_Country table holds record which isn't in Countries passed to this function, then delete those.

        //    foreach (SortDatum datum in CountryRestrictions)
        //    {
        //        Delivery_Country dc = UnitOfWork.GetRepository<Delivery_Country>().GetAll().Where(p => p.DeliveryID == DeliveryID && p.CountryID == datum.ID).SingleOrDefault();

        //        if (dc == null)
        //        {
        //            dc = new Delivery_Country() { DeliveryID = DeliveryID, CountryID = (short)datum.ID };
        //            UnitOfWork.GetRepository<Delivery_Country>().Add(dc);
        //        }
        //    }

        //    UnitOfWork.Save();

        //    List<short> Linker = UnitOfWork.GetRepository<Delivery_Country>().GetAll().Where(p => p.DeliveryID == DeliveryID).Select(p => p.CountryID).ToList();
        //    List<Country> Model = UnitOfWork.GetRepository<Country>().GetAll().Where(p => Linker.Contains(p.CountryID)).ToList();
        //    return PartialView("DeliveryOptionCountryRestrictionsPartial", Model);
        //}

        //[HttpPost]
        //public ActionResult SaveDeliveryOptionDetails(DeliveryOptionViewModel item)
        //{
        //    bool hasError = false;

        //    if (item.OffDate != null && item.OnDate == null)
        //    {
        //        hasError = true;
        //        ModelState.AddModelError("OnDate", "Please set an On Date.");
        //    }
        //    if (item.OnDate != null && item.OffDate != null)
        //    {
        //        if (item.OffDate <= item.OnDate)
        //        {
        //            hasError = true;
        //            ModelState.AddModelError("OnDate", "On Date must precede Off Date.");
        //            ModelState.AddModelError("OffDate", "Off Date date must not precede On Date.");
        //        }
        //    }

        //    if(item.MinThreshold != null && item.MaxThreshold != null)
        //    {
        //        if (item.MinThreshold < 0)
        //            ModelState.AddModelError("MinThreshold", "Min Threshold must not be negative");
        //        if (item.MaxThreshold < 0)
        //            ModelState.AddModelError("MaxThreshold", "Max Threshold must not be negative");
        //        if(item.MinThreshold >= item.MaxThreshold)
        //        {
        //            hasError = true;
        //            ModelState.AddModelError("MinThreshold", "Min Threshold must be less than Max Threshold");
        //            ModelState.AddModelError("MaxThreshold", "Max Threshold must be greater than Min Threshold");
        //        }
        //    }

        //    if (!hasError)
        //    {

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                Delivery d = UnitOfWork.GetRepository<Delivery>().GetAll().Where(p => p.DeliveryID == item.DeliveryID).Single();
        //                d.DeliveryServiceID = item.DeliveryServiceID;
        //                d.Name = item.Name;
        //                d.HAULCODE = item.HaulCode;
        //                d.CARRCODE = item.CarrCode;
        //                d.PostCodeRestrictions = item.PostCodeRestrictions;
        //                d.MinThreshold = item.MinThreshold;
        //                d.MaxThreshold = item.MaxThreshold;
        //                d.PricePerItem = item.PricePerItem;
        //                d.PricePerDispatch = item.PricePerDispatch;
        //                d.OnDate = item.OnDate;
        //                d.OffDate = item.OffDate;
        //                UnitOfWork.Save();
        //            }
        //            catch (Exception e)
        //            {
        //                Logger.ErrorException("", e);
        //                ViewData["EditError"] = e.Message;
        //                Response.StatusCode = 400;
        //            }
        //        }
        //        else
        //        {
        //            Response.StatusCode = 400;
        //            ViewData["EditError"] = "Please, correct all errors.";
        //        }
        //    }
        //    else
        //    {
        //        Response.StatusCode = 400;
        //        ViewData["EditError"] = "Please, correct all errors.";
        //    }

        //    var Linker = UnitOfWork.GetRepository<Delivery_ProductType>().GetAll().Where(p => p.DeliveryID == item.DeliveryID).Select(p => p.ProductTypeID).ToList();
        //    item.AssignedProductTypes = UnitOfWork.GetRepository<ProductType>().GetAll().Where(p => Linker.Contains(p.ProductTypeID)).ToList();

        //    return PartialView("DeliveryOptionComposePartial", item);
        //}

        //[HttpPost]
        //public ActionResult UpdateDeliveryOptionProductTypes(short DeliveryID, string ProductTypeIDs)
        //{
        //    List<byte> Current = UnitOfWork.GetRepository<Delivery_ProductType>().GetAll().Where(p => p.DeliveryID == DeliveryID).Select(p => p.ProductTypeID).ToList();
        //    List<byte> InputProductTypes = new List<byte>();
        //    if (ProductTypeIDs != "")
        //        InputProductTypes = ProductTypeIDs.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToList();

        //    List<byte> Removals = Current.Except(InputProductTypes).ToList();

        //    foreach (var i in Removals)
        //    {
        //        Delivery_ProductType Removal = UnitOfWork.GetRepository<Delivery_ProductType>().GetAll().Where(p => p.DeliveryID == DeliveryID && p.ProductTypeID == i).Single();
        //        UnitOfWork.GetRepository<Delivery_ProductType>().Delete(Removal);
        //    }
        //    UnitOfWork.Save();

        //    foreach (var i in InputProductTypes)
        //    {
        //        Delivery_ProductType apt = UnitOfWork.GetRepository<Delivery_ProductType>().GetAll().Where(p => p.DeliveryID == DeliveryID && p.ProductTypeID == i).SingleOrDefault();

        //        if (apt == null)
        //        {
        //            apt = new Delivery_ProductType() { DeliveryID = DeliveryID, ProductTypeID = i };
        //            UnitOfWork.GetRepository<Delivery_ProductType>().Add(apt);
        //        }
        //    }
        //    UnitOfWork.Save();

        //    var Model = new DeliveryOptionViewModelBuilder(UnitOfWork).Build(DeliveryID);

        //    return PartialView("DeliveryOptionComposePartial", Model);
        //}

        //[HttpPost]
        //public ActionResult RemoveProductTypeFromDeliveryOption(short DeliveryID, byte ProductTypeID)
        //{
        //    try
        //    {
        //        Delivery_ProductType AGPT = UnitOfWork.GetRepository<Delivery_ProductType>().GetAll().Where(p => p.DeliveryID == DeliveryID && p.ProductTypeID == ProductTypeID).Single();
        //        UnitOfWork.GetRepository<Delivery_ProductType>().Delete(AGPT);

        //        UnitOfWork.Save();

        //        var Model = new DeliveryOptionViewModelBuilder(UnitOfWork).Build(DeliveryID);

        //        return PartialView("DeliveryOptionComposePartial", Model);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        return Content(ex.ToString());
        //    }
        //}

        //[HttpPost]
        //public ActionResult AddAllProductTypesToDeliveryOption(short DeliveryID)
        //{
        //    List<byte> ProductTypes = UnitOfWork.GetRepository<ProductType>().GetAll().Select(p => p.ProductTypeID).ToList();

        //    foreach (byte s in ProductTypes)
        //    {
        //        var t = UnitOfWork.GetRepository<Delivery_ProductType>().GetAll().Where(p => p.DeliveryID == DeliveryID && p.ProductTypeID == s).SingleOrDefault();
        //        if (t == null)
        //        {
        //            t = new Delivery_ProductType() { DeliveryID = DeliveryID, ProductTypeID = s };
        //            UnitOfWork.GetRepository<Delivery_ProductType>().Add(t);
        //        }
        //    }
        //    UnitOfWork.Save();


        //    var Model = new DeliveryOptionViewModelBuilder(UnitOfWork).Build(DeliveryID);

        //    return PartialView("DeliveryOptionComposePartial", Model);
        //}

        //[HttpPost]
        //public ActionResult RemoveAllProductTypesFromDeliveryOption(short DeliveryID)
        //{
        //    List<Delivery_ProductType> DeliveryPTs = UnitOfWork.GetRepository<Delivery_ProductType>().GetAll().Where(p => p.DeliveryID == DeliveryID).ToList();

        //    foreach (var s in DeliveryPTs)
        //    {
        //        UnitOfWork.GetRepository<Delivery_ProductType>().Delete(s);
        //    }
        //    UnitOfWork.Save();

        //    var Model = new DeliveryOptionViewModelBuilder(UnitOfWork).Build(DeliveryID);

        //    return PartialView("DeliveryOptionComposePartial", Model);
        //}

        //[HttpGet]
        //public ActionResult DeliveryOptionStoredProcedureTest()
        //{


        //    return Content("");
        //}
        #endregion

        public ActionResult DeliveryZones(short? id)
        {
            if (id != null)
            {
                DeliveryZoneViewModel DZVM = null;
                if (UnitOfWork.GetRepository<DeliveryZone>().GetAll().Where(p => p.DeliveryZoneID == id).SingleOrDefault() != null)
                {
                    DeliveryZoneViewModelBuilder Builder = new DeliveryZoneViewModelBuilder(UnitOfWork);
                    DZVM = Builder.Build(id);
                }
                else
                {
                    DZVM = new DeliveryZoneViewModel();
                    ViewData["InvalidDeliveryZoneIDError"] = "Invalid Delivery Zone ID.";
                }
                return View(DZVM);
            }
            else
            {
                DeliveryZoneViewModel DZVM = new DeliveryZoneViewModel();
                return View(DZVM);
            }
        }
        
        [HttpPost]
        public ActionResult ChangeDeliveryZone(short DeliveryZoneID)
        {
            var Model = new DeliveryZoneViewModelBuilder(UnitOfWork).Build(DeliveryZoneID);

            return PartialView("DeliveryZoneComposePartial", Model);
        }

        public ActionResult DeliveryZoneGridViewPartial()
        {
            return PartialView();
        }

        public ActionResult DeliveryZoneGridViewPartialAddNew(DeliveryZoneViewModel item)
        {
            try 
            { 
                if(ModelState.IsValid)
                {
                    DeliveryZone dz = new DeliveryZone() { Name = item.Name, CountryID = item.CountryID, CourierID = item.CourierID };

                    UnitOfWork.GetRepository<DeliveryZone>().Add(dz);
                    UnitOfWork.Save();
                }
            }
            catch (Exception e)
            {
                Logger.ErrorException("", e);
                Response.StatusCode = 400;
                return Content(e.ToString());
            }
            return PartialView("DeliveryZoneGridViewPartial");
        }

        [HttpPost]
        public ActionResult SaveDeliveryZoneDetails(DeliveryZoneViewModel item, string Postcodes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var PostcodeList = new JavaScriptSerializer().Deserialize<List<PostcodeDatum>>(Postcodes);

                    var hasError = false;

                    for (int i = 0; i < PostcodeList.Count; i++)
                    {
                        if (PostcodeList[i].Exemption == null && PostcodeList[i].Restriction == null)
                        {
                            ModelState.AddModelError("DeliveryZone_Postcodes["+i.ToString()+"].PostcodeRestriction", "Provide either a Restriction or an Exemption");
                            ModelState.AddModelError("DeliveryZone_Postcodes[" + i.ToString() + "].PostcodeExemption", "Provide either a Restriction or an Exemption");
                            hasError = true;
                        }

                        if (PostcodeList[i].Exemption != null && PostcodeList[i].Restriction != null)
                        {
                            ModelState.AddModelError("DeliveryZone_Postcodes[" + i.ToString() + "].PostcodeRestriction", "Provide either a Restriction or an Exemption, not both");
                            ModelState.AddModelError("DeliveryZone_Postcodes[" + i.ToString() + "].PostcodeExemption", "Provide either a Restriction or an Exemption, not both");
                            hasError = true;
                        }
                    }

                    var Model = new DeliveryZoneViewModelBuilder(UnitOfWork).Build(item.DeliveryZoneID);

                    if(!hasError)
                    {
                        List<DeliveryZone_Postcode> Current = UnitOfWork.GetRepository<DeliveryZone_Postcode>().GetAll().Where(p => p.DeliveryZoneID == item.DeliveryZoneID).ToList();
                        List<PostcodeDatum> CurrentDZP = new List<PostcodeDatum>();
                        foreach (DeliveryZone_Postcode dzp in Current)
                        {
                            CurrentDZP.Add(new PostcodeDatum() { DeliveryZone_PostcodeID = dzp.DeliveryZone_PostcodeID, Restriction = dzp.PostcodeRestriction, Exemption = dzp.PostcodeExemption });
                        }
                        List<PostcodeDatum> Removals = CurrentDZP.Except(PostcodeList).ToList();

                        foreach (PostcodeDatum datum in Removals)
                        {
                            DeliveryZone_Postcode remove = UnitOfWork.GetRepository<DeliveryZone_Postcode>().GetAll().Where(p => p.DeliveryZone_PostcodeID == datum.DeliveryZone_PostcodeID).Single();
                            UnitOfWork.GetRepository<DeliveryZone_Postcode>().Delete(remove);
                        }
                        UnitOfWork.Save();

                        foreach (var datum in PostcodeList)
                        {
                            DeliveryZone_Postcode dc = UnitOfWork.GetRepository<DeliveryZone_Postcode>().GetAll().Where(p => p.DeliveryZone_PostcodeID == datum.DeliveryZone_PostcodeID).SingleOrDefault();

                            if (dc == null)
                            {
                                dc = new DeliveryZone_Postcode() { DeliveryZoneID = item.DeliveryZoneID.Value, PostcodeRestriction = datum.Restriction, PostcodeExemption = datum.Exemption };
                                UnitOfWork.GetRepository<DeliveryZone_Postcode>().Add(dc);
                            }
                            else
                            {
                                dc.PostcodeExemption = datum.Exemption;
                                dc.PostcodeRestriction = datum.Restriction;
                            }
                        }

                        DeliveryZone dz = UnitOfWork.GetRepository<DeliveryZone>().GetAll().Where(p => p.DeliveryZoneID == item.DeliveryZoneID).Single();
                        dz.Name = item.Name;
                        dz.CountryID = item.CountryID;
                        dz.CourierID = item.CourierID;
                        UnitOfWork.Save();

                    }
                    else
                    {
                        Response.StatusCode = 400;
                        return PartialView("DeliveryZoneComposePartial", Model);
                    }

                    return PartialView("DeliveryZoneComposePartial", Model);
                }
                else
                {
                    Response.StatusCode = 400;
                    item.DeliveryZone_Postcodes = UnitOfWork.GetRepository<DeliveryZone_Postcode>().GetAll().Where(p => p.DeliveryZoneID == item.DeliveryZoneID).ToList();

                    return PartialView("DeliveryZoneComposePartial", item);
                }
            }
            catch(Exception e)
            {
                Logger.ErrorException("", e);
                Response.StatusCode = 400;
                item.DeliveryZone_Postcodes = UnitOfWork.GetRepository<DeliveryZone_Postcode>().GetAll().Where(p => p.DeliveryZoneID == item.DeliveryZoneID).ToList();

                return PartialView("DeliveryZoneComposePartial", item);
            }
        }

        [HttpPost]
        public ActionResult AddNewDeliveryZonePostcode(DeliveryZone_Postcode item)
        {
            DeliveryZoneViewModel Model = null;
            try
            {
                if (item.PostcodeExemption == null && item.PostcodeRestriction == null)
                {
                    ModelState.AddModelError("NewPostcodeRestriction", "Provide either a Restriction or an Exemption");
                    ModelState.AddModelError("NewPostcodeExemption", "Provide either a Restriction or an Exemption");

                    Response.StatusCode = 400;

                    Model = new DeliveryZoneViewModelBuilder(UnitOfWork).Build(item.DeliveryZoneID);
                    return PartialView("DeliveryZoneComposePartial", Model);
                }

                if(item.PostcodeExemption != null && item.PostcodeRestriction != null)
                {
                    ModelState.AddModelError("NewPostcodeRestriction", "Provide either a Restriction or an Exemption, not both");
                    ModelState.AddModelError("NewPostcodeExemption", "Provide either a Restriction or an Exemption, not both");

                    Response.StatusCode = 400;

                    Model = new DeliveryZoneViewModelBuilder(UnitOfWork).Build(item.DeliveryZoneID);
                    return PartialView("DeliveryZoneComposePartial", Model);
                }


                UnitOfWork.GetRepository<DeliveryZone_Postcode>().Add(item);
                UnitOfWork.Save();

                Model = new DeliveryZoneViewModelBuilder(UnitOfWork).Build(item.DeliveryZoneID);

                return PartialView("DeliveryZoneComposePartial", Model);
            }
            catch(Exception ex)
            {
                Logger.ErrorException("", ex);
                Response.StatusCode = 400;
                return Content(ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult RemovePostcodeFromDeliveryZone(short DeliveryZoneID, short DeliveryZone_PostcodeID)
        {
            try
            {
                DeliveryZone_Postcode dzpc = UnitOfWork.GetRepository<DeliveryZone_Postcode>().GetAll().Where(p => p.DeliveryZone_PostcodeID == DeliveryZone_PostcodeID).Single();
                UnitOfWork.GetRepository<DeliveryZone_Postcode>().Delete(dzpc);
                UnitOfWork.Save();

                var Model = new DeliveryZoneViewModelBuilder(UnitOfWork).Build(DeliveryZoneID);

                return PartialView("DeliveryZoneComposePartial", Model);
            }
            catch (Exception ex) 
            { 
                Logger.ErrorException("", ex);
                Response.StatusCode = 400;
                return Content(ex.ToString());
            }
        }

        public ActionResult CheckoutHistory()
        {
            return View();
        }

        public ActionResult CheckoutLogGridViewPartial()
        {
            return PartialView();
        } 

        [HttpPost]
        public ActionResult CheckoutSearchPartial(string SearchString, short? CountryID, DateTime? FromDate, DateTime? ToDate)
        {
            if (SearchString == "")
                SearchString = null;

            var Model = UnitOfWork.Context.Checkout_Search(SearchString, CountryID, FromDate, ToDate).ToList();

            return PartialView(Model);
        }

        public ActionResult CountryComboBoxPartial()
        {
            return PartialView();
        }

        public ActionResult CheckoutLogGridPartial(long? CheckoutID)
        {
            ViewData["CheckoutID"] = CheckoutID;

            var Model = UnitOfWork.Context.CheckoutLog_Select(CheckoutID).ToList();

            return PartialView(Model);
        }

        public ActionResult DeliveryMatrix()
        {
            return View();
        }

        public ActionResult DeliveryMatrixCountryComboBoxPartial()
        {
            return PartialView();
        }

        public ActionResult DeliveryMatrixDeliveryOptionComboBoxPartial(short? CountryID)
        {
            return PartialView(CountryID);
        }

        [HttpPost]
        public ActionResult DeliveryMatrixPartial(short? CountryID, byte? DeliveryOptionID)
        {
            if(CountryID != null && DeliveryOptionID != null)
            {
                var Model = UnitOfWork.Context.DeliveryMatrix_Select(CountryID, DeliveryOptionID).ToList();

                return PartialView(Model);
            }
            else
            {
                return PartialView(null);
            }
        }

        [HttpPost]
        public ActionResult DeliveryMatrixMerge(short CountryID, byte DeliveryOptionID, string CourierProductTypes)
        {
            UnitOfWork.Context.DeliveryMatrix_Merge(CountryID, DeliveryOptionID, CourierProductTypes);

            var Model = UnitOfWork.Context.DeliveryMatrix_Select(CountryID, DeliveryOptionID).ToList();

            return PartialView("DeliveryMatrixPartial", Model);
        }

        public ActionResult DeliveryBands(short? id)
        {
            if (id != null)
            {
                DeliveryBandViewModel DBVM = null;
                if (UnitOfWork.GetRepository<DeliveryBand>().GetAll().Where(p => p.DeliveryBandID == id && p.DeletedDT == null).SingleOrDefault() != null)
                {
                    DeliveryBandViewModelBuilder Builder = new DeliveryBandViewModelBuilder(UnitOfWork);
                    DBVM = Builder.Build(id);
                }
                else
                {
                    DBVM = new DeliveryBandViewModel();
                    ViewData["InvalidDeliveryBandIDError"] = "Invalid Delivery Band ID, or the delivery band is marked as deleted. Deleted delivery bands can be retrieved.";
                }
                return View(DBVM);
            }
            else
            {
                DeliveryBandViewModel DOVM = new DeliveryBandViewModel();
                return View(DOVM);
            }
        }

        public ActionResult ChangeDeliveryBand(short DeliveryBandID)
        {
            var Model = new DeliveryBandViewModelBuilder(UnitOfWork).Build(DeliveryBandID);

            return PartialView("DeliveryBandDetailPartial", Model);
        }

        public ActionResult DeliveryBandGridViewPartial()
        {
            return PartialView();
        }
        
        public ActionResult DeliveryBandGridViewPartialAddNew(DeliveryBandViewModel item)
        {
            if (ModelState.IsValid)
            {
                DeliveryBand db = new DeliveryBand();

                db.Name = item.Name;
                db.DisplayName = item.DisplayName;
                db.PriceListID = item.PriceListID;
                db.CourierID = item.CourierID;
                db.CARRCODE = item.CarrCode;
                db.CreatedDT = DateTime.Now;
                db.SortOrder = item.SortOrder;

                UnitOfWork.GetRepository<DeliveryBand>().Add(db);

                UnitOfWork.Save();
            }

            return PartialView("DeliveryBandGridViewPartial");
        }

        public ActionResult DeliveryBandGridViewPartialUpdate(DeliveryBandViewModel item, string DeliveryZones, string StockLocations)
        {
            if(ModelState.IsValid)
            {
                DeliveryBand db = UnitOfWork.GetRepository<DeliveryBand>().GetAll().Where(p => p.DeliveryBandID == item.DeliveryBandID).Single();

                db.Name = item.Name;
                db.DisplayName = item.DisplayName;
                db.CARRCODE = item.CarrCode;
                db.SortOrder = item.SortOrder;

                List<short> InputDZ = new List<short>();
                if(DeliveryZones != "" && DeliveryZones != null)
                    InputDZ = DeliveryZones.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToList();
                List<short> CurrentDZ = UnitOfWork.GetRepository<DeliveryBand_DeliveryZone>().GetAll().Where(p => p.DeliveryBandID == item.DeliveryBandID.Value).Select(p => p.DeliveryZoneID).ToList();
                
                List<short> Removals = CurrentDZ.Except(InputDZ).ToList();

                foreach (short dz in Removals)
                {
                    DeliveryBand_DeliveryZone remove = UnitOfWork.GetRepository<DeliveryBand_DeliveryZone>().GetAll().Where(p => p.DeliveryBandID == item.DeliveryBandID.Value && p.DeliveryZoneID == dz).Single();
                    UnitOfWork.GetRepository<DeliveryBand_DeliveryZone>().Delete(remove);
                }
                UnitOfWork.Save();

                foreach (short dz in InputDZ)
                {
                    DeliveryBand_DeliveryZone dbdz = UnitOfWork.GetRepository<DeliveryBand_DeliveryZone>().GetAll().Where(p => p.DeliveryBandID == item.DeliveryBandID.Value && p.DeliveryZoneID == dz).SingleOrDefault();

                    if (dbdz == null)
                    {
                        dbdz = new DeliveryBand_DeliveryZone() { DeliveryBandID = item.DeliveryBandID.Value, DeliveryZoneID = dz };
                        UnitOfWork.GetRepository<DeliveryBand_DeliveryZone>().Add(dbdz);
                    }
                }


                List<byte> InputSL = new List<byte>();
                if(StockLocations != "" && StockLocations != null)
                    InputSL = StockLocations.Split(',').ToList().ConvertAll(p => Convert.ToByte(p)).ToList();
                List<byte> CurrentSL = UnitOfWork.GetRepository<DeliveryBand_StockLocation>().GetAll().Where(p => p.DeliveryBandID == item.DeliveryBandID.Value).Select(p => p.StockLocationID).ToList();

                List<byte> SLRemovals = CurrentSL.Except(InputSL).ToList();

                foreach (byte sl in SLRemovals)
                {
                    DeliveryBand_StockLocation remove = UnitOfWork.GetRepository<DeliveryBand_StockLocation>().GetAll().Where(p => p.DeliveryBandID == item.DeliveryBandID.Value && p.StockLocationID == sl).Single();
                    UnitOfWork.GetRepository<DeliveryBand_StockLocation>().Delete(remove);
                }
                UnitOfWork.Save();

                foreach (byte sl in InputSL)
                {
                    DeliveryBand_StockLocation dbsl = UnitOfWork.GetRepository<DeliveryBand_StockLocation>().GetAll().Where(p => p.DeliveryBandID == item.DeliveryBandID.Value && p.StockLocationID == sl).SingleOrDefault();

                    if (dbsl == null)
                    {
                        dbsl = new DeliveryBand_StockLocation() { DeliveryBandID = item.DeliveryBandID.Value, StockLocationID = sl };
                        UnitOfWork.GetRepository<DeliveryBand_StockLocation>().Add(dbsl);
                    }
                }

                UnitOfWork.Save();
            }

            return PartialView("DeliveryBandDetailPartial", new DeliveryBandViewModelBuilder(UnitOfWork).Build(item.DeliveryBandID));
        }

        public ActionResult DeliveryBandGridViewPartialDelete(DeliveryBandViewModel item)
        {
            DeliveryBand db = UnitOfWork.GetRepository<DeliveryBand>().GetAll().Where(p => p.DeliveryBandID == item.DeliveryBandID).Single();

            db.DeletedDT = DateTime.Now;

            UnitOfWork.Save();

            return PartialView("DeliveryBandGridViewPartial");
        }
    }

    public struct PostcodeDatum
    {
        public short DeliveryZone_PostcodeID;
        public string Restriction;
        public string Exemption;
    }
}