using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.ViewModels;
using Flock.DAL;
using Flock.Models;

namespace Flock.Controllers
{
    public class MasonryController : Controller
    {
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        // GET: Masonry
        public ActionResult Index()
        {
            var Model = new MasonryTestViewModel();
            Model.Items = new List<MasonryItem>();
            var images = UnitOfWork.GetRepository<Flock.Models.Image>().GetAll().Where(p => p.ImageID >= 500 && p.ImageID < 600).ToList();

            var r = new Random();

            for (var i = 0; i < 100; i++)
            {
                var s = r.Next(1, 4);

                string masonryClass = "masonry-";

                if (s == 1)
                    masonryClass += "w1";
                if (s == 2)
                    masonryClass += "w2";
                if (s == 3)
                    masonryClass += "w3";

                Model.Items.Add(new MasonryItem() { 
                    MasonryClass = masonryClass,
                    Width = 239,
                    Image = images[i]
                });
            }

            return View(Model);
        }
    }
}