using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class ProductOverviewViewModelBuilder : ViewModelBuilder, IViewModelBuilder<ProductOverviewViewModel>
    {
        public ProductOverviewViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public ProductOverviewViewModel Build(long? id)
        {
            ProductOverviewViewModel POVM = new ProductOverviewViewModel();
            //POVM.Markets = UoW.GetRepository<Market>().GetAll().ToList();
            //POVM.Collections = UoW.GetRepository<Collection>().GetAll().ToList();
            //POVM.Colours = UoW.GetRepository<Colour>().GetAll().ToList();
            POVM.PriceLists = UoW.GetRepository<PriceList>().GetAll().ToList();
            return POVM;
        }
    }

    public class Coll
    {
        public long ID;
        public string Name;
    }

    public class ProductOverviewViewModel : BaseViewModel
    {
        public List<PriceList> PriceLists { get; set; }
    }

    public class ProductOverviewSettings
    {
        public short[] CollectionIDs { get; set; }
        public byte[] PatternIDs { get; set; }
        public byte[] ColourIDs { get; set; }
        public byte[] MarketIDs { get; set; }
    }
}