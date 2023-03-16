using Flock.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class BundlePriceImportViewModel : BaseViewModel
    {
        [Required]
        public string ThirdParty { get; set; }
    }

    public class BundlePriceImportResultViewModel
    {
        public string FileName { get; set; }
        public string[] ErrorTexts { get; set; }
        public List<Flock.Controllers.BundlePriceImportExcelRow> UploadedRows { get; set; }
    }

    public class BundlePriceMergeResultViewModel
    {
        public List<Flock.Controllers.BundlePriceImportExcelRow> AllRecords { get; set; }
        public List<Flock.Controllers.BundlePriceImportExcelRow> Rejected { get; set; }
        public List<Flock.Controllers.BundlePriceImportExcelRow> Accepted { get; set; }

        public List<BundlePriceImport> BundlePrices
        {
            set
            {

            }
            get
            {
                return AllRecords.Select(p => new BundlePriceImport()
                {
                    NewBundleGrouping = p.NewBundleGrouping,
                    BundleProductCode = p.BundleProductCode,
                    BundleName = p.BundleName,
                    ComponentProductCode = p.ComponentProductCode,
                    ComponentQuantity = p.ComponentQuantity,
                    PriceListCode = p.PriceListCode,
                    PriceListName = p.PriceListName,
                    OriginalBundlePrice = p.OriginalBundlePrice,
                    DiscountedBundlePrice = p.DiscountedBundlePrice,
                    OriginalComponentPrice = p.OriginalComponentPrice,
                    DiscountedComponentPrice = p.DiscountedComponentPrice
                }).ToList();
            }
        }
    }
}