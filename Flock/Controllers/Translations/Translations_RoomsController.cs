using DevExpress.Web.Mvc;
using Flock.Models;
using Flock.ViewModels;
using Google.Apis.Translate.v2;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Flock.Controllers
{
    public partial class TranslationsController : Controller
    {
        public ActionResult Rooms(string targetLanguage)
        {
            var Model = new TranslateRoomViewModel();

            if (targetLanguage != null)
            {
                if (targetLanguage.Length > 2 || targetLanguage.Length < 2)
                {
                    ModelState.AddModelError("TargetLanguage", "Invalid language code detected");
                }
                else
                {
                    var lang = UnitOfWork.GetRepository<Language>().GetAll().Where(p => p.Code == targetLanguage).SingleOrDefault();

                    if (lang == null)
                    {
                        ModelState.AddModelError("TargetLanguage", "Invalid language code detected");
                    }
                    else
                    {
                        Model.TargetLanguageID = lang.LanguageID;
                    }
                }
            }

            return View("Rooms/TranslateRoom", Model);
        }

        public ActionResult TranslateRoomTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();

            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateRoomGridView(byte? languageId, string commandType, byte? roomId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var item = UnitOfWork.GetRepository<Models.Room_Language>().GetAll().Where(p => p.RoomID == roomId && p.LanguageID == languageId).Single();
                        item.Name = null;
                        item.Stage = null;
                        item.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<Room_Language_SelectList_RoomResult> Rooms = new List<Room_Language_SelectList_RoomResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Room_Language_SelectList(languageId.Value);

                    Rooms = Results.GetResult<Room_Language_SelectList_RoomResult>().ToList();
                }

                TranslateRoomGridViewModel Model = new TranslateRoomGridViewModel(Rooms, languageId);

                return PartialView("Rooms/_TranslateRoomGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, MethodBase.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateRoomGridViewUpdate(Room_Language_SelectList_RoomResult item, byte languageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveModel = new TranslateRoomGridViewModel();
                    saveModel.Rooms.Add(item);
                    UnitOfWork.Context.Room_Language_Save(languageId, saveModel);

                    List<Room_Language_SelectList_RoomResult> Rooms = new List<Room_Language_SelectList_RoomResult>();
                    IMultipleResults Results = UnitOfWork.Context.Room_Language_SelectList(languageId);
                    Rooms = Results.GetResult<Room_Language_SelectList_RoomResult>().ToList();
                    var Model = new TranslateRoomGridViewModel(Rooms, languageId);

                    return PartialView("Rooms/_TranslateRoomGridView", Model);
                }
                else
                {
                    List<Room_Language_SelectList_RoomResult> Rooms = new List<Room_Language_SelectList_RoomResult>();
                    IMultipleResults Results = UnitOfWork.Context.Room_Language_SelectList(languageId);
                    Rooms = Results.GetResult<Room_Language_SelectList_RoomResult>().ToList();
                    var Model = new TranslateRoomGridViewModel(Rooms, languageId);

                    return PartialView("Rooms/_TranslateRoomGridView", Model);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, MethodBase.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }
    }
}