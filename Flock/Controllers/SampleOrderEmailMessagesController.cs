//using Flock.DAL;
//using Flock.Models;
//using Flock.ViewModels;
//using NLog;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace Flock.Controllers
//{
//    public class SampleOrderEmailMessagesController : Controller
//    {
//        private static Logger Logger = LogManager.GetCurrentClassLogger();
//        private UnitOfWork<FlockDataContext> UoW = new UnitOfWork<FlockDataContext>();

//        protected override void Dispose(bool disposing)
//        {
//            if (!UoW.Disposed)
//                UoW.Dispose();
//            base.Dispose(disposing);
//        }
        
//        // GET: EmailMessages
//        //public ActionResult Index()
//        //{
//        //    var Model = new SampleOrderEmailMessageViewModel(UoW);

//        //    return View(Model);
//        //}
//    }
//}