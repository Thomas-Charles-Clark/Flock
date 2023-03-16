using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Flock.Controllers
{
    public class DiscrepancyController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("orderformname");
        }

        //
        // GET: /Discrepancy/
        public ActionResult OrderFormName()
        {
            return View();
        }

        Flock.Models.FlockDataContext db = new Flock.Models.FlockDataContext();

        [ValidateInput(false)]
        public ActionResult GridViewPartial(bool? IgnoreCase)
        {
            var model = db.Products.Where(p => IgnoreCase.HasValue ? p.OrderFormName.ToLower() != p.BasketName.ToLower() : p.OrderFormName != p.BasketName);
            return PartialView("_GridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew(Flock.Models.Product item)
        {
            var model = db.Products;
            if (ModelState.IsValid)
            {
                try
                {
                    model.InsertOnSubmit(item);
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate(Flock.Models.Product item)
        {
            var model = db.Products;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ProductID == item.ProductID);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SubmitChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete(System.Int32 ProductID)
        {
            var model = db.Products;
            if (ProductID >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ProductID == ProductID);
                    if (item != null)
                        model.DeleteOnSubmit(item);
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GridViewPartial", model);
        }
	}
}