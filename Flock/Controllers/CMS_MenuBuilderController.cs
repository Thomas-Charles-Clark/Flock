using DevExpress.Web.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace Flock.Controllers
{
    public partial class CMSController : Controller
    {
        public ActionResult MenuItemsBuilder()
        {
            return View("MenuBuilder/MenuItemsBuilder");
        }

        public ActionResult MenuItemsBuilderWrapperPartial(short CountryID, DateTime? UseDate, bool? TradeSwitch)
        {
            var Model = UnitOfWork.GetRepository<Flock.Models.Menu>().GetAll().Where(p => p.DeletedDT == null && 
                (UnitOfWork.Context.fnc_DateControl(p.OnDate, p.OffDate, UseDate) == true) &&
                (TradeSwitch == null || p.TradeSwitch == TradeSwitch)).ToList();

            /* where countryrestrictions are null or countryrestrictions contains country */
            var MenuLinker = UnitOfWork.GetRepository<Flock.Models.Menu_Country>().GetAll().ToList();

            var unrestrictedMenuIDs = Model.Select(p => p.MenuID).Where(p => !MenuLinker.Select(q => q.MenuID).Contains(p)).ToList();

            var restrictedMenuIDs = MenuLinker.Where(p => p.CountryID == CountryID).Select(p => p.MenuID).ToList();

            var menusToDisplay = unrestrictedMenuIDs.Concat(restrictedMenuIDs).Distinct().ToList();

            Model = Model.Where(p => menusToDisplay.Contains(p.MenuID)).OrderBy(p => p.SortOrder).ToList();

            return PartialView("MenuBuilder/MenuItemsBuilderWrapperPartial", Model);
        }
        
        public ActionResult MenuItemsBuilderPartial(byte MenuID, bool? TradeSwitch)
        {
            var Model = UnitOfWork.GetRepository<MenuItem>().GetAll().Where(p => p.MenuID == MenuID && p.DeletedDT == null &&
                (TradeSwitch == null || p.TradeSwitch == TradeSwitch)).OrderBy(p => p.SortOrder).ToList();
            ViewBag.MenuID = MenuID;
            return PartialView("MenuBuilder/MenuItemsBuilderPartial", Model);
        }

        public ActionResult MenuItemsColumnGridViewPartial(byte MenuID, byte Column)
        {
            var Model = UnitOfWork.GetRepository<MenuItem>().GetAll().Where(p => p.MenuID == MenuID && p.Column == Column && p.DeletedDT == null).OrderBy(p => p.SortOrder).ToList();

            ViewBag.MenuID = MenuID;
            ViewBag.Column = Column;
            return PartialView("MenuBuilder/MenuItemsColumnGridViewPartial", Model);
        }

        [HttpPost]
        public ActionResult MenuBuilderGridViewDelete(byte MenuID)
        {
            var m = UnitOfWork.GetRepository<Menu>().GetAll().Where(p => p.MenuID == MenuID).Single();
            m.DeletedDT = DateTime.Now;
            UnitOfWork.Save();
                
            var PreModel = UnitOfWork.Context.Menu_Select();

            var Model = new MenuBuilderGridViewModel();

            Model.Menus = PreModel.GetResult<MenuBuilder_MenuSelectResult>().ToList();
            Model.CountryRestrictions = PreModel.GetResult<MenuBuilder_MenuCountrySelectResult>().ToList();

            return PartialView("MenuBuilder/MenuBuilderGridViewPartial", Model);
        }

        [HttpPost]
        public ActionResult MenuItemsColumnGridViewCustomCommand(string CommandType, short MenuItemID, byte MenuID, byte Column, byte DraggingSI, byte TargetSI)
        {
            try
            {
                switch(CommandType)
                {
                    case "DragRow":
                        var t = UnitOfWork.GetRepository<MenuItem>().GetAll().Where(p => p.MenuID == MenuID && p.Column == Column && p.DeletedDT == null).ToList();
                        if(t.Count > 0)
                        {
                            if (TargetSI == UnitOfWork.GetRepository<MenuItem>().GetAll().Where(p => p.MenuID == MenuID && p.Column == Column && p.DeletedDT == null).Select(p => p.SortOrder).Max())
                                UnitOfWork.Context.MenuItem_Move(MenuItemID, TargetSI + 0.5M, MenuID, Column);
                            else
                                UnitOfWork.Context.MenuItem_Move(MenuItemID, TargetSI - 0.5M, MenuID, Column);
                            UnitOfWork.Context.MenuItem_Sort();
                        }
                        else
                        {
                            UnitOfWork.Context.MenuItem_Move(MenuItemID, TargetSI, MenuID, Column);
                            UnitOfWork.Context.MenuItem_Sort();
                        }
                        break;
                    default: break;
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.Message);
            }

            var Model = UnitOfWork.GetRepository<MenuItem>().GetAll().Where(p => p.MenuID == MenuID && p.Column == Column && p.DeletedDT == null).OrderBy(p => p.SortOrder).ToList();

            ViewBag.MenuID = MenuID;
            ViewBag.Column = Column;

            return PartialView("MenuBuilder/MenuItemsColumnGridViewPartial", Model);
        }

        [HttpPost]
        public ActionResult AddNewMenuItem(MenuItem item)
        {
            ViewBag.MenuID = item.MenuID;
            ViewBag.Column = item.Column;

            if (item.Name == null || item.Name == "")
            {
                item.Name = " ";
            }

            if (ModelState.IsValid)
            {
                byte newSortOrder = 0;
                var column = UnitOfWork.GetRepository<MenuItem>().GetAll().Where(p => p.MenuID == item.MenuID && p.Column == item.Column && p.DeletedDT == null).ToList();

                if (column.Count() > 0)
                {
                    newSortOrder = Convert.ToByte(column.Select(p => p.SortOrder).Max() + 1);
                }

                item.SortOrder = newSortOrder;
                item.CreatedDT = DateTime.Now;

                UnitOfWork.GetRepository<MenuItem>().Add(item);
                UnitOfWork.Save();
                UnitOfWork.Context.MenuItem_Language_Initialise();
            }

            var Model = UnitOfWork.GetRepository<MenuItem>().GetAll().Where(p => p.MenuID == item.MenuID && p.Column == item.Column && p.DeletedDT == null).OrderBy(p => p.SortOrder).ToList();

            return PartialView("MenuBuilder/MenuItemsColumnGridViewPartial", Model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateMenuItem(MenuItem item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Rec = UnitOfWork.GetRepository<MenuItem>().GetAll().Where(p => p.MenuItemID == item.MenuItemID).Single();
                    Rec.Name = item.Name;

                    Rec.FontClassID = item.FontClassID;
                    Rec.PageID = item.PageID;
                    Rec.ProductPageID = item.ProductPageID;
                    Rec.ArticleID = item.ArticleID;
                    Rec.CollectionID = item.CollectionID;
                    Rec.ExternalURL = item.ExternalURL == null ? null : item.ExternalURL.ToLowerInvariant().TrimEnd(new[] { '/' });
                    Rec.BannerLocationID = item.BannerLocationID;
                    Rec.NewWindow = item.NewWindow;
                    Rec.Header = item.Header;
                    Rec.NoFollow = item.NoFollow;
                    Rec.QueryStringParameters = item.QueryStringParameters;
                    Rec.TrackExternalURL = item.TrackExternalURL;
                    Rec.TextAlign = item.TextAlign;
                    Rec.TradeSwitch = item.TradeSwitch;

                    UnitOfWork.Save();

                    // Adjust the text align of items in the same menu & column...
                    var itemsInSameColumn = UnitOfWork.GetRepository<MenuItem>().GetAll().Where(p => p.MenuID == item.MenuID && p.Column == item.Column && p.DeletedDT == null).ToList();

                    foreach (var o in itemsInSameColumn)
                    {
                        o.TextAlign = item.TextAlign;
                    }

                    UnitOfWork.Save();
                }

                var Model = UnitOfWork.GetRepository<MenuItem>().GetAll().Where(p => p.MenuID == item.MenuID && p.Column == item.Column && p.DeletedDT == null).OrderBy(p => p.SortOrder).ToList();

                ViewBag.MenuID = item.MenuID;
                ViewBag.Column = item.Column;

                return PartialView("MenuBuilder/MenuItemsColumnGridViewPartial", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult DeleteMenuItem(MenuItem item)
        {
            try
            {
                var Rec = UnitOfWork.GetRepository<MenuItem>().GetAll().Where(p => p.MenuItemID == item.MenuItemID).Single();
                Rec.DeletedDT = DateTime.Now;

                UnitOfWork.Save();
                    
                UnitOfWork.Context.MenuItem_Sort();
                
                var Model = UnitOfWork.GetRepository<MenuItem>().GetAll().Where(p => p.MenuID == item.MenuID && p.Column == item.Column && p.DeletedDT == null).OrderBy(p => p.SortOrder).ToList();

                ViewBag.MenuID = item.MenuID;
                ViewBag.Column = item.Column;

                return PartialView("MenuBuilder/MenuItemsColumnGridViewPartial", Model);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }

        public ActionResult MenuBuilderPageSelectorGridPartial(string extraName)
        {
            return PartialView("MenuBuilderPageSelectorGridPartial", extraName);
        }

        public ActionResult MenuBuilderArticleSelectorGridPartial(string extraName)
        {
            return PartialView("MenuBuilderArticleSelectorGridPartial", extraName);
        }

        public ActionResult MenuBuilderProductPageSelectorGridPartial(string extraName)
        {
            return PartialView("MenuBuilderProductPageSelectorGridPartial", extraName);
        }

        public ActionResult MenuBuilderCollectionSelectorGridPartial(string extraName)
        {
            return PartialView("MenuBuilderCollectionSelectorGridPartial", extraName);
        }
    
        public ActionResult MenuBuilder()
        {
            return View("MenuBuilder/MenuBuilder");
        }

        public ActionResult MenuBuilderGridViewPartial()
        {
            var PreModel = UnitOfWork.Context.Menu_Select();

            var Model = new MenuBuilderGridViewModel();

            Model.Menus = PreModel.GetResult<MenuBuilder_MenuSelectResult>().ToList();
            Model.CountryRestrictions = PreModel.GetResult<MenuBuilder_MenuCountrySelectResult>().ToList();

            return PartialView("MenuBuilder/MenuBuilderGridViewPartial", Model);
        }

        public ActionResult MenuBuilderGridViewPartialAddNew([ModelBinderAttribute(typeof(DevExpressEditorsBinder))] Menu item)
        {
            if(item.Name != null && item.Name != "")
            {
                var so = Convert.ToByte(UnitOfWork.GetRepository<Menu>().GetAll().Where(p => p.DeletedDT == null).Select(p => p.SortOrder).Max() + 1);
                var mi = Convert.ToByte(UnitOfWork.GetRepository<Menu>().GetAll().Select(p => p.MenuID).Max() + 1);

                Menu m = new Menu();

                m.MenuID = mi;
                m.SortOrder = so;
                m.LocalName = item.Name;
                m.Name = item.Name;
                m.SortOrder = so;
                m.CreatedDT = DateTime.Now;
                m.MessageHTML = item.MessageHTML;
                m.TradeMessageHTML = item.TradeMessageHTML;
                m.NewWindow = item.NewWindow;
                m.PageID = item.PageID;
                m.ExternalURL = item.ExternalURL == null ? null : item.ExternalURL.ToLowerInvariant().TrimEnd(new[] { '/' });
                m.FontClassID = item.FontClassID;
                m.NoFollow = item.NoFollow;
                m.QueryStringParameters = item.QueryStringParameters;
                m.TrackExternalURL = item.TrackExternalURL;
                m.OnDate = item.OnDate;
                m.OffDate = item.OffDate;
                m.TradeSwitch = item.TradeSwitch;

                UnitOfWork.GetRepository<Menu>().Add(m);

                UnitOfWork.Save();

                UnitOfWork.Context.Menu_Language_Initialise();
            }

            var PreModel = UnitOfWork.Context.Menu_Select();

            var Model = new MenuBuilderGridViewModel();

            Model.Menus = PreModel.GetResult<MenuBuilder_MenuSelectResult>().ToList();
            Model.CountryRestrictions = PreModel.GetResult<MenuBuilder_MenuCountrySelectResult>().ToList();

            return PartialView("MenuBuilder/MenuBuilderGridViewPartial", Model);
        }

        public ActionResult MenuBuilderGridViewPartialUpdate([ModelBinderAttribute(typeof(DevExpressEditorsBinder))] Menu item)
        {
            if(ModelState.IsValid)
            {
                var menu = UnitOfWork.GetRepository<Menu>().GetAll().Where(p => p.MenuID == item.MenuID).Single();

                if (item.ExternalURL != null)
                    item.PageID = null;

                if (item.MessageHTML == "")
                    item.MessageHTML = null;

                menu.LocalName = item.LocalName;
                menu.Name = item.Name;
                menu.MessageHTML = item.MessageHTML;
                menu.TradeMessageHTML = item.TradeMessageHTML;
                menu.NewWindow = item.NewWindow;
                menu.PageID = item.PageID;
                menu.ExternalURL = item.ExternalURL == null ? null : item.ExternalURL.ToLowerInvariant().TrimEnd(new[] { '/' });
                menu.FontClassID = item.FontClassID;
                menu.NoFollow = item.NoFollow;
                menu.QueryStringParameters = item.QueryStringParameters;
                menu.TrackExternalURL = item.TrackExternalURL;
                menu.OnDate = item.OnDate;
                menu.OffDate = item.OffDate;
                menu.TradeSwitch = item.TradeSwitch;

                UnitOfWork.Save();
            }

            var PreModel = UnitOfWork.Context.Menu_Select();

            var Model = new MenuBuilderGridViewModel();

            Model.Menus = PreModel.GetResult<MenuBuilder_MenuSelectResult>().ToList();
            Model.CountryRestrictions = PreModel.GetResult<MenuBuilder_MenuCountrySelectResult>().ToList();

            return PartialView("MenuBuilder/MenuBuilderGridViewPartial", Model);
        }

        public ActionResult MenuBuilderDetailPartial(byte MenuID)
        {
            var m = UnitOfWork.GetRepository<Menu>().GetAll().Where(p => p.MenuID == MenuID).Single();
            var mc = UnitOfWork.GetRepository<Menu_Country>().GetAll().Where(p => p.MenuID == MenuID).Select(p => p.CountryID).ToList();
            var c = UnitOfWork.GetRepository<Country>().GetAll().Where(p => mc.Contains(p.CountryID)).ToList();
            var Model = new MenuBuilderDetailViewModel();
            Model.MenuID = MenuID;
            Model.Name = m.Name;
            Model.MessageHTML = m.MessageHTML;
            Model.SortOrder = m.SortOrder;
            Model.CountryRestrictions = c;

            return PartialView("MenuBuilder/MenuBuilderDetailPartial", Model);
        }

        [HttpPost]
        public ActionResult UpdateMenuCountryRestrictions(byte MenuID, string Countries)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> CountryRestrictions = ser.Deserialize<List<SortDatum>>(Countries);

            List<Menu_Country> Current = UnitOfWork.GetRepository<Menu_Country>().GetAll().Where(p => p.MenuID == MenuID).ToList();
            List<SortDatum> CurrentCountries = new List<SortDatum>();
            foreach (Menu_Country country in Current)
            {
                CurrentCountries.Add(new SortDatum() { ID = country.CountryID, SortOrder = 0 });
            }
            List<SortDatum> Removals = CurrentCountries.Except(CountryRestrictions).ToList();

            foreach (SortDatum datum in Removals)
            {
                Menu_Country remove = UnitOfWork.GetRepository<Menu_Country>().GetAll().Where(p => p.MenuID == MenuID && p.CountryID == datum.ID).Single();
                UnitOfWork.GetRepository<Menu_Country>().Delete(remove);
            }
            UnitOfWork.Save();

            foreach (SortDatum datum in CountryRestrictions)
            {
                Menu_Country ppc = UnitOfWork.GetRepository<Menu_Country>().GetAll().Where(p => p.MenuID == MenuID && p.CountryID == datum.ID).SingleOrDefault();

                if (ppc == null)
                {
                    ppc = new Menu_Country() { MenuID = MenuID, CountryID = (short)datum.ID };
                    UnitOfWork.GetRepository<Menu_Country>().Add(ppc);
                }
            }

            UnitOfWork.Save();

            var m = UnitOfWork.GetRepository<Menu>().GetAll().Where(p => p.MenuID == MenuID).Single();
            var mc = UnitOfWork.GetRepository<Menu_Country>().GetAll().Where(p => p.MenuID == MenuID).Select(p => p.CountryID).ToList();
            var c = UnitOfWork.GetRepository<Country>().GetAll().Where(p => mc.Contains(p.CountryID)).ToList();
            var Model = new MenuBuilderDetailViewModel();
            Model.MenuID = MenuID;
            Model.Name = m.Name;
            Model.MessageHTML = m.MessageHTML;
            Model.SortOrder = m.SortOrder;
            Model.CountryRestrictions = c;

            return PartialView("MenuBuilder/MenuBuilderDetailPartial", Model);
        }

        [HttpPost]
        public ActionResult MenuBuilderGridViewCustomCommand(string CommandType, byte MenuID, byte DraggingSI, byte TargetSI)
        {
            try
            {
                switch (CommandType)
                {
                    case "DragRow":
                        var t = UnitOfWork.GetRepository<Menu>().GetAll().Where(p => p.MenuID == MenuID && p.DeletedDT == null).ToList();
                        if (t.Count > 0)
                        {
                            if (TargetSI == UnitOfWork.GetRepository<Menu>().GetAll().Where(p => p.MenuID == MenuID && p.DeletedDT == null).Select(p => p.SortOrder).Max())
                                UnitOfWork.Context.Menu_Move(MenuID, TargetSI + 0.5M);
                            else
                                UnitOfWork.Context.Menu_Move(MenuID, TargetSI - 0.5M);
                            UnitOfWork.Context.Menu_Sort();
                        }
                        else
                        {
                            UnitOfWork.Context.Menu_Move(MenuID, TargetSI);
                            UnitOfWork.Context.Menu_Sort();
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

            var PreModel = UnitOfWork.Context.Menu_Select();

            var Model = new MenuBuilderGridViewModel();

            Model.Menus = PreModel.GetResult<MenuBuilder_MenuSelectResult>().ToList();
            Model.CountryRestrictions = PreModel.GetResult<MenuBuilder_MenuCountrySelectResult>().ToList();

            return PartialView("MenuBuilder/MenuBuilderGridViewPartial", Model);
        }


        /*

    @OriginalMenuItemID SMALLINT,
    @MenuID TINYINT = NULL,
    @Column TINYINT = NULL
        */

        [HttpPost]
        public ActionResult MenuCopy(byte originMenuID, byte? destinationMenuID)
        {
            try
            {
                UnitOfWork.Context.Menu_Copy(originMenuID, destinationMenuID);
            }
            catch(Exception ex)
            {
                Response.StatusCode = 500;
                Logger.Error(ex);
                return Content(ex.ToString());
            }

            return Content("ok");
        }
    }
}