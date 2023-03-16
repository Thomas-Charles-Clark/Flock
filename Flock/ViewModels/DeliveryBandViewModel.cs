using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;
using System.ComponentModel.DataAnnotations;

namespace Flock.ViewModels
{
    public class DeliveryBandViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public DeliveryBandViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public DeliveryBandViewModel Build(short? DeliveryBandID)
        {
            var Model = new DeliveryBandViewModel();
            var d = UoW.GetRepository<DeliveryBand>().GetAll().Where(p => p.DeliveryBandID == DeliveryBandID).Single();

            Model.DeliveryBandID = d.DeliveryBandID;
            Model.CourierID = d.CourierID;
            Model.PriceListID = d.PriceListID;
            Model.Name = d.Name;
            Model.DisplayName = d.DisplayName;
            Model.CARRCODE = d.CARRCODE;
            Model.SortOrder = d.SortOrder;
            Model.CreatedDT = d.CreatedDT;
            Model.DeletedDT = d.DeletedDT; 
            Model.CutOff = Convert.ToDateTime(DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString()) + d.CutOff;

            var DZLinker = UoW.GetRepository<DeliveryBand_DeliveryZone>().GetAll().Where(p => p.DeliveryBandID == DeliveryBandID).Select(p => p.DeliveryZoneID).ToList();
            Model.AssignedDeliveryZones = UoW.GetRepository<DeliveryZone>().GetAll().Where(p => DZLinker.Contains(p.DeliveryZoneID)).ToList();

            var SLLinker = UoW.GetRepository<DeliveryBand_StockLocation>().GetAll().Where(p => p.DeliveryBandID == DeliveryBandID).Select(p => p.StockLocationID).ToList();
            Model.AssignedStockLocations = UoW.GetRepository<StockLocation>().GetAll().Where(p => SLLinker.Contains(p.StockLocationID)).ToList();

            Model.AllowedDeliveryZones = new List<DeliveryZone>();

            var ADZLinker = new List<short>();
                   
            var CountryLinker = UoW.GetRepository<Country>().GetAll().Where(p => p.PriceListID == Model.PriceListID).Select(p => p.CountryID).ToList();
            var CourierLinker = UoW.GetRepository<Courier>().GetAll().Where(p => p.CourierID == Model.CourierID).Select(p => p.CourierID).ToList();

            Model.AllowedDeliveryZones = UoW.GetRepository<DeliveryZone>().GetAll().Where(p => CountryLinker.Contains(p.CountryID) && CourierLinker.Contains(p.CourierID)).ToList();

            Model.AllowedStockLocations = UoW.GetRepository<StockLocation>().GetAll().OrderBy(p => p.Name).ToList();

            Model.Prices = UoW.GetRepository<DeliveryPrice>().GetAll().Where(p => p.DeliveryBandID == DeliveryBandID).ToList();

            Model.DeliveryOptionID = d.DeliveryOptionID;
            Model.WeekdayStart = d.WeekdayStart;
            Model.WeekdayEnd = d.WeekdayEnd;
            Model.DaysToDeliverMin = d.DaysToDeliverMin;
            Model.DaysToDeliverMax = d.DaysToDeliverMax;

            return Model;
        }
    }

    public class DeliveryBandViewModel : BaseViewModel
    {
        public short? DeliveryBandID { get; set; }

        public byte DeliveryOptionID { get; set; }

        [Required]
        public byte CourierID { get; set; }

        [Required]
        public byte PriceListID { get; set; }

        [Required]
        [StringLength(50, MinimumLength=3)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength=3)]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(10)]
        public string CARRCODE { get; set; }

        [Required]
        public byte SortOrder { get; set; }

        public DateTime? CutOff { get; set; }

        public DateTime CreatedDT { get; set; }

        public DateTime? DeletedDT { get; set; }

        public List<DeliveryZone> AssignedDeliveryZones { get; set; }

        public List<StockLocation> AssignedStockLocations { get; set; }

        public List<DeliveryZone> AllowedDeliveryZones { get; set; }

        public List<StockLocation> AllowedStockLocations { get; set; }

        public List<DeliveryPrice> Prices { get; set; }

        public byte WeekdayStart { get; set; }

        public byte WeekdayEnd { get; set; }

        public byte DaysToDeliverMin { get; set; }

        public byte DaysToDeliverMax { get; set; }
    }
}