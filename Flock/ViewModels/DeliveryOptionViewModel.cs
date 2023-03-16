using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;
using System.ComponentModel.DataAnnotations;

namespace Flock.ViewModels
{
    public class DeliveryOptionViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public DeliveryOptionViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public DeliveryOptionViewModel Build(short? DeliveryID)
        {
            var Model = new DeliveryOptionViewModel();
            var d = UoW.GetRepository<Delivery>().GetAll().Where(p => p.DeliveryID == DeliveryID).Single();

            Model.DeliveryID = d.DeliveryID;
            Model.DeliveryServiceID = d.DeliveryServiceID;
            Model.Name = d.Name;
            Model.HaulCode = d.HAULCODE;
            Model.CarrCode = d.CARRCODE;
            Model.PostCodeRestrictions = d.PostCodeRestrictions;
            Model.MinThreshold = d.MinThreshold;
            Model.MaxThreshold = d.MaxThreshold;
            Model.PricePerItem = d.PricePerItem;
            Model.PricePerDispatch = d.PricePerDispatch;
            Model.OnDate = d.OnDate;
            Model.OffDate = d.OffDate;
            Model.CreatedDT = d.CreatedDT;
            Model.DeletedDT = d.DeletedDT;

            var Linker = UoW.GetRepository<Delivery_ProductType>().GetAll().Where(p => p.DeliveryID == DeliveryID).Select(p => p.ProductTypeID).ToList();
            Model.AssignedProductTypes = UoW.GetRepository<ProductType>().GetAll().Where(p => Linker.Contains(p.ProductTypeID)).ToList();

            return Model;
        }
    }

    public class DeliveryOptionViewModel : BaseViewModel
    {
        public short? DeliveryID { get; set; }

        //public byte? DeliveryTypeID { get; set; }
        public byte? DeliveryServiceID { get; set; }

        [StringLength(50, MinimumLength=3)]
        public string Name { get; set; }

        [StringLength(10)]
        public string HaulCode { get; set; }

        [StringLength(10)]
        public string CarrCode { get; set; }

        public string PostCodeRestrictions { get; set; }

        public decimal? MinThreshold { get; set; }

        public decimal? MaxThreshold { get; set; }

        public decimal? PricePerItem { get; set; }

        public decimal? PricePerDispatch { get; set; }

        public DateTime? OnDate { get; set; }

        public DateTime? OffDate { get; set; }

        public DateTime CreatedDT { get; set; }

        public DateTime? DeletedDT { get; set; }

        public List<ProductType> AssignedProductTypes { get; set; }
    }
}