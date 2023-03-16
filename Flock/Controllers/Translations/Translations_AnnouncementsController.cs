using DevExpress.Web.Mvc;
using Flock.Models;
using Flock.ViewModels;
using Google.Apis.Translate.v2;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Flock.Controllers
{
    public partial class TranslationsController : Controller
    {
        public ActionResult Announcements(string targetLanguage)
        {
            var Model = new TranslateAnnouncementViewModel();

            targetLanguage = null;
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
            return View("Announcements/TranslateAnnouncement", Model);
        }

        public ActionResult TranslateAnnouncementTargetLanguageDropDown(byte? targetLanguageId)
        {
            var PreModel = UnitOfWork.GetRepository<Country_Language>().GetAll().Where(p => (p.OffDate == null) || (p.OffDate > DateTime.Now)).OrderBy(p => p.CountryID).ToList();

            var Model = UnitOfWork.GetRepository<Language>().GetAll().Where(p => PreModel.Select(q => q.LanguageID).Distinct().Contains(p.LanguageID) && p.LanguageID != 1).ToList();
            
            ViewBag.TargetLanguageID = targetLanguageId;

            return PartialView("Shared/_TargetLanguageDropDown", Model);
        }

        public ActionResult TranslateAnnouncementGridView(string commandType, short? announcementId, byte? languageId)
        {
            try
            {
                switch (commandType)
                {
                    case "Clear":
                        var announcement = UnitOfWork.GetRepository<Models.Announcement_Language>().GetAll().Where(p => p.AnnouncementID == announcementId && p.LanguageID == languageId).Single();
                        announcement.Text = null;
                        announcement.Stage = null;
                        announcement.ModifiedDT = DateTime.Now;
                        UnitOfWork.Save();
                        break;
                }

                List<Announcement_Language_SelectList_AnnouncementResult> Announcements = new List<Announcement_Language_SelectList_AnnouncementResult>();

                if (languageId != null)
                {
                    IMultipleResults Results = UnitOfWork.Context.Announcement_Language_SelectList(languageId.Value);

                    Announcements = Results.GetResult<Announcement_Language_SelectList_AnnouncementResult>().ToList();
                }

                TranslateAnnouncementGridViewModel Model = new TranslateAnnouncementGridViewModel(Announcements, languageId);

                return PartialView("Announcements/_TranslateAnnouncementGridView", Model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult TranslateAnnouncementGridViewUpdate(Announcement_Language_SelectList_AnnouncementResult item)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    TranslateAnnouncementGridViewModel saveModel = new TranslateAnnouncementGridViewModel();
                    saveModel.Announcements.Add(item);
                    UnitOfWork.Context.Announcement_Language_Save(item.LanguageID, saveModel);

                    List<Announcement_Language_SelectList_AnnouncementResult> Announcements = new List<Announcement_Language_SelectList_AnnouncementResult>();
                    
                    IMultipleResults Results = UnitOfWork.Context.Announcement_Language_SelectList(item.LanguageID);

                    Announcements = Results.GetResult<Announcement_Language_SelectList_AnnouncementResult>().ToList();

                    TranslateAnnouncementGridViewModel Model = new TranslateAnnouncementGridViewModel(Announcements, item.LanguageID);

                    return PartialView("Announcements/_TranslateAnnouncementGridView", Model);
                }
                else
                {
                    List<Announcement_Language_SelectList_AnnouncementResult> Announcements = new List<Announcement_Language_SelectList_AnnouncementResult>();

                    IMultipleResults Results = UnitOfWork.Context.Announcement_Language_SelectList(item.LanguageID);

                    Announcements = Results.GetResult<Announcement_Language_SelectList_AnnouncementResult>().ToList();

                    TranslateAnnouncementGridViewModel Model = new TranslateAnnouncementGridViewModel(Announcements, item.LanguageID);

                    return PartialView("Announcements/_TranslateAnnouncementGridView", Model);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.Message);
            }    
        }
    }
}