using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateAnnouncementGridViewModel
    {
        public List<Announcement_Language_SelectList_AnnouncementResult> Announcements { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateAnnouncementGridViewModel()
        {
            Announcements = new List<Announcement_Language_SelectList_AnnouncementResult>();
        }

        public TranslateAnnouncementGridViewModel(
            List<Announcement_Language_SelectList_AnnouncementResult> announcements,
            byte? targetLanguageId)
        {
            Announcements = announcements;
            TargetLanguageID = targetLanguageId;
        }
    }
}