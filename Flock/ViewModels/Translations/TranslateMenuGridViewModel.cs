using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateMenuGridViewModel
    {
        public List<Menu_Language_SelectList_MenuResult> Menus { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateMenuGridViewModel()
        {
            Menus = new List<Menu_Language_SelectList_MenuResult>();
        }

        public TranslateMenuGridViewModel(
            List<Menu_Language_SelectList_MenuResult> menus,
            byte? targetLanguageId)
        {
            Menus = menus;
            TargetLanguageID = targetLanguageId;
        }
    }
}