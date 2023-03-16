using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;
using Flock.DAL;
using System.ComponentModel.DataAnnotations;

namespace Flock.ViewModels
{
    public class AdvertisingImagesViewModel : BaseViewModel
    {
        public List<AdvertisingImage> Images { get; set; }

        public AdvertisingImagesViewModel(UnitOfWork<FlockDataContext> _UoW)
        {
            Images = _UoW.GetRepository<AdvertisingImage>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.SortOrder).ToList();
            Images.ForEach((i) => { i.WebEnabled = _UoW.Context.fnc_DateControl(i.OnDate, i.OffDate, DateTime.Now); });
        }
    }

    public class AdvertisingImageEditModel
    {
        public int AdvertisingImageID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(4000, MinimumLength = 5)]
        public string Description { get; set; }

        public string WebQualityFilename { get; set; }

        public string PrintQualityFilename { get; set; }

        public string ThumbnailFilename { get; set; }

        public string UniqueThumbnailFilename { get; set; }

        public DateTime? OnDate { get; set; }

        public DateTime? OffDate { get; set; }

        public AdvertisingImageEditModel()
        {
        }

        public AdvertisingImageEditModel(int _advertisingImageID, UnitOfWork<FlockDataContext> _UoW)
        {
            var item = _UoW.GetRepository<AdvertisingImage>().GetAll().Where(p => p.AdvertisingImageID == _advertisingImageID).Single();

            AdvertisingImageID = _advertisingImageID;
            Name = item.Name;
            Description = item.Description;
            ThumbnailFilename = item.ThumbnailFilename;
            UniqueThumbnailFilename = item.UniqueThumbnailFilename;
            WebQualityFilename = item.WebQualityFilename;
            PrintQualityFilename = item.PrintQualityFilename;
            OnDate = item.OnDate;
            OffDate = item.OffDate;
        }
    }
}

namespace Flock.Models
{
    public partial class AdvertisingImage
    {
        public bool? WebEnabled { get; set; }
    }
}