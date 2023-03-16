using Flock.DAL;
using Flock.Models;
using Flock.ViewModels.Bronto;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bronto;

namespace Flock.Controllers
{
    public class BrontoController : Controller
    {
        /// <summary>
        /// For managing Bronto Lists (for now).
        /// 
        /// Objectives:
        ///     - Create new lists
        ///     - Read & review all lists belonging to a particular Sub Account
        ///     - Add & Remove contacts from lists
        ///     - What about between sub accounts? Is this supposed to be another case of "we have N lists, each exist in all our sub accounts"?
        ///         + Oh, god...
        ///     
        /// 
        /// </summary>

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        // GET: Bronto
        public ActionResult Lists()
        {
            BrontoListsViewModel Model = new BrontoListsViewModel(UnitOfWork);

            return View("Lists/Lists", Model);
        }

        [HttpPost]
        public ActionResult LoadSubAccountLists(Byte SubAccountID)
        {
            BrontoSubAccountListsViewModel Model = new BrontoSubAccountListsViewModel(SubAccountID, UnitOfWork);

            return View("Lists/_SubAccountListsPartial", Model);
        }

        public ActionResult ListGridViewPartial(Byte SubAccountID)
        {
            BrontoListGridViewModel Model = new BrontoListGridViewModel(SubAccountID, UnitOfWork);

            return View("Lists/_ListGridViewPartial", Model);
        }
    }
}