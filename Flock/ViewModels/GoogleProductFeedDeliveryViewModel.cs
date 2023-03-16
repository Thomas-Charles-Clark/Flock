using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;
using Flock.DAL;

namespace Flock.ViewModels
{
    public class GoogleProductFeedDeliveryViewModel : BaseViewModel
    {
        public GoogleProductFeedDeliveryViewModel()
        {
            var db = new FlockDataContext();
            Prices = db.DefaultPrices.ToList();
            PriceLists = db.PriceLists.Select(p => new PriceListNonEntity() { PriceListID = p.PriceListID, Name = p.Name }).ToList();
            ProductTypes = db.ProductTypes.Select(p => new ProductTypeNonEntity() { ProductTypeID = p.ProductTypeID, Name = p.Name }).ToList();
        }

        public List<DefaultPrice> Prices { get; set; }

        public List<PriceListNonEntity> PriceLists { get; set; }

        public new List<ProductTypeNonEntity> ProductTypes { get; set; }
    }

    public class PriceListNonEntity
    {
        public Byte PriceListID { get; set; }
        public String Name { get; set; }
    }

    public class ProductTypeNonEntity
    {
        public byte ProductTypeID { get; set; }
        public string Name { get; set; }
    }

    public class UnsavedDefaultPriceChange
    {
        public Byte PriceListID { get; set; }
        public Byte ProductTypeID { get; set; }
        public Decimal PriceInc { get; set; }

        public Decimal PriceEx { get; set; }
    }

    public class objUpdateDefaultPrice
    {
        public List<UnsavedDefaultPriceChange> UnsavedChanges { get; set; }
    }
}