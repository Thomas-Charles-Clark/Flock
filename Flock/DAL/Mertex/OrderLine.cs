using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Flock.DAL.Mertex
{
    public class OrderLine
    {
        public String CustomerAccountCode; // C0996
        public String OrderLineNumber; // 1... n
        public String ProductCode; // CCDG9999 - Try fake code first
        public String OrderQuantity; // 10
        public String SampleIndicator; // N
        public String BackOrderIndicator; // 
        public String ReserveIndicator; // 
        public String DiscountPercentage; // 0
        public String NumberOf; // 
        public String Quantity; //
        public String LineNotes; // 
        public String OrderUnit; // 
        public String CustomersOwnReference; // 
        public String AutoUpdateKey; // 
        public String WithCuttingIndicator; // 
        public String SpecificBatchInd; //
        public String OrderLinePrice; // 10
        public String OrderLineValue; // 100
        public String OrderLineVATCode; // S
        public String OrderLineVATValue; // 20
        public String SundryCode; // 
    }
}
