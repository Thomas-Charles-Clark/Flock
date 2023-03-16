using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateMenuItemGridViewModel
    {
        public List<Menu_SelectResult> Menus { get; set; }
        public List<MenuItem_Language_SelectList_MenuItemResult> MenuItems { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateMenuItemGridViewModel()
        {
            MenuItems = new List<MenuItem_Language_SelectList_MenuItemResult>();
            Menus = new List<Menu_SelectResult>();
        }

        public TranslateMenuItemGridViewModel(
            List<MenuItem_Language_SelectList_MenuItemResult> menuItems,
            List<Menu_SelectResult> menus,
            byte? targetLanguageId)
        {
            MenuItems = menuItems;
            Menus = menus;
            TargetLanguageID = targetLanguageId;
        }
    }
}