using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Flock.ViewModels;
using NLog;
using Flock.DAL;
using Flock.Models;

namespace Flock.Controllers
{
    public partial class CallsToActionController : Controller
    {
        public ActionResult Styles()
        {
            return View("Styles/Index");
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            var model = UoW.GetRepository<Style>().GetAll().Where(p => p.DeletedDT == null).ToList();
            return PartialView("Styles/_GridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew(StylesCreateModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UoW.GetRepository<Style>().Add(new Style() { 
                        Name = item.Name,
                        FontID = 1
                    });

                    UoW.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var model = UoW.GetRepository<Style>().GetAll().Where(p => p.DeletedDT == null).ToList();

            return PartialView("Styles/_GridViewPartial", model);
        }

        public ActionResult LoadStyleEditForm(Int16 StyleID)
        {
            var model = new StylesEditModel(UoW, StyleID);

            return PartialView("Styles/_StyleEditFormPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate(StylesEditModel item)
        {
            item.RebuildLists(UoW);

            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = UoW.GetRepository<Style>().GetAll().Where(p => p.StyleID == item.StyleID).Single();

                    modelItem.Name = item.Name;
                    modelItem.FontID = item.FontID;
                    modelItem.Margin = item.Margin;
                    modelItem.Padding = item.Padding;
                    modelItem.BackgroundColour = item.BackgroundColour;
                    modelItem.Colour = item.Colour;
                    modelItem.BorderColour = item.BorderColour;
                    modelItem.BorderWidth = item.BorderWidth;
                    modelItem.BorderStyle = item.BorderStyle;
                    modelItem.Opacity = item.Opacity;

                    UoW.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            
            return PartialView("Styles/_StyleEditFormPartial", item);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete(StylesEditModel item)
        {
            try
            {
                var modelItem = UoW.GetRepository<Style>().GetAll().Where(p => p.StyleID == item.StyleID).FirstOrDefault();

                if (modelItem != null)
                    modelItem.DeletedDT = DateTime.Now;

                UoW.Save();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            var model = UoW.GetRepository<Style>().GetAll().Where(p => p.DeletedDT == null).ToList();

            return PartialView("Styles/_GridViewPartial", model);
        }
    }
}