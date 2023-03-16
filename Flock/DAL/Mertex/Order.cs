using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Flock.DAL.Mertex
{
    public class Order
    {
        /// <summary>
        /// Cancel with Sarah immediate as poss.
        /// 
        /// Ring Freydis -7724 to cancel if Sarah can't
        /// or Marie on -7705  
        /// </summary>

        public String CustomerAccountCode; // C0996
        public String DeliveryReference; // E
        public Address DeliveryAddress; // 123, One Way St
        public String Email; // swaller@designersguild.com  
        public String CustomerOrderReference; // eComm-12/05/2014
        public String HaulierCode; // DP
        public String PriorityOrderIndicator; // 
        public String GiftWrapIndicator; // 
        public String PartDeliveryRequested; // Y
        public String OrderValue; // 100
        public String CarriageServiceCode; // DP1
        public String CarriageVATCode; // S
        public String CarriageValue; // 5
        public String CarriageVATValue; // 1 
        public String OrderNo; // 0-5832853
        public OrderLine[] OrderLines; 
    }
}
