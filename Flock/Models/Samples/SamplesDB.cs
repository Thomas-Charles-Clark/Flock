using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Flock.Models
{
    public class TradeAccountPrimaryAddress
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public short? CountryID { get; set; }
    }

    public class TradeAccountSecondaryAddress
    {
        public string DeliveryReference { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public short? CountryID { get; set; }
    }

    public partial class FlockDataContext
    {
        [Function(Name = "adm.TradeAccountNumber_FetchAddresses")]
        [ResultType(typeof(TradeAccountPrimaryAddress))]
        [ResultType(typeof(TradeAccountSecondaryAddress))]
        public IMultipleResults TradeAccountNumber_FetchAddresses(String tradeAccountNumber)
        {
            IExecuteResult result = this.ExecuteMethodCall(
                this, (MethodInfo)MethodInfo.GetCurrentMethod(),
                tradeAccountNumber);

            return (IMultipleResults)result.ReturnValue;
        }
    }
}