using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Flock.Controllers
{
    public class AddNewImageEditModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class AssetDeliverySystemController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UoW = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UoW.Disposed)
                UoW.Dispose();
            base.Dispose(disposing);
        }

        // GET: AdvertisingImages
        public ActionResult AdvertisingImages()
        {
            var Model = new AdvertisingImagesViewModel(UoW);

            return View(Model);
        }

        public ActionResult ImagesPartial()
        {
            var Model = new AdvertisingImagesViewModel(UoW);

            return PartialView("_ImagesPartial", Model.Images);
        }

        [HttpPost]
        public ActionResult AddNewImage(AddNewImageEditModel model)
        {
            try
            {
                var images = UoW.GetRepository<AdvertisingImage>().GetAll().OrderBy(i => i.SortOrder).ToList();

                images.ForEach(i =>
                {
                    i.SortOrder += 1;
                });

                UoW.Save();

                UoW.GetRepository<AdvertisingImage>().Add(new AdvertisingImage()
                {
                    Name = model.Name,
                    Description = model.Description,
                    CreatedDT = DateTime.Now,
                    SortOrder = 0
                });

                UoW.Save();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.ToString());
            }

            return Content("ok");
        }

        [HttpPost]
        public ActionResult UpdateImage(AdvertisingImageEditModel model)
        {
            var item = UoW.GetRepository<AdvertisingImage>().GetAll().Where(p => p.AdvertisingImageID == model.AdvertisingImageID).Single();

            model.UniqueThumbnailFilename = item.UniqueThumbnailFilename;
            model.ThumbnailFilename = item.ThumbnailFilename;
            model.WebQualityFilename = item.WebQualityFilename;
            model.PrintQualityFilename = item.PrintQualityFilename;

            try
            {
                if (ModelState.IsValid)
                {
                    item.Name = model.Name;
                    item.Description = model.Description;
                    item.OnDate = model.OnDate;
                    item.OffDate = model.OffDate;

                    UoW.Save();
                }
                else
                {
                    throw new Exception("Invalid Model State");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
            }

            return PartialView("_ImageEditFormPartial", model);
        }

        [HttpPost]
        public ActionResult updatesortorders(List<AdvertisingImageSortDatum> AdvertisingImages)
        {
            try
            {
                foreach (var i in AdvertisingImages)
                {
                    var item = UoW.GetRepository<AdvertisingImage>().GetAll().Where(p => p.AdvertisingImageID == i.AdvertisingImageID).Single();

                    item.SortOrder = i.SortOrder;
                }

                UoW.Save();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.ToString());
            }
            return Content("ok");
        }

        [HttpPost]
        public ActionResult DeleteImage(int AdvertisingImageID)
        {
            try
            {
                var i = UoW.GetRepository<AdvertisingImage>().GetAll().Where(p => p.AdvertisingImageID == AdvertisingImageID).Single();

                i.DeletedDT = DateTime.Now;

                UoW.Save();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.ToString());
            }

            return Content("ok");
        }

        public ActionResult ImageEditFormPartial(int AdvertisingImageID)
        {
            var Model = new AdvertisingImageEditModel(AdvertisingImageID, UoW);

            return PartialView("_ImageEditFormPartial", Model);
        }

        public ActionResult ThumbnailUploadControlUpload()
        {
            UploadControlExtension.GetUploadedFiles("ThumbnailUploadControl", AssetDeliverySystemControllerThumbnailUploadControlSettings.UploadValidationSettings, AssetDeliverySystemControllerThumbnailUploadControlSettings.FileUploadComplete);
            return null;
        }

        public ActionResult WebQualityUploadControlUpload()
        {
            UploadControlExtension.GetUploadedFiles("WebQualityUploadControl", AssetDeliverySystemControllerWebQualityUploadControlSettings.UploadValidationSettings, AssetDeliverySystemControllerWebQualityUploadControlSettings.FileUploadComplete);
            return null;
        }

        public ActionResult PrintQualityUploadControlUpload()
        {
            UploadControlExtension.GetUploadedFiles("PrintQualityUploadControl", AssetDeliverySystemControllerPrintQualityUploadControlSettings.UploadValidationSettings, AssetDeliverySystemControllerPrintQualityUploadControlSettings.FileUploadComplete);
            return null;
        }

        public ActionResult MertexMarkets()
        {
            var Model = new MertexMarketViewModel(UoW);

            return View("MertexMarkets", Model);
        }

        [HttpPost]
        public ActionResult UpdateMertexMarket(string Code, Int16 CountryID)
        {
            try
            {
                var modelItem = UoW.GetRepository<Mertex_Market>().GetAll().Where(p => p.Code == Code).Single();
                modelItem.CountryID = CountryID;
                UoW.Save();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.ToString());
            }

            return Content("ok");
        }

        [HttpPost]
        public ActionResult UpdateMarketDisplayPrices(string Code, bool DisplayPrices)
        {
            try
            {
                var modelItem = UoW.GetRepository<Mertex_Market>().GetAll().Where(p => p.Code == Code).Single();
                modelItem.DisplayPrices = DisplayPrices;
                UoW.Save();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.ToString());
            }

            return Content("ok");
        }

        [HttpPost]
        public ActionResult clearMarket(string Code)
        {
            try
            {
                var modelItem = UoW.GetRepository<Mertex_Market>().GetAll().Where(p => p.Code == Code).Single();
                modelItem.CountryID = null;
                UoW.Save();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.ToString());
            }

            return Content("ok");
        }

        public ActionResult Users()
        {
            var Model = new ADS_UsersViewModel();

            return View("Users", Model);
        }

        [ValidateInput(false)]
        public ActionResult UserGridViewPartial()
        {
            return PartialView("_UserGridViewPartial");
        }

        public ActionResult ApprovalRequestEditFormPartial(int ApprovalRequestID)
        {
            var req = UoW.GetRepository<ApprovalRequest>().GetAll().Where(p => p.ApprovalRequestID == ApprovalRequestID)
                .Select(p => new { User = p.User, ApprovalRequest = p })
                .Single();
            
            var Model = new ApprovalRequestEditModel(req.ApprovalRequest, 
                req.User.APIKey != null,
                req.User.PieceBatchStockAPIKeys.Any(),
                req.User.UpcomingSeasonEarlyAccess);

            return PartialView("_ApprovalRequestEditFormPartial", Model);
        }

        public ActionResult UserGridViewPartialUpdate(approvalRequestShunt item)
        {
            if (item.ApprovalStatus == "False" && item.ApprovalExpiryDT == null)
            {
                ModelState.AddModelError("ApprovalExpiryDT", "Please provide an Expiry Date");
            } 

            if(ModelState.IsValid)
            {
                try
                {
                    var user = UoW.GetRepository<User>().GetAll().Where(p => p.Id == item.Id).Single();

                    //if (item.StockFeedAccess)
                    //{
                    //    if (user.APIKey != null) { }
                    //    else
                    //    {
                    //        user.APIKey = Guid.NewGuid();
                    //    }
                    //}\

                    if (item.UpcomingSeasonEarlyAccess)
                        user.UpcomingSeasonEarlyAccess = true;
                    else
                        user.UpcomingSeasonEarlyAccess = false;

                    if (item.BatchPieceStockAccess)
                    {
                        if (user.PieceBatchStockAPIKeys.Any()) { }
                        else
                        {
                            UoW.GetRepository<PieceBatchStockAPIKey>().Add(new PieceBatchStockAPIKey() {
                                UserId = item.Id,
                                APIKey = Guid.NewGuid()
                            });
                        }
                    }
                    else
                    {
                        if (user.PieceBatchStockAPIKeys.Any())
                        {
                            UoW.GetRepository<PieceBatchStockAPIKey>().DeleteAll(user.PieceBatchStockAPIKeys);
                        }
                    }

                    var modelItem = UoW.GetRepository<ApprovalRequest>().GetAll().Where(p => p.ApprovalRequestID == item.ApprovalRequestID).Single();

                    //e.AddValue("Not Approved", "2");
                    //e.AddValue("Timed Approval", "False");
                    //e.AddValue("Approval Expired", "3");
                    //e.AddValue("Indefinitely Approved", "True");

                    //    (c.ApprovalRequest.ApprovalExpiryDT.Value < DateTime.Now && c.ApprovalRequest.ApprovalStatus == false) ?
                    //"3" :
                    //(c.ApprovalRequest.ApprovalStatus.HasValue ?
                    //   c.ApprovalRequest.ApprovalStatus.ToString() :
                    //    "2"),

                    modelItem.ApprovalExpiryDT = item.ApprovalExpiryDT;

                    if (item.ApprovalStatus == "False")
                    {
                        modelItem.ApprovalStatus = false;
                    }
                    if (item.ApprovalStatus == "True")
                    {
                        modelItem.ApprovalStatus = true;
                        modelItem.ApprovalExpiryDT = null;
                    }
                    if (item.ApprovalStatus == "2")
                    {
                        modelItem.ApprovalStatus = null;
                    }
                    
                    if (modelItem.ApprovalStatus == null)
                        modelItem.ApprovalExpiryDT = null;

                    UoW.Save();
                }
                catch(Exception ex)
                {
                    Response.StatusCode = 500;
                    return Content(ex.ToString());
                }
            }

            return PartialView("_UserGridViewPartial", item);
        }

        //public ActionResult ApprovalRequestGridViewPartial(string userId)
        //{
        //    var user = UoW.GetRepository<User>().GetAll().Where(p => p.Id == userId).Single();

        //    var Model = new ApprovalRequestEditModel();
        //    Model.UserID = userId;
        //    Model.MostRecentApprovalRequestID = user.ApprovalRequestID;

        //    return PartialView("_ApprovalRequestGridViewPartial", Model);
        //}

        //public ActionResult ApprovalRequestGridViewUpdate(ApprovalRequestEditModel item)
        //{
        //    var req = UoW.GetRepository<ApprovalRequest>().GetAll().Where(p => p.ApprovalRequestID == item.ApprovalRequestID).Single();

        //    item.MostRecentApprovalRequestID = req.User.ApprovalRequestID;

        //    if (item.ApprovalStatus == false && item.ApprovalExpiryDT == null)
        //    {
        //        ModelState.AddModelError("ApprovalExpiryDT", "Please provide an expiry date.");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        req.ApprovalStatus = item.ApprovalStatus;
        //        req.ApprovalExpiryDT = item.ApprovalExpiryDT;
        //        UoW.Save();

        //        item.ApprovalRequestID = null;

        //        return PartialView("_ApprovalRequestGridViewPartial", item);
        //    }

        //    return PartialView("_ApprovalRequestGridViewPartial", item);
        //}

        public ActionResult ApprovalRequestSetExpiry(int approvalRequestId)
        {
            var req = UoW.GetRepository<ApprovalRequest>().GetAll().Where(p => p.ApprovalRequestID == approvalRequestId).Single();

            req.ApprovalStatus = false; // NULL = not approved, 0 = timed expiry, 1 = indefinite
            req.ApprovalExpiryDT = DateTime.Now.AddMonths(3);

            UoW.Save();

            ViewBag.ApprovalRequestID = approvalRequestId;
            ViewBag.ApprovalExpiryDT = req.ApprovalExpiryDT;

            return View(new BaseViewModel());
        }

        public ActionResult ApprovalRequestSetLongExpiry(int approvalRequestId)
        {
            var req = UoW.GetRepository<ApprovalRequest>().GetAll().Where(p => p.ApprovalRequestID == approvalRequestId).Single();

            req.ApprovalStatus = false; // NULL = not approved, 0 = timed expiry, 1 = indefinite
            req.ApprovalExpiryDT = DateTime.Now.AddMonths(6);

            UoW.Save();

            ViewBag.ApprovalRequestID = approvalRequestId;
            ViewBag.ApprovalExpiryDT = req.ApprovalExpiryDT;

            return View(new BaseViewModel());
        }

        public ActionResult ApprovalRequestSetIndefinite(int approvalRequestId)
        {
            var req = UoW.GetRepository<ApprovalRequest>().GetAll().Where(p => p.ApprovalRequestID == approvalRequestId).Single();

            req.ApprovalStatus = true; // NULL = not approved, 0 = timed expiry, 1 = indefinite
            req.ApprovalExpiryDT = null;

            UoW.Save();

            ViewBag.ApprovalRequestID = approvalRequestId;

            return View(new BaseViewModel());
        }

        public ActionResult Announcements()
        {
            return View(new BaseViewModel());
        }

        public ActionResult AnnouncementGridViewPartial()
        {
            return PartialView("_AnnouncementGridViewPartial");
        }

        public ActionResult AnnouncementGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))]AnnouncementEditModel item)
        {
            if (ModelState.IsValid)
            {
                UoW.GetRepository<Announcement>().Add(
                    new Announcement() { Text = item.Text, OnDate = item.OnDate, OffDate = item.OffDate, CreatedDT = DateTime.Now }
                );
                UoW.Save();
                UoW.Context.Announcement_Language_Initialise();
            }

            return PartialView("_AnnouncementGridViewPartial");
        }

        public ActionResult AnnouncementGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))]AnnouncementEditModel item)
        {
            if (ModelState.IsValid)
            {
                var modelItem = UoW.GetRepository<Announcement>().GetAll().Where(p => p.AnnouncementID == item.AnnouncementID).Single();

                modelItem.Text = item.Text;
                modelItem.OnDate = item.OnDate;
                modelItem.OffDate = item.OffDate;

                UoW.Save();
            }

            return PartialView("_AnnouncementGridViewPartial");
        }

        public ActionResult AnnouncementGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))]AnnouncementEditModel item)
        {
            var modelItem = UoW.GetRepository<Announcement>().GetAll().Where(p => p.AnnouncementID == item.AnnouncementID).Single();
            modelItem.DeletedDT = DateTime.Now;
            UoW.Save();

            return PartialView("_AnnouncementGridViewPartial");
        }
    }

    public class AssetDeliverySystemControllerThumbnailUploadControlSettings
    {
        public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg" },
            MaxFileSize = 4194304
        };

        public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                var uniqueFileName = Guid.NewGuid() + System.IO.Path.GetExtension(e.UploadedFile.FileName);

                e.UploadedFile.SaveAs(HttpRuntime.AppDomainAppPath + "/Images/AssetDistributionSystem/Thumbnails/" + uniqueFileName, true);

                var UoW = new UnitOfWork<FlockDataContext>();

                var AdvertisingImageID = Convert.ToInt32(HttpContext.Current.Request.Params["advertising-image-id"]);

                var item = UoW.GetRepository<AdvertisingImage>().GetAll().Where(p => p.AdvertisingImageID == AdvertisingImageID).Single();

                item.UniqueThumbnailFilename = uniqueFileName;
                item.ThumbnailFilename = e.UploadedFile.FileName;

                UoW.Save();

                e.CallbackData = "{\"filePath\":\"/images/assetdistributionsystem/thumbnails/" + uniqueFileName + "\",\"fileName\":\"" + e.UploadedFile.FileName + "\"}";
            }
        }
    }

    public class AssetDeliverySystemControllerWebQualityUploadControlSettings
    {
        public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg" },
            MaxFileSize = 4194304
        };

        public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                var uniqueFileName = Guid.NewGuid() + System.IO.Path.GetExtension(e.UploadedFile.FileName);

                e.UploadedFile.SaveAs(HttpRuntime.AppDomainAppPath + "/Images/AssetDistributionSystem/WebQuality/" + uniqueFileName, true);

                var UoW = new UnitOfWork<FlockDataContext>();

                var AdvertisingImageID = Convert.ToInt32(HttpContext.Current.Request.Params["advertising-image-id"]);
                
                var item = UoW.GetRepository<AdvertisingImage>().GetAll().Where(p => p.AdvertisingImageID == AdvertisingImageID).Single();

                item.UniqueWebQualityFilename = uniqueFileName;
                item.WebQualityFilename = e.UploadedFile.FileName;

                UoW.Save();

                e.CallbackData = "{\"filePath\":\"/images/assetdistributionsystem/webquality/" + uniqueFileName + "\",\"fileName\":\"" + e.UploadedFile.FileName + "\"}";
            }
        }
    }

    public class AssetDeliverySystemControllerPrintQualityUploadControlSettings
    {
        public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".tif", ".tiff", ".tga", ".targa" },
            MaxFileSize = 500000000
        };

        public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                var uniqueFileName = Guid.NewGuid() + System.IO.Path.GetExtension(e.UploadedFile.FileName);

                e.UploadedFile.SaveAs(HttpRuntime.AppDomainAppPath + "/Images/AssetDistributionSystem/PrintQuality/" + uniqueFileName, true);

                var UoW = new UnitOfWork<FlockDataContext>();

                var AdvertisingImageID = Convert.ToInt32(HttpContext.Current.Request.Params["advertising-image-id"]);

                var item = UoW.GetRepository<AdvertisingImage>().GetAll().Where(p => p.AdvertisingImageID == AdvertisingImageID).Single();

                item.UniquePrintQualityFilename = uniqueFileName;
                item.PrintQualityFilename = e.UploadedFile.FileName;

                UoW.Save();

                e.CallbackData = "{\"filePath\":\"/images/assetdistributionsystem/printquality/" + uniqueFileName + "\",\"fileName\":\"" + e.UploadedFile.FileName + "\"}";
            }
        }
    }

    public class AdvertisingImageSortDatum
    {
        public int AdvertisingImageID { get; set; }
        public int SortOrder { get; set; }
    }
}