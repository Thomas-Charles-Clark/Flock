using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.Controllers
{
    public class GDPRController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        // GET: GDPR
        public ActionResult DataSubjects()
        {
            var currentQueue = UnitOfWork.Context.DataSubjectRequest_Select(null).ToList();

            var Model = new DataSubjectsViewModel(currentQueue);

            return View(Model);
        }
        
        public ActionResult MemberComboBoxPartial()
        {
            return PartialView();
        }
    }
}