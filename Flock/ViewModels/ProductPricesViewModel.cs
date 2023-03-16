using Flock.DAL;
using Flock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.ViewModels
{
    public class ProductPricesViewModelBuilder : ViewModelBuilder, IViewModelBuilder<ProductPricesViewModel>
    {
        public ProductPricesViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public ProductPricesViewModel Build(long? id)
        {
            ProductPricesViewModel POVM = new ProductPricesViewModel();
            POVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();
            POVM.Collections = UoW.GetRepository<Collection>().GetAll().ToList();
            POVM.Colours = UoW.GetRepository<Colour>().GetAll().ToList();
            POVM.PriceLists = UoW.GetRepository<PriceList>().GetAll().ToList();
            PriceList PL = new PriceList();
            PL.Name = "(Unassigned)";
            PL.PriceListID = 0;
            POVM.PriceLists.Add(PL);
            return POVM;
        }
    }

    public class ProductPricesViewModel : BaseViewModel
    {
        public List<PriceList> PriceLists;
    }

    public class PriceOverviewSettings
    {
        public byte? PriceListID { get; set; }
        public short[] CollectionIDs { get; set; }
        public byte[] ColourIDs { get; set; }
    }
}
