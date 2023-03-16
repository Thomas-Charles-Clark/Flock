using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using NLog;
using Flock.Models;
using Flock.ViewModels;

namespace Flock.Controllers
{
    public class MenuController : Controller
    {
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            MenuBuilderViewModelBuilder Builder = new MenuBuilderViewModelBuilder(UnitOfWork);

            MenuBuilderViewModel Model = Builder.Build(null);

            return View(Model);
        }
    }
}
