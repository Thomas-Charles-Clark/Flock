using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL.Mertex;

namespace Flock.Controllers
{
    public class MertexController : Controller
    {
        //
        // GET: /Mertex/
        public ActionResult Index()
        {
            return RedirectToAction("overview");
        }

        public ActionResult Overview()
        {
            string Username = "B2C.LIVE";
            string Password = "RedAnt";
            string IP = "";
            string ID = "0";

            var MertexConnection = new MertexConnection(Username, Password, ID, IP);

            ViewData["MertexLoginResult"] = MertexConnection.VerifyLogin();

            var MertexStock = new MertexStock(MertexConnection);

            var StockRawRes = MertexStock.StockRaw(null);

            ViewData["MertexStockRawResult"] = StockRawRes;

            return View();
        }

        public ActionResult OrderSearchResultPartial(string OrderRef)
        {
            return PartialView();
        }
	}
}