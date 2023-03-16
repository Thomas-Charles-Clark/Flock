using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductPageViewModelBuilder : ViewModelBuilder, IViewModelBuilder<ProductPageViewModel>
    {
        public ProductPageViewModelBuilder(UnitOfWork<FlockDataContext> uow)
        {
            UoW = uow;
        }

        public ProductPageViewModel Build(long? id)
        {
            throw new NotImplementedException();
        }

        public ProductPageViewModel Build(long ProductID, short MarketID)
        {
            ProductPageViewModel ProductModel = new ProductPageViewModel();

            ProductModel.MainProduct = UoW.GetRepository<Product>().GetAll().Where(p => p.ProductID == ProductID).Single();

            ProductModel.AssociatedProducts = new List<Product>();

            // Build AssociatedProducts.

            ProductModel.SlideShowImages = new List<Image>();

            // Fetch assigned SlideShow images.

            return ProductModel;
        }
    }

    public class ProductPageViewModel
    {
        public Product MainProduct;
        public List<Product> AssociatedProducts;
        public List<Image> SlideShowImages;
    }
}