using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;

namespace Flock.Controllers
{
    public class RedirectsController : Controller
    {
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        //
        // GET: /Redirects/
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult RedirectsGridViewPartial()
        {
            return PartialView("_RedirectsGridViewPartial");
        }

        [HttpPost]
        public ActionResult RedirectsGridViewPartialBatchUpdate(MVCxGridViewBatchUpdateValues<Flock.Models.Redirect, int> updateValues)
        {
            foreach(var re in updateValues.Update)
            {
                if(updateValues.IsValid(re))
                {
                    var modelItem = UnitOfWork.GetRepository<Redirect>().GetAll().Where(p => p.OldURL == re.OldURL).SingleOrDefault();

                    if(modelItem != null)
                    {
                        modelItem.NewURL = re.NewURL;
                    }
                }
            }

            UnitOfWork.Save();

            foreach(var re in updateValues.Insert)
            {
                if(updateValues.IsValid(re))
                {
                    UnitOfWork.GetRepository<Redirect>().Add(re);
                }
            }

            UnitOfWork.Save();

            return PartialView("_RedirectsGridViewPartial");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RedirectsGridViewPartialAddNew(RedirectViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mI = new Redirect() { OldURL = item.OldURL, NewURL = item.NewURL, CreatedDT = DateTime.Now };
                    UnitOfWork.GetRepository<Redirect>().Add(mI);
                    UnitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_RedirectsGridViewPartial", item);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RedirectsGridViewPartialUpdate(RedirectViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mI = UnitOfWork.GetRepository<Redirect>().GetAll().Where(p => p.RedirectID == item.RedirectID).SingleOrDefault();

                    if (mI != null)
                    {
                        mI.NewURL = item.NewURL;
                        mI.OldURL = item.OldURL;

                        UnitOfWork.Save();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_RedirectsGridViewPartial");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RedirectsGridViewPartialDelete(System.String OldURL)
        {
            //var model = db.Redirects;
            //if (OldURL != null)
            //{
            //    try
            //    {
            //        var item = model.FirstOrDefault(it => it.OldURL == OldURL);
            //        if (item != null)
            //            model.DeleteOnSubmit(item);
            //        db.SubmitChanges();
            //    }
            //    catch (Exception e)
            //    {
            //        ViewData["EditError"] = e.Message;
            //    }
            //}
            return PartialView("_RedirectsGridViewPartial");
        }
	}
}