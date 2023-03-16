using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.Controllers
{
    public class PreviewController : Controller
    {
        //
        // GET: /Preview/
        public ActionResult Index()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "https://flock-preview.designersguild.com");
            return View();
        }
	}
}