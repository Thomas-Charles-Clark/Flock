using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using NLog;
using DevExpress.Web.Mvc;
using System.Data.Linq;
using System.Data;
using Microsoft.AspNet.Identity;

namespace Flock.Controllers
{
    public partial class PromotionsController : Controller
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
            return RedirectToAction("overview", "promotions");
        }

        public ActionResult Overview()
        {
            return View();
        }

        public ActionResult CreateSalePrices()
        {
            return View();
        }

        public ActionResult SalePriceOverviewPartial()
        {
            return PartialView();
        }

        public ActionResult SalePricePriceListDropDownPartial()
        {
            var Model = UnitOfWork.GetRepository<PriceList>().GetAll().OrderBy(p => p.PriceListID).ToList();

            return PartialView("~/Views/Shared/SearchFilters/PriceListDropDownPartial.cshtml", Model);
        }
        
        [HttpPost]
        public ActionResult SalePricePriceListBoxSetSessionVariable(byte? PriceListID)
        {
            Session["SalePricePriceListID"] = PriceListID;
            return Content("");
        }

        public ActionResult PromoCodeFreeGiftProductIDComboBoxPartial()
        {
            return PartialView("Codes/PromoCodeFreeGiftProductIDComboBoxPartial");
        }

        public ActionResult SalePriceGridView()
        {
            return PartialView("_SalePriceOverviewPartial");
        }

        //public ActionResult SalePriceGridViewBatchEditingUpdate(MVCxGridViewBatchUpdateValues<vwSalePrice, long> updateValues)
        //{
        //    string updateErrors = null;
        //    MVCxGridViewBatchUpdateValues<vwSalePrice, long> validUpdates = new MVCxGridViewBatchUpdateValues<vwSalePrice, long>();
        //    foreach (var product in updateValues.Update)
        //    {
        //        if (updateValues.IsValid(product)) 
        //        {
        //            validUpdates.Update.Add(product);
        //        }
        //        else
        //        {
        //            updateErrors += "Row: " +product.SalePriceID.ToString() + " was not updated. \r\n" + updateValues.EditorErrors[product].Count.ToString() + " errors were present for this record.\r\n";
        //            //log e
        //        }
        //    }
        //    if(validUpdates.Update.Count > 0)
        //    {
        //        UnitOfWork.Context.SalePrice_Update(validUpdates);
        //    }
        //    ViewData["UpdateErrors"] = updateErrors;
        //    return PartialView("_SalePriceGridView");
        //}

        public ActionResult SalePriceGridViewCustomCommand(string commandType, string keyString, DateTime? onDate, DateTime? offDate, decimal? discount, int? stockMinimum, bool? overwriteConflicts)
        {
            List<long> KeysList = keyString.Split(',').ToList().ConvertAll(p => Convert.ToInt64(p)).ToList();

            switch (commandType)
            {
                case "delete":
                    var ToDelete = UnitOfWork.GetRepository<SalePrice>().GetAll().InRange(p => p.SalePriceID, 2000, KeysList).ToList();
                    foreach(var td in ToDelete)
                    {
                        td.DeletedDT = DateTime.Now;
                    }
                    UnitOfWork.Save();
                    break;

                case "update":

                    string updateResponse = UnitOfWork.Context.SalePrice_Overview_Update(KeysList, onDate, offDate, discount, stockMinimum, overwriteConflicts.Value);

                    if(updateResponse != "" && updateResponse != null)
                    {
                        Response.StatusCode = 400;
                        return Content(updateResponse);
                    }

                    break;
                default: break;
            }

            return PartialView("_SalePriceOverviewPartial");
        }

        public ActionResult AddNewSaleGroupProductSelectPartial()
        {
            return PartialView("_AddNewSaleGroupProductSelectGrid");
        }

        public ActionResult SalePriceAddNewGridView()
        {
            return PartialView("_SalePriceAddNewGridView");
        }

        //public ActionResult SalePriceAddNewGridViewBatchEditingUpdate(MVCxGridViewBatchUpdateValues<vwSalePrice, long> updateValues, byte PriceListID)
        //{
        //    MVCxGridViewBatchUpdateValues<vwSalePrice, long> validUpdates = new MVCxGridViewBatchUpdateValues<vwSalePrice, long>();
        //    bool isValid = true;
        //    for (var i = (updateValues.Update.Count - 1); i >= 0; i--)// in updateValues.Update)
        //    {
        //        if (updateValues.IsValid(updateValues.Update[i])) 
        //        {
        //            if (updateValues.Update[i].OnDate >= updateValues.Update[i].OffDate)
        //            {
        //                var msd = new ModelStateDictionary();
        //                // apparently EditorErrors aren't displayed...?
        //                msd.AddModelError("OnDate", "On Date must preceed Off Date");
        //                updateValues.EditorErrors.Add(updateValues.Update[i], msd);
        //                isValid = false;
        //                updateValues.Update.Remove(updateValues.Update[i]);
        //            }
        //            else if (updateValues.Update[i].OffDate <= updateValues.Update[i].OnDate)
        //            {
        //                var msd = new ModelStateDictionary();
        //                msd.AddModelError("OffDate", "Off Date must succeed On Date");
        //                updateValues.EditorErrors.Add(updateValues.Update[i], msd);
        //                isValid = false;
        //                updateValues.Update.Remove(updateValues.Update[i]);
        //            }
        //            if(isValid)
        //                validUpdates.Update.Add(updateValues.Update[i]);
        //        }
        //        else
        //        {
        //            //log e
        //        }
        //    }
        //    if(validUpdates.Update.Count > 0)
        //    {
        //        for (int i = 0; i < validUpdates.Update.Count; i++)
        //        {
        //            SalePrice SP = new SalePrice()
        //            {
        //                PriceListID = PriceListID,
        //                Discount = validUpdates.Update[i].Discount,
        //                SalePriceTypeID = validUpdates.Update[i].SalePriceTypeID,
        //                ProductID = validUpdates.Update[i].ProductID,
        //                OnDate = validUpdates.Update[i].OnDate,
        //                OffDate = validUpdates.Update[i].OffDate,
        //                SalePriceInc = validUpdates.Update[i].SalePriceInc,
        //                SalePriceEx = validUpdates.Update[i].SalePriceEx
        //            };

        //            UnitOfWork.GetRepository<SalePrice>().Add(SP);
        //        }

        //        UnitOfWork.Save();
        //        //UnitOfWork.Context.SalePrice_Update(validUpdates);
        //    }
        //    if (!isValid)
        //        Response.StatusCode = 400;
        //    return PartialView("_SalePriceAddNewGridView", updateValues);
        //}

        public ActionResult AddNewSaleGroupProductSelectGrid(byte? SalePricePriceListID)
        {
            
            Session["SalePricePriceListID"] = SalePricePriceListID;

            return PartialView("_AddNewSaleGroupProductSelectGrid");
        }

        [HttpPost]
        public ActionResult NewSaleGroupReview(string Name, string KeyString, byte PriceListID, byte SalePriceTypeID, decimal Discount, DateTime OnDate, string OffDate, byte? StockMinimum)
        {
            DateTime? offDate;
            if (OffDate == "" || OffDate == null)
                offDate = null;
            else offDate = DateTime.Parse(OffDate);

            //SalePriceEvent SPE = new SalePriceEvent() { Name = Name, CreatedDT = DateTime.Now };
            //UnitOfWork.GetRepository<SalePriceEvent>().Add(SPE);
            //UnitOfWork.Save();

            List<int> ProductIDs = KeyString.Split(',').ToList().ConvertAll(p => Convert.ToInt32(p)).ToList();

            DataSet Results = UnitOfWork.Context.SalePrice_Validate(ProductIDs, PriceListID, SalePriceTypeID, Discount, OnDate, offDate, StockMinimum);
           
            return PartialView("_NewSaleGroupReviewPartial", Results);
        }

        public ActionResult ReviewGridPartial(string Name, string KeyString, byte PriceListID, byte SalePriceTypeID, decimal Discount, DateTime OnDate, string OffDate, byte? StockMinimum)
        {
            DateTime? offDate;
            if (OffDate == "" || OffDate == null)
                offDate = null;
            else offDate = DateTime.Parse(OffDate);

            List<int> ProductIDs = KeyString.Split(',').ToList().ConvertAll(p => Convert.ToInt32(p)).ToList();

            DataSet Results = UnitOfWork.Context.SalePrice_Validate(ProductIDs, PriceListID, SalePriceTypeID, Discount, OnDate, offDate, StockMinimum);
           
            return PartialView("_ReviewGridPartial", Results);
        }

        //
        public ActionResult ReviewGridDetailDelete([ModelBinder(typeof(DevExpressEditorsBinder))]vwSalePriceOverview item, string Name, byte PriceListID, byte SalePriceTypeID, decimal Discount, DateTime OnDate, string OffDate, byte? StockMinimum)
        {
            SalePrice SP = UnitOfWork.GetRepository<SalePrice>().GetAll().Where(p => p.SalePriceID == item.SalePriceID).Single();
            SP.DeletedDT = DateTime.Now;
            UnitOfWork.Save();

            DateTime? offDate;
            if (OffDate == "" || OffDate == null)
                offDate = null;
            else offDate = DateTime.Parse(OffDate);

            List<int> ProductIDs = new List<int>() { SP.ProductID };

            DataSet Results = UnitOfWork.Context.SalePrice_Validate(ProductIDs, PriceListID, SalePriceTypeID, Discount, OnDate, offDate, StockMinimum);
            
            var SalePriceIDs = Results.Tables[1].AsEnumerable()
                        .Where(row => (int)item.ProductID == row.Field<int>("ProductID")).Select(p => p.Field<long>("SalePriceID")).ToList();

            var DetailModel = UnitOfWork.GetRepository<vwSalePriceOverview>().GetAll().Where(p => p.ProductID == (int)item.ProductID && SalePriceIDs.Contains(p.SalePriceID)).ToList();

            return Content("");
            //return PartialView("_ReviewGridDetailPartial", DetailModel);
        }

        [HttpPost]
        public ActionResult CanSaveWithoutOverwrite(string Name, string KeyString, byte PriceListID, byte SalePriceTypeID, decimal Discount, DateTime OnDate, string OffDate, byte? StockMinimum)
        {
            DateTime? offDate;
            if (OffDate == "" || OffDate == null)
                offDate = null;
            else offDate = DateTime.Parse(OffDate);

            List<int> ProductIDs = KeyString.Split(',').ToList().ConvertAll(p => Convert.ToInt32(p)).ToList();

            DataSet Results = UnitOfWork.Context.SalePrice_Validate(ProductIDs, PriceListID, SalePriceTypeID, Discount, OnDate, offDate, StockMinimum);
           
            return Content(Results.Tables[1].Rows.Count.ToString());
        }

        [HttpPost]
        public ActionResult SaveSalePrices(string Name, string KeyString, byte PriceListID, byte SalePriceTypeID, decimal Discount, DateTime OnDate, string OffDate, byte? StockMinimum, bool OverwriteConflicts)
        {
            DateTime? offDate;
            if (OffDate == "" || OffDate == null)
                offDate = null;
            else offDate = DateTime.Parse(OffDate);

            List<int> ProductIDs = KeyString.Split(',').ToList().ConvertAll(p => Convert.ToInt32(p)).ToList();

            DataSet Results = UnitOfWork.Context.SalePrice_Validate(ProductIDs, PriceListID, SalePriceTypeID, Discount, OnDate, offDate, StockMinimum);

            UnitOfWork.Context.SalePrice_Save(Results.Tables[0], Name, PriceListID, SalePriceTypeID, Discount, OnDate, offDate, StockMinimum, OverwriteConflicts);

            //ViewData["SuccessfulSave"] = true;

            return Content(Results.Tables[1].Rows.Count.ToString());
        }

        public ActionResult MarketingPartners()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MarketingPartnerManagerGridView()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddNewMarketingPartner(MarketingPartner item)
        {
            if(ModelState.IsValid)
            {
                item.CreatedDT = DateTime.Now;
                UnitOfWork.GetRepository<MarketingPartner>().Add(item);
                UnitOfWork.Save();
            }

            return PartialView("MarketingPartnerManagerGridView");
        }

        [HttpPost]
        public ActionResult UpdateMarketingPartner(MarketingPartner item)
        {
            if(ModelState.IsValid)
            {
                var i = UnitOfWork.GetRepository<MarketingPartner>().GetAll().Where(p => p.MarketingPartnerID == item.MarketingPartnerID).Single();
                i.Name = item.Name;
                UnitOfWork.Save();
            }

            return PartialView("MarketingPartnerManagerGridView");
        }

        [HttpPost]
        public ActionResult DeleteMarketingPartner(MarketingPartner item)
        {
            var i = UnitOfWork.GetRepository<MarketingPartner>().GetAll().Where(p => p.MarketingPartnerID == item.MarketingPartnerID).Single();
            i.DeletedDT = DateTime.Now;
            UnitOfWork.Save();

            return PartialView("MarketingPartnerManagerGridView");
        }


        public ActionResult Competitions()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CompetitionManagerGridView()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddNewCompetition(Competition item)
        {
            if (item.Name == null)
                ModelState.AddModelError("Name", "Provide a Name for this Competition");
            if (item.Heading == null)
                ModelState.AddModelError("Heading", "Provide a Heading for this Competition");
            if (item.OffDate != null && item.OnDate == null)
                ModelState.AddModelError("OnDate", "Please set an On Date");
            if (item.OffDate != null && item.OnDate != null)
                if (item.OffDate <= item.OnDate)
                    ModelState.AddModelError("OnDate", "On Date must preceed Off Date");

            item.CreatedDT = DateTime.Now;

            if (ModelState.IsValid)
            {
                UnitOfWork.GetRepository<Competition>().Add(item);
                UnitOfWork.Save();
                UnitOfWork.Context.Competition_Language_Initialise();
            }

            return PartialView("CompetitionManagerGridView");
        }

        [HttpPost]
        public ActionResult UpdateCompetition(Competition item)
        {
            if (item.Name == null)
                ModelState.AddModelError("Name", "Provide a Name for this Competition");
            if (item.Heading == null)
                ModelState.AddModelError("Heading", "Provide a Heading for this Competition");
            if (item.OffDate != null && item.OnDate == null)
                ModelState.AddModelError("OnDate", "Please set an On Date");
            if (item.OffDate != null && item.OnDate != null)
                if (item.OffDate <= item.OnDate)
                    ModelState.AddModelError("OffDate", "Off Date must not precede or equal On Date");

            if (ModelState.IsValid)
            {
                var i = UnitOfWork.GetRepository<Competition>().GetAll().Where(p => p.CompetitionID == item.CompetitionID).Single();
                i.Name = item.Name;
                i.Heading = item.Heading;
                i.SubHeading = item.SubHeading;
                i.Description = item.Description;
                i.Terms = item.Terms;
                i.OnDate = item.OnDate;
                i.OffDate = item.OffDate;
                i.MarketingPartnerID = item.MarketingPartnerID;
                UnitOfWork.Save();
            }

            return PartialView("CompetitionManagerGridView");
        }

        [HttpPost]
        public ActionResult DeleteCompetition(Competition item)
        {
            var i = UnitOfWork.GetRepository<Competition>().GetAll().Where(p => p.CompetitionID == item.CompetitionID).Single();
            i.DeletedDT = DateTime.Now;
            UnitOfWork.Save();

            return PartialView("CompetitionManagerGridView");
        }

        [Authorize(Roles = "Admin,Promotion Codes")]
        public ActionResult Codes()
        {
            return View("Codes/Codes");
        }

        public ActionResult PromoCodeGridViewPartial()
        {
            return PartialView("Codes/PromoCodeGridViewPartial");
        }

        public ActionResult PromoCodeDeliveryBandPartial(short? promotionCodeID, byte? priceListID)
        {
            ViewData["PriceListID"] = priceListID;

            return PartialView("Codes/PromoCodeDeliveryBandPartial", new PromoCodeViewModelBuilder(UnitOfWork).Build(promotionCodeID));
        }

        public ActionResult PromoCodeGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))]PromoCodeViewModel item, string memberRestrictions, string productRestrictions)
        {
            ///// IN NEED OF TESTING CONFLICTING PROMOTIONAL CODES

            if (item.OffDate != null && item.OnDate == null)
                ModelState.AddModelError("OnDate", "Please set an On Date");
            else if (item.OffDate != null && item.OnDate != null)
            {
                if (item.OffDate <= item.OnDate)
                {
                    ModelState.AddModelError("OffDate", "Off Date must not precede or equal On Date");
                    ModelState.AddModelError("OnDate", "On Date must precede Off Date");
                }
            }
            
            {   // this isn't being hit by most of the new additions... why not... is it because it's elsed in.
                // why is it ... okay, that works now!
                // next problem: the calendar drop downs aren't given sufficient height to live.

                var extant = UnitOfWork.GetRepository<PromotionCode>().GetAll().Where(p => p.Code == item.Code && p.PriceListID == item.PriceListID).ToList();

                List<string> conflictingIDs = new List<string>();

                foreach (var pc in extant)
                {
                    if (pc != null)
                    {
                        DateTime? StartExtant = pc.OnDate;
                        DateTime? EndExtant = pc.OffDate == null ? DateTime.MaxValue : pc.OffDate;

                        DateTime? StartNew = item.OnDate;
                        DateTime? EndNew = item.OffDate == null ? DateTime.MaxValue : item.OffDate;

                        if (pc.OnDate != null)
                        {
                            if (StartNew < EndExtant && EndNew > StartExtant)
                            {
                                conflictingIDs.Add(pc.PromotionCodeID.ToString());
                            }
                        }
                    }
                }

                if (conflictingIDs.Count > 0)
                {
                    ModelState.AddModelError("Code", "This conflicts with Promotion Code" + (conflictingIDs.Count == 1 ? ": " : "s: ") + String.Join(", ", conflictingIDs.ToArray()));
                    ModelState.AddModelError("OnDate", "This conflicts with Promotion Code " + (conflictingIDs.Count == 1 ? ": " : "s: ") + String.Join(", ", conflictingIDs.ToArray()));
                    ModelState.AddModelError("OffDate", "This conflicts with Promotion Code " + (conflictingIDs.Count == 1 ? ": " : "s: ") + String.Join(", ", conflictingIDs.ToArray()));
                }
            }

            if (!item.PriceListID.HasValue)
                ModelState.AddModelError("PriceListID", "A Price List must be specified for this promotion code");

            List<int> memberIDs = new List<int>();
            if (memberRestrictions != null && memberRestrictions != "")
                memberIDs = memberRestrictions.Split(',').ToList().ConvertAll(p => Convert.ToInt32(p)).ToList();

            List<int> productIDs = new List<int>();
            if (productRestrictions != null && productRestrictions != "")
                productIDs = productRestrictions.Split(',').ToList().ConvertAll(p => Convert.ToInt32(p)).ToList();

            byte[] ProductTypes = CheckBoxListExtension.GetSelectedValues<byte>("PromoCodeProductTypesUnbound");
            item.PromoCode_ProductTypes = new List<PromotionCode_ProductType>();
            if(ProductTypes != null)
                foreach(byte pt in ProductTypes)
                    item.PromoCode_ProductTypes.Add(new PromotionCode_ProductType() { ProductTypeID = pt, PromotionCodeID = 0 });

            byte[] DeliveryBands = CheckBoxListExtension.GetSelectedValues<byte>("PromoCodeDeliveryBandsUnbound");
            item.PromoCode_DeliveryBands = new List<PromotionCode_DeliveryBand>();
            if(DeliveryBands != null)
                foreach(byte db in DeliveryBands)
                    item.PromoCode_DeliveryBands.Add(new PromotionCode_DeliveryBand() { DeliveryBandID = db, PromotionCodeID = 0 });

            byte[] MemberSegments = CheckBoxListExtension.GetSelectedValues<byte>("PromoCodeMemberSegmentsUnbound");
            item.PromoCode_MemberSegments = new List<PromotionCode_MemberSegment>();
            if(MemberSegments != null)
                foreach(byte ms in MemberSegments)
                    item.PromoCode_MemberSegments.Add(new PromotionCode_MemberSegment(){ MemberSegmentID = ms, PromotionCodeID = 0 });

            if(ModelState.IsValid)
            {
                PromotionCode pc = new PromotionCode();
                pc.PromotionCodeTypeID = 1;
                pc.PromotionGroupID = item.PromotionGroupID;
                pc.Code = item.Code;
                pc.Description = item.Description;
                pc.OnDate = item.OnDate;
                pc.OffDate = item.OffDate;
                pc.MinimumSpend = item.MinimumSpend;
                pc.MaxUses = item.MaxUses;
                pc.MaxUsesPerUser = item.MaxUsesPerUser;
                pc.TransactionSequenceRestriction = item.TransactionSequenceRestriction;
                pc.PriceListID = (byte)item.PriceListID.Value;
                pc.Discount = item.Discount;
                pc.DiscountType = item.DiscountType;
                pc.FreeGiftProductID = item.FreeGiftProductID;
                pc.FreeGiftQuantity = item.FreeGiftQuantity;
                pc.CreatedDT = DateTime.Now;
                pc.DiscountBundles = item.DiscountBundles;

                pc.ApplyDiscountToFullPrice = item.ApplyDiscountToFullPrice;
                pc.ApplyDiscountToSalePrice = item.ApplyDiscountToSalePrice;
                pc.ApplyDiscountToTradePrice = item.ApplyDiscountToTradePrice;

                pc.CreatedByUserID = User.Identity.GetUserId();

                UnitOfWork.GetRepository<PromotionCode>().Add(pc);

                UnitOfWork.Save();

                if (MemberSegments != null)
                    foreach (byte ms in MemberSegments)
                        UnitOfWork.GetRepository<PromotionCode_MemberSegment>().Add(new PromotionCode_MemberSegment() { PromotionCodeID = pc.PromotionCodeID, MemberSegmentID = ms });

                if (ProductTypes != null)
                    foreach (byte pt in ProductTypes)
                        UnitOfWork.GetRepository<PromotionCode_ProductType>().Add(new PromotionCode_ProductType() { PromotionCodeID = pc.PromotionCodeID, ProductTypeID = pt });

                if (DeliveryBands != null)
                    foreach (byte db in DeliveryBands)
                        UnitOfWork.GetRepository<PromotionCode_DeliveryBand>().Add(new PromotionCode_DeliveryBand() { PromotionCodeID = pc.PromotionCodeID, DeliveryBandID = db, PriceEx = item.DeliveryBandPriceEx, PriceInc = item.DeliveryBandPriceInc });

                if(memberIDs.Count > 0)
                {
                    foreach (int m in memberIDs)
                        UnitOfWork.GetRepository<PromotionCode_Member>().Add(new PromotionCode_Member() { PromotionCodeID = pc.PromotionCodeID, MemberID = m });
                }

                if(productIDs.Count > 0)
                {
                    foreach (int p in productIDs)
                        UnitOfWork.GetRepository<PromotionCode_Product>().Add(new PromotionCode_Product() { PromotionCodeID = pc.PromotionCodeID, ProductID = p });
                }

                UnitOfWork.Save();

                var MemberLinker = UnitOfWork.GetRepository<PromotionCode_Member>().GetAll().Where(p => p.PromotionCodeID == item.PromotionCodeID).Select(p => p.MemberID).ToList();
                item.PromoCode_Members = UnitOfWork.GetRepository<Member>().GetAll().Where(p => MemberLinker.Contains(p.MemberID)).ToList();

                var ProductLinker = UnitOfWork.GetRepository<PromotionCode_Product>().GetAll().Where(p => p.PromotionCodeID == item.PromotionCodeID).Select(p => p.ProductID).ToList();
                item.PromoCode_Products = UnitOfWork.GetRepository<vwPromoCodeProductImage>().GetAll().Where(p => p.PromotionCodeID == item.PromotionCodeID).ToList();

                return PartialView("Codes/PromoCodeGridViewPartial");
            }
            else
            {
                item.PromoCode_Members = UnitOfWork.GetRepository<Member>().GetAll().Where(p => memberIDs.Contains(p.MemberID)).ToList();
                item.PromoCode_Products = UnitOfWork.GetRepository<vwPromoCodeProductImage>().GetAll().Where(p => p.PromotionCodeID == item.PromotionCodeID).ToList();

                return PartialView("Codes/PromoCodeGridViewPartial", item);
            }
        }

        public ActionResult PromoCodeGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))]PromoCodeViewModel item)
        {
            string errorText = "";

            if (item.OffDate != null && item.OnDate == null)
                ModelState.AddModelError("OnDate", "Please set an On Date");
            else if (item.OffDate != null && item.OnDate != null)
            { 
                if (item.OffDate <= item.OnDate)
                {
                    ModelState.AddModelError("OffDate", "Off Date must not precede or equal On Date");
                    ModelState.AddModelError("OnDate", "On Date must precede Off Date");
                }
            }
            if(item.GiftBoxPriceInc != null && item.GiftBoxPriceEx == null)
            {
                ModelState.AddModelError("GiftBoxPriceEx", "If GiftBoxPriceInc is set, GiftBoxPriceEx must be set as well.");
            }
            if (item.GiftBoxPriceEx != null && item.GiftBoxPriceInc == null)
            {
                ModelState.AddModelError("GiftBoxPriceInc", "If GiftBoxPriceEx is set, GiftBoxPriceInc must be set as well.");
            }
            {
                // test this in the morning
                var extant = UnitOfWork.GetRepository<PromotionCode>().GetAll().Where(p => p.Code == item.Code && p.PriceListID == item.PriceListID && p.PromotionCodeID != item.PromotionCodeID).ToList();

                List<string> conflictingIDs = new List<string>();

                foreach(var pc in extant)
                { 
                    if (pc != null)
                    {
                        DateTime? StartExtant = pc.OnDate;
                        DateTime? EndExtant = pc.OffDate == null ? DateTime.MaxValue : pc.OffDate;

                        DateTime? StartNew = item.OnDate;
                        DateTime? EndNew = item.OffDate == null ? DateTime.MaxValue : item.OffDate;

                        if (pc.OnDate != null)
                        {
                            if (StartNew < EndExtant && EndNew > StartExtant)
                            {
                                conflictingIDs.Add(pc.PromotionCodeID.ToString());
                            }
                        }
                    }
                }

                if(conflictingIDs.Count > 0)
                {
                    ModelState.AddModelError("Code", "This conflicts with Promotion Code" + (conflictingIDs.Count == 1 ? ": " : "s: ")  + String.Join(", ", conflictingIDs.ToArray()));
                    ModelState.AddModelError("OnDate", "This conflicts with Promotion Code " + (conflictingIDs.Count == 1 ? ": " : "s: ") + String.Join(", ", conflictingIDs.ToArray()));
                    ModelState.AddModelError("OffDate", "This conflicts with Promotion Code " + (conflictingIDs.Count == 1 ? ": " : "s: ") + String.Join(", ", conflictingIDs.ToArray()));
                }
            }

            if (item.FreeGiftProductID != null)
            {
                var pr = UnitOfWork.GetRepository<Product>().GetAll().Where(p => p.ProductID == item.FreeGiftProductID.Value).SingleOrDefault();

                if (pr == null)
                {
                    ModelState.AddModelError("FreeGiftProductID", "Invalid Product ID");
                    errorText += "Invalid Free Gift Product ID. Product not found.";
                }
                else if (pr.DeletedDT != null) {
                    ModelState.AddModelError("FreeGiftProductID", "This Product has been deleted");
                    errorText += " The Free Gift Product is marked as deleted.";
                }   
            }

            if (item.FreeGiftQuantity == 0)
            {
                ModelState.AddModelError("FreeGiftQuantity", "Invalid value");
                errorText += " Invalid value for Free Gift Quantity.";
            }

            if (!item.PriceListID.HasValue)
                ModelState.AddModelError("PriceListID", "A Price List must be specified for this promotion code");

            byte[] ProductTypes = CheckBoxListExtension.GetSelectedValues<byte>("PromoCodeProductTypesUnbound");
            item.PromoCode_ProductTypes = new List<PromotionCode_ProductType>();
            if (ProductTypes != null)
                foreach (byte pt in ProductTypes)
                    item.PromoCode_ProductTypes.Add(new PromotionCode_ProductType() { ProductTypeID = pt, PromotionCodeID = 0 });

            byte[] DeliveryBands = CheckBoxListExtension.GetSelectedValues<byte>("PromoCodeDeliveryBandsUnbound");
            item.PromoCode_DeliveryBands = new List<PromotionCode_DeliveryBand>();
            if (DeliveryBands != null)
                foreach (byte db in DeliveryBands)
                    item.PromoCode_DeliveryBands.Add(new PromotionCode_DeliveryBand() { DeliveryBandID = db, PromotionCodeID = 0 });

            byte[] MemberSegments = CheckBoxListExtension.GetSelectedValues<byte>("PromoCodeMemberSegmentsUnbound");
            item.PromoCode_MemberSegments = new List<PromotionCode_MemberSegment>();
            if (MemberSegments != null)
                foreach (byte ms in MemberSegments)
                    item.PromoCode_MemberSegments.Add(new PromotionCode_MemberSegment() { MemberSegmentID = ms, PromotionCodeID = 0 });

            var MemberLinker = UnitOfWork.GetRepository<PromotionCode_Member>().GetAll().Where(p => p.PromotionCodeID == item.PromotionCodeID).Select(p => p.MemberID).ToList();
            item.PromoCode_Members = UnitOfWork.GetRepository<Member>().GetAll().Where(p => MemberLinker.Contains(p.MemberID)).ToList();

            var ProductLinker = UnitOfWork.GetRepository<PromotionCode_Product>().GetAll().Where(p => p.PromotionCodeID == item.PromotionCodeID).Select(p => p.ProductID).ToList();
            item.PromoCode_Products = UnitOfWork.GetRepository<vwPromoCodeProductImage>().GetAll().Where(p => p.PromotionCodeID == item.PromotionCodeID).ToList();

            ViewData["EditError"] = errorText;

            if (ModelState.IsValid)
            {
                PromotionCode pc = UnitOfWork.GetRepository<PromotionCode>().GetAll().Where(p => p.PromotionCodeID == item.PromotionCodeID).Single();

                pc.PromotionGroupID = item.PromotionGroupID;
                pc.Code = item.Code;
                pc.Description = item.Description;
                pc.OnDate = item.OnDate;
                pc.OffDate = item.OffDate;
                pc.MinimumSpend = item.MinimumSpend;
                pc.MaxUses = item.MaxUses;
                pc.MaxUsesPerUser = item.MaxUsesPerUser;
                pc.TransactionSequenceRestriction = item.TransactionSequenceRestriction;
                pc.PriceListID = (byte)item.PriceListID.Value;
                pc.Discount = item.Discount;
                pc.DiscountType = item.DiscountType;
                pc.FreeGiftProductID = item.FreeGiftProductID;
                pc.FreeGiftQuantity = item.FreeGiftQuantity;
                pc.GiftBoxPriceEx = item.GiftBoxPriceEx;
                pc.GiftBoxPriceInc = item.GiftBoxPriceInc;
                pc.DiscountBundles = item.DiscountBundles;
                pc.CreatedDT = DateTime.Now;

                pc.ApplyDiscountToFullPrice = item.ApplyDiscountToFullPrice;
                pc.ApplyDiscountToSalePrice = item.ApplyDiscountToSalePrice;
                pc.ApplyDiscountToTradePrice = item.ApplyDiscountToTradePrice;

                if(MemberSegments != null)
                    UnitOfWork.GetRepository<PromotionCode_MemberSegment>().DeleteAll(UnitOfWork.GetRepository<PromotionCode_MemberSegment>().GetAll().Where(p => p.PromotionCodeID == pc.PromotionCodeID));
                if(ProductTypes != null)
                    UnitOfWork.GetRepository<PromotionCode_ProductType>().DeleteAll(UnitOfWork.GetRepository<PromotionCode_ProductType>().GetAll().Where(p => p.PromotionCodeID == pc.PromotionCodeID));
                if(DeliveryBands != null)
                    UnitOfWork.GetRepository<PromotionCode_DeliveryBand>().DeleteAll(UnitOfWork.GetRepository<PromotionCode_DeliveryBand>().GetAll().Where(p => p.PromotionCodeID == pc.PromotionCodeID));

                UnitOfWork.Save();

                if(MemberSegments != null)
                    foreach (byte ms in MemberSegments)
                        UnitOfWork.GetRepository<PromotionCode_MemberSegment>().Add(new PromotionCode_MemberSegment() { PromotionCodeID = pc.PromotionCodeID, MemberSegmentID = ms });

                if(ProductTypes != null)
                    foreach (byte pt in ProductTypes)
                        UnitOfWork.GetRepository<PromotionCode_ProductType>().Add(new PromotionCode_ProductType() { PromotionCodeID = pc.PromotionCodeID, ProductTypeID = pt });

                if(DeliveryBands != null)
                    foreach (byte db in DeliveryBands)
                        UnitOfWork.GetRepository<PromotionCode_DeliveryBand>().Add(new PromotionCode_DeliveryBand() { PromotionCodeID = pc.PromotionCodeID, DeliveryBandID = db, PriceEx = item.DeliveryBandPriceEx, PriceInc = item.DeliveryBandPriceInc });

                UnitOfWork.Save();

                return PartialView("Codes/PromoCodeGridViewPartial");
            }
            else
            {
                return PartialView("Codes/PromoCodeGridViewPartial", item);
            }
        }

        [HttpPost]
        public ActionResult UpdatePromoCodeMemberRestrictions(short? promotionCodeID, string members)
        {
            List<int> Desired = new List<int>();
            if (members != null && members != "")
                Desired = members.Split(',').ToList().ConvertAll(p => Convert.ToInt32(p)).ToList();

            if(promotionCodeID == null)
            {
                var Model = UnitOfWork.GetRepository<Member>().GetAll().Where(p => Desired.Contains(p.MemberID)).ToList();

                return PartialView("Codes/PromoCodeMemberRestrictionsPartial", Model);
            }
            else
            {
                var Current = UnitOfWork.GetRepository<PromotionCode_Member>().GetAll().Where(p => p.PromotionCodeID == promotionCodeID.Value).Select(p => p.MemberID).ToList();
                var Removals = Current.Except(Desired);
                foreach(var i in Removals)
                {
                    PromotionCode_Member pcm = UnitOfWork.GetRepository<PromotionCode_Member>().GetAll().Where(p => p.PromotionCodeID == promotionCodeID && p.MemberID == i).Single();
                    UnitOfWork.GetRepository<PromotionCode_Member>().Delete(pcm);
                }
                UnitOfWork.Save();

                foreach(var i in Desired)
                {
                    PromotionCode_Member pcm = UnitOfWork.GetRepository<PromotionCode_Member>().GetAll().Where(p => p.PromotionCodeID == promotionCodeID && p.MemberID == i).SingleOrDefault();
                    if(pcm == null)
                    {
                        pcm = new PromotionCode_Member() { PromotionCodeID = promotionCodeID.Value, MemberID = i };
                        UnitOfWork.GetRepository<PromotionCode_Member>().Add(pcm);
                    }
                }
                UnitOfWork.Save();

                var Model = UnitOfWork.GetRepository<Member>().GetAll().Where(p => Desired.Contains(p.MemberID)).ToList();

                return PartialView("Codes/PromoCodeMemberRestrictionsPartial", Model);
            }
        }

        [HttpPost]
        public ActionResult UpdatePromoCodeProductRestrictions(short? promotionCodeID, string products)
        {
            List<int> Desired = new List<int>();
            if (products != null && products != "")
                Desired = products.Split(',').ToList().ConvertAll(p => Convert.ToInt32(p)).ToList();

            if (promotionCodeID == null)
            {
                var Model = UnitOfWork.GetRepository<vwPromoCodeProductImage>().GetAll().Where(p => Desired.Contains(p.ProductID)).ToList();

                return PartialView("Codes/PromoCodeProductRestrictionsPartial", Model);
            }
            else
            {
                var Current = UnitOfWork.GetRepository<PromotionCode_Product>().GetAll().Where(p => p.PromotionCodeID == promotionCodeID).Select(p => p.ProductID).ToList();
                var Removals = Current.Except(Desired);
                foreach(var i in Removals)
                {
                    PromotionCode_Product ppc = UnitOfWork.GetRepository<PromotionCode_Product>().GetAll().Where(p => p.PromotionCodeID == promotionCodeID && p.ProductID == i).Single();
                    UnitOfWork.GetRepository<PromotionCode_Product>().Delete(ppc);
                }
                UnitOfWork.Save();

                foreach(var i in Desired)
                {
                    PromotionCode_Product ppc = UnitOfWork.GetRepository<PromotionCode_Product>().GetAll().Where(p => p.PromotionCodeID == promotionCodeID && p.ProductID == i).SingleOrDefault();
                    if(ppc == null)
                    {
                        ppc = new PromotionCode_Product() { PromotionCodeID = promotionCodeID.Value, ProductID = i };
                        UnitOfWork.GetRepository<PromotionCode_Product>().Add(ppc);
                    }
                }
                UnitOfWork.Save();

                var Model = UnitOfWork.GetRepository<vwPromoCodeProductImage>().GetAll().Where(p => Desired.Contains(p.ProductID)).ToList();

                return PartialView("Codes/PromoCodeProductRestrictionsPartial", Model);
            }
        }

        public ActionResult PromoCodeEditFormPartial(short? promotionCodeID)
        {
            var Model = new PromoCodeViewModelBuilder(UnitOfWork).Build(promotionCodeID);

            return PartialView("Codes/PromoCodeEditFormPartial", Model);
        }

        public ActionResult PromoCodeGridViewPartialDelete()
        {
            return PartialView("Codes/PromoCodeGridViewPartial");
        }

        [HttpPost]
        public ActionResult AddProductsFromPageSection(short PromotionCodeID, long PageSectionID)
        {
            var res = UnitOfWork.Context.PromotionCode_Product_Merge(PromotionCodeID, PageSectionID);
            var Current = UnitOfWork.GetRepository<PromotionCode_Product>().GetAll().Where(p => p.PromotionCodeID == PromotionCodeID).Select(p => p.ProductID).ToList();
            var Model = UnitOfWork.GetRepository<vwPromoCodeProductImage>().GetAll().Where(p => p.PromotionCodeID == PromotionCodeID).ToList();

            return PartialView("Codes/PromoCodeProductRestrictionsPartial", Model);
        }

        [HttpPost]
        public ActionResult RemoveProductsFromPromoCode(short PromotionCodeID)
        {
            UnitOfWork.GetRepository<PromotionCode_Product>().DeleteAll(UnitOfWork.GetRepository<PromotionCode_Product>().GetAll().Where(p => p.PromotionCodeID == PromotionCodeID).ToList());
            UnitOfWork.Save();
            var Model = new List<vwPromoCodeProductImage>();
            return PartialView("Codes/PromoCodeProductRestrictionsPartial", Model);
        }
    }
}