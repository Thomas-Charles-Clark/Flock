using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.Controllers
{
    public class MenuBuilderController : Controller
    {
        //
        // GET: /MenuBuilder/
        public ActionResult Index()
        {
            return View("~/Views/Admin/MenuBuilder/MenuBuilder.cshtml");
        }
	}
}