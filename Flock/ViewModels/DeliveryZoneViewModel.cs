using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;
using System.ComponentModel.DataAnnotations;

namespace Flock.ViewModels
{
    public class DeliveryZoneViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public DeliveryZoneViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public DeliveryZoneViewModel Build(short? DeliveryZoneID)
        {
            var Model = new DeliveryZoneViewModel();
            var d = UoW.GetRepository<DeliveryZone>().GetAll().Where(p => p.DeliveryZoneID == DeliveryZoneID).Single();

            Model.DeliveryZoneID = d.DeliveryZoneID;
            Model.Name = d.Name;
            Model.CountryID = d.CountryID;
            Model.CourierID = d.CourierID;
            Model.DeliveryZone_Postcodes = UoW.GetRepository<DeliveryZone_Postcode>().GetAll().Where(p => p.DeliveryZoneID == DeliveryZoneID).ToList();

            return Model;
        }
    }

    public class DeliveryZoneViewModel : BaseViewModel
    {
        public short? DeliveryZoneID { get; set; }

        [StringLength(50, MinimumLength=3)]
        public string Name { get; set; }

        public short CountryID { get; set; }

        public byte CourierID { get; set; }

        public List<DeliveryZone_Postcode> DeliveryZone_Postcodes { get; set; }
    }
}