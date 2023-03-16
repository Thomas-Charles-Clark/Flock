using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NLog;

namespace Flock.Controllers.Admin
{
    public class ProductGridViewController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        //
        // GET: /ProductGridView/
        public ActionResult Index()
        {
            return View();
        }
	}

    public partial class GridViewController : Controller
    {
        public ActionResult DataBindingToLargeDatabase()
        {
            return View("DataBindingToLargeDatabase");
        }
        public ActionResult DataBindingToLargeDatabasePartial()
        {
            return PartialView("DataBindingToLargeDatabasePartial");
        }
    }
}