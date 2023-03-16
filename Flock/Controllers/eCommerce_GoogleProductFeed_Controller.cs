using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.Controllers
{
    public partial class eCommerceController : Controller
    {
        public ActionResult GoogleProductFeedDelivery()
        {
            var Model = new GoogleProductFeedDeliveryViewModel();

            return View("~/Views/eCommerce/GoogleProductFeedDelivery/Index.cshtml", Model);
        }

        [HttpPost]
        public ActionResult UpdateGoogleFeedDefaultPrices(objUpdateDefaultPrice objUpdate)
        {
            try
            {
                var UoW = new UnitOfWork<FlockDataContext>();
                
                foreach(var change in objUpdate.UnsavedChanges)
                {
                    var rec = UoW.GetRepository<DefaultPrice>().GetAll().Where(p => p.PriceListID == change.PriceListID && p.ProductTypeID == change.ProductTypeID).SingleOrDefault();
                    
                    if (rec == null)
                    {
                        UoW.GetRepository<DefaultPrice>().Add(new DefaultPrice()
                        {
                            PriceListID = change.PriceListID,
                            ProductTypeID = change.ProductTypeID,
                            PriceInc = change.PriceInc,
                            PriceEx = change.PriceEx
                        });
                    }
                    else
                    {
                        rec.PriceEx = change.PriceEx;
                        rec.PriceInc = change.PriceInc;
                    }
                }

                UoW.Save();

                return Json(new { message = "success" }, JsonRequestBehavior.DenyGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { message = ex.ToString() }, JsonRequestBehavior.DenyGet);
            }
        }
    }
}