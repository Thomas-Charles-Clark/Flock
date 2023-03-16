using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using Flock.Models;
//using Flock.Classes.Auth;
using System.Xml;
using Flock.DAL;
using Flock.ViewModels;

namespace Flock.Controllers
{
    public class AdminController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        //[Authorize(Roles="Administrator")]
        ////[CustomAuthorize(Roles="Administrator")]
        public ActionResult Index()
        {
            AdminViewModelBuilder Builder = new AdminViewModelBuilder(UnitOfWork);

            AdminViewModel Model = Builder.Build(null); 

            return View(Model);
        }

        [AllowAnonymous]
        public ActionResult MenuBuilder()
        {
            return View();
        }
    }
}