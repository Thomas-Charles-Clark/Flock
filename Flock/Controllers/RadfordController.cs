using Flock.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Flock.Controllers
{
    public class RadfordController : Controller
    {
        // GET: Radford
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderExport()
        {
            ViewBag.PriceListID = 17;
            return View("OrderExport/OrderExport");
        }
        
        public FileStreamResult ExportDownload(Byte PriceListID, List<Int32> OrderHeadIDs)
        {
            var results = new FlockDataContext().Order_Export(PriceListID, OrderHeadIDs);

            MemoryStream stream = WriteCSV(results);

            return File(stream, "text/csv", "Orders.csv");
        }

        public ActionResult OrderGrid()
        {
            var model = new FlockDataContext().OrderHeads
                .Where(p => p.Transaction.PriceListID == 17 && p.Transaction.DeletedDT == null && p.Transaction.ReleasedDT != null)
                .Select(p => new {
                    OrderHeadID = p.OrderHeadID,
                    TotalInc = p.OrderTotalInc,
                    TotalEx = p.OrderTotalEx,
                    CreatedDT = p.CreatedDT,
                    FullName = p.FullName,
                    Email = p.Transaction.Member.Email,
                    ExportedDT = p.ExportedDT
                }).ToList();

            return PartialView("OrderExport/_OrderGridPartial", model);
        }

        public MemoryStream WriteCSV<T>(IEnumerable<T> items)
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var stream = new MemoryStream();

            var writer = new StreamWriter(stream);
            
            writer.WriteLine(string.Join(",", props.Select(p => p.Name.Replace("_", " "))));

            foreach (var item in items)
            {
                writer.WriteLine(string.Join(",", props.Select(p => p.GetValue(item, null))));
            }

            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            
            return stream;
        }
    }
}