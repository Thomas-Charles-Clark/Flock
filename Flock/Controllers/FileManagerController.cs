using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Flock.Controllers
{
    public class FileManagerController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        //
        // GET: /FileManager/

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult FileManagerPartial()
        {
            return PartialView("_FileManagerPartial", FileManagerControllerFileManagerSettings.Model);
        }

        public FileStreamResult FileManagerPartialDownload()
        {
            return FileManagerExtension.DownloadFiles("FileManager", FileManagerControllerFileManagerSettings.Model);
        }
    }

    public class FileManagerControllerFileManagerSettings
    {
        public const string RootFolder = @"~\";

        public static string Model { get { return RootFolder; } }
    }
}
