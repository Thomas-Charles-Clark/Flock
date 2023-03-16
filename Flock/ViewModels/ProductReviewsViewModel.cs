using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductReviewsViewModelBuilder : ViewModelBuilder, IViewModelBuilder<ProductReviewsViewModel>
    {
        public ProductReviewsViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public ProductReviewsViewModel Build(long? id)
        {
            ProductReviewsViewModel PRVM = new ProductReviewsViewModel();
            PRVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();

            return PRVM;
        }
    }

    public class ProductReviewsViewModel : BaseViewModel
    {
    }

    public class ReviewGridSettings
    {
        public byte? MarketID { get; set; }
    }
}