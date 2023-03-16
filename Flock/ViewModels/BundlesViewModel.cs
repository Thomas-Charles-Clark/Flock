using Flock.DAL;
using Flock.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class BundlesViewModel : BaseViewModel
    {
        public List<PriceList> PriceLists { get; set; }

        public BundlesViewModel() { }

        public BundlesViewModel(UnitOfWork<FlockDataContext> UoW)
        {
            PriceLists = UoW.GetRepository<PriceList>().GetAll().ToList();
        }
    }

    public class BundleEditModel
    {
        public int? BundleID { get; set; }

        [Required(ErrorMessage="You must provide a Code")]
        [StringLength(50)]
        public string Code { get; set; }
        
        [Required(ErrorMessage = "You must provide a Brand")]
        public byte? BrandID { get; set; }

        [Required(ErrorMessage = "You must provide a Launch")]
        public short? LaunchID { get; set; }

        [Required(ErrorMessage = "You must provide a Product Type")]
        public byte? ProductTypeID { get; set; }

        [Required(ErrorMessage  = "You must provide an Order Form Name")]
        [StringLength(100)]
        public string OrderFormName { get; set; }

        [Required(ErrorMessage = "You must provide a Basket Name")]
        [StringLength(100)]
        public string BasketName { get; set; }

        public List<LaunchNonEntity> Launches { get; set; }

        public List<ProductTypeNonEntity> ProductTypes { get; set; }

        public List<OrderFormGroup> OrderFormGroups { get; set; }

        public List<Brand> Brands { get; set; }

        public List<ProductBundle> BundledProducts { get; set; }

        [Required(ErrorMessage = "Empty bundles are not permitted")]
        public string ProductBundle { get; set; }

        public BundleEditModel() { }
        
        public BundleEditModel(int? _bundleId, UnitOfWork<FlockDataContext> UoW)
        {
            BundleID = _bundleId;
            
            Launches = UoW.GetRepository<Launch>().GetAll().Select(p => new ViewModels.LaunchNonEntity() { LaunchID = p.LaunchID, Name = p.Name }).OrderByDescending(p => p.LaunchID).ToList();

            ProductTypes = UoW.GetRepository<ProductType>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.SortOrder).Select(p => new ProductTypeNonEntity() { ProductTypeID = p.ProductTypeID, Name = p.Name }).ToList();

            OrderFormGroups = UoW.GetRepository<OrderFormGroup>().GetAll().OrderBy(p => p.Name).ToList();

            Brands = UoW.GetRepository<Brand>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.SortOrder).ToList();

            if (BundleID != null)
            {
                var b = UoW.GetRepository<Product>().GetAll().Where(p => p.ProductID == _bundleId).Single();

                Code = b.Code;
                BrandID = b.BrandID;
                LaunchID = b.LaunchID;
                ProductTypeID = b.ProductTypeID;
                OrderFormName = b.OrderFormName;
                BasketName = b.BasketName;

                BundledProducts = UoW.GetRepository<ProductBundle>().GetAll().Where(p => p.BundleProductID == BundleID).ToList();

                ProductBundle = JsonConvert.SerializeObject(BundledProducts.Select(bp => new { bp.ProductID, bp.Quantity, bp.BundleProductID }));
            }
            else
            {
                BundledProducts = new List<ProductBundle>();

                ProductBundle = null;
            }
        }

        public void RebuildRequirements(UnitOfWork<FlockDataContext> UoW)
        {
            Launches = UoW.GetRepository<Launch>().GetAll().Select(p => new ViewModels.LaunchNonEntity() { LaunchID = p.LaunchID, Name = p.Name }).OrderByDescending(p => p.LaunchID).ToList();

            ProductTypes = UoW.GetRepository<ProductType>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.SortOrder).Select(p => new ProductTypeNonEntity() { ProductTypeID = p.ProductTypeID, Name = p.Name }).ToList();

            OrderFormGroups = UoW.GetRepository<OrderFormGroup>().GetAll().OrderBy(p => p.Name).ToList();

            Brands = UoW.GetRepository<Brand>().GetAll().Where(p => p.DeletedDT == null).OrderBy(p => p.SortOrder).ToList();

            if (BundledProducts == null)
            {
                BundledProducts = new List<ProductBundle>();
            }
            else
            {
                BundledProducts.ForEach(bp =>
                {
                    bp.Product = UoW.GetRepository<Product>().GetAll().Single(p => p.ProductID == bp.ProductID);
                });
            }
        }
    }
}