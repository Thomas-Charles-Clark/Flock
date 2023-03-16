using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.Controllers
{
    public class VideoController : Controller
    {
        private UnitOfWork<FlockDataContext> UoW = new UnitOfWork<FlockDataContext>();

        // GET: Video
        public ActionResult VideoSearch(byte? BrandID, short? LaunchID, string NameOrID)
        {
            if (NameOrID == "" || NameOrID == "Name, Code or ID")
                NameOrID = null;

            var Model = UoW.Context.Video_Search(null, BrandID, LaunchID, null, null, NameOrID).ToList();

            return View("_VideoSearchDataViewPartial", Model);
        }

        public ActionResult VideoSearchBrandDropDownPartial()
        {
            List<Brand> Model = UoW.GetRepository<Brand>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.BrandID).ToList();
            return PartialView("~/Views/Shared/SearchFilters/BrandDropDownPartial.cshtml", Model);
        }

        public ActionResult VideoSearchLaunchDropDownPartial()
        {
            List<Flock.Models.Launch> Model = UoW.GetRepository<Launch>().GetAll().OrderByDescending(p => p.LaunchDate).ToList();
            return PartialView("~/Views/Shared/SearchFilters/LaunchDropDownPartial.cshtml", Model);
        }

        public ActionResult VideoSearchNameOrIDTextBoxPartial()
        {
            return PartialView("_VideoSearchNameOrIDTextBoxPartial");
        }
    }
}