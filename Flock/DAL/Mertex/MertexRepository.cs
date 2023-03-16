using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using EnterpriseLink.RFQUERY.NET;
using NLog;

namespace Flock.DAL.Mertex
{
    public class MertexConnection
    {
        private RFUOdotNET.RF_UniObjectsClass Query;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public MertexConnection(String Name, String Pwd, String UniqueID, String SvrIP)
        {
            Query = new RFUOdotNET.RF_UniObjectsClass();
            Query.WebUserName = Name;
            Query.WebUserPwd = Pwd;
            Query.WebUniqueID = UniqueID;

            Query.UVHostIP = SvrIP;
            //Query.DebugMsg = true;
        }

        public String CustomerRef
        {
            get
            {
                return Query.CustomerRefs;
            }
        }
        
        public String ID
        {
            get
            {
                return Query.WebUniqueID;
            }
            set
            {
                Query.WebUniqueID = value;
            }
        }

        public String VerifyLogin()
        {
            return Query.VerifyLogin("", Query.WebUserName, Query.WebUserPwd);
        }

        public XmlDocument Call(String FileName, String SelectStatement, String FieldsToReturn, String WebData)
        {
            XmlDocument xdd = new XmlDocument();
            Query.UVFileName = FileName;
            Query.UVSelectStatement = SelectStatement;
            Query.UVFieldsToReturn = FieldsToReturn;
            Query.WebData = WebData;

            Query.PageNumber = 1;
            Query.PageSize = 0;

            Query.RunQuery();

            String strResult = Query.Result;

            return ResultWrapper(strResult, FileName);
        }

        public XmlDocument CallNoXML(String FileName, String SelectStatement, String FieldsToReturn, String WebData)
        {
            XmlDocument xdd = new XmlDocument();
            Query.UVFileName = FileName;
            Query.UVSelectStatement = SelectStatement;
            Query.UVFieldsToReturn = FieldsToReturn;
            Query.WebData = WebData;

            Query.PageNumber = 1;
            Query.PageSize = 0;

            Query.RunQuery();

            String strResult;
            strResult = Query.Result;

            return ResultWrapper("<message>" + strResult + "</message>", FileName);
        }

        private XmlDocument ResultWrapper(String sRes, String FileName)
        {
            XmlDocument xdd = new XmlDocument();
            xdd.LoadXml("<result></result>");

            try
            {
                if (sRes.Length == 0)
                {
                    XmlElement xnd = xdd.CreateElement("data");
                    xdd.DocumentElement.AppendChild(xnd);
                }
                else
                {
                    XmlDocument xddRes = new XmlDocument();
                    xddRes.LoadXml(sRes);

                    XmlElement xnd = xdd.CreateElement("data");
                    xnd.AppendChild(xdd.ImportNode(xddRes.DocumentElement, true));
                    xdd.DocumentElement.AppendChild(xnd);
                }

                XmlElement xn = xdd.CreateElement("success");
                xn.InnerText = "yes";
                xdd.DocumentElement.AppendChild(xn);
            }
            catch (Exception e)
            {
                XmlElement xn = xdd.CreateElement("success");
                xn.InnerText = "no";
                xdd.DocumentElement.AppendChild(xn);

                XmlElement xnd = xdd.CreateElement("data");
                xnd.InnerText = e.Message + "\n\n" + e.StackTrace + "\n\n" + sRes;
                Logger.Error(e, "MertexRepository.cs - ResultWrapper threw an Exception: " + "\r\n" + "sRes: " + sRes + " ~" + " FileName: " + FileName);
                xdd.DocumentElement.AppendChild(xnd);
            }

            return xdd;
        }

        public String CallRaw(String FileName, String[] SelectStatement, String FieldsToReturn, String WebData)
        {
            XmlDocument xdd = new XmlDocument();
            Query.UVFileName = FileName;
            Query.UVFieldsToReturn = FieldsToReturn;
            Query.WebData = WebData;

            // Default Params across all statements
            Query.PageNumber = 1;
            Query.PageSize = 0;


            String strResult = "";
            foreach (String ss in SelectStatement)
            {
                Query.UVSelectStatement = ss;
                Query.RunQuery();
                strResult += Query.Result;
            }

            return strResult;
        }
    }

    public class MertexOrders
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        public MertexConnection MertexConnection;

        public MertexOrders(MertexConnection MertexConnection)
        {
            this.MertexConnection = MertexConnection;
        }

        public bool CreateOrder(String BasketID, Order order, XmlDocument failedReturn)
        {
            XmlDocument eResultXML = new XmlDocument();

            Logger.Trace("Creating Basket");
            // To-do: Create the order in Mertex

            eResultXML = CreateBasket(BasketID);

            if (eResultXML.SelectSingleNode("//success").InnerText != "yes")
            {
                failedReturn.LoadXml(eResultXML.OuterXml);
                return false;
            }

            Logger.Trace("Begin Adding OrderLines");
            eResultXML = AddLineItems(order.OrderLines);

            if (eResultXML.SelectSingleNode("//success").InnerText != "yes")
            {
                Logger.Error("Failed Adding OrderLines");
                failedReturn.LoadXml(eResultXML.OuterXml);
                return false;
            }

            Logger.Trace("Begin Adding Header");
            eResultXML = AddHeader(order);
            if (eResultXML.SelectSingleNode("//success").InnerText != "yes")
            {
                Logger.Error("Failed Adding Header");
                failedReturn.LoadXml(eResultXML.OuterXml);
                return false;
            }

            return true;
        }

        public XmlDocument CreateBasket(String BasketID)
        {
            XmlDocument eResultXML = MertexConnection.Call("B2C_GET_BASKET", BasketID, "", "");
            return eResultXML;
        }

        public XmlDocument AddLineItems(OrderLine[] OrderLines)
        {
            String errors = "";
            XmlDocument xdd = new XmlDocument();
            xdd.LoadXml("<result></result>");

            foreach (OrderLine ol in OrderLines)
            {
                MertexStock stock = new MertexStock(MertexConnection);
                XmlDocument stockXML = stock.StockStatus(ol.ProductCode);
                Logger.Trace("MertexRepository.cs - AddLineItems: stockXML.OuterXml was: \r\n" + stockXML.OuterXml);
                XmlNode xn = stockXML.SelectSingleNode("//stock[@stockcode='" + ol.ProductCode + "' and string-length(@qty) > 0 ]");
                if (xn != null)
                {
                    Logger.Trace("Stock Level: " + xn.Attributes["qty"].Value);

                    double iStockCount = Double.Parse(xn.Attributes["qty"].Value);
                    double iQtyReq = Double.Parse(ol.OrderQuantity);

                    if (iStockCount < iQtyReq)
                    {
                        ol.BackOrderIndicator = "Y";
                    }
                    else
                    {
                        ol.BackOrderIndicator = "";
                    }
                }

                XmlDocument eResultXML = new XmlDocument();
                String WebData = "";
                WebData += MertexConnection.CustomerRef + "\r\n";   //ol.CustomerAccountCode + "\r\n";
                WebData += ol.OrderLineNumber + "\r\n";
                WebData += ol.ProductCode + "\r\n";
                WebData += ol.OrderQuantity + "\r\n";
                // Following section fields removed since they are to be left blank by specification.
                WebData += "\r\n"; //ol.SampleIndicator + 
                WebData += ol.BackOrderIndicator + "\r\n"; //ol.BackOrderIndicator +
                WebData += "\r\n"; // ol.ReserveIndicator +
                WebData += "\r\n"; // ol.DiscountPercentage +
                WebData += "\r\n"; // ol.NumberOf + 
                WebData += "\r\n"; // ol.Quantity + 
                WebData += ol.LineNotes + "\r\n";
                WebData += ol.OrderUnit + "\r\n";
                WebData += ol.CustomersOwnReference + "\r\n";
                WebData += "\r\n"; // ol.AutoUpdateKey
                WebData += "\r\n"; //ol.WithCuttingIndicator
                WebData += "\r\n"; // ol.SpecificBatchInd
                WebData += ol.OrderLinePrice + "\r\n";
                WebData += ol.OrderLineValue + "\r\n";
                WebData += ol.OrderLineVATCode + "\r\n";
                WebData += ol.OrderLineVATValue + "\r\n";
                WebData += "\r\n";
                WebData += ol.SundryCode + "\r\n";

                Logger.Trace("Adding:\r\n\"" + WebData + "\"");

                eResultXML = MertexConnection.Call("B2C_SALES_ORDER_ALLOC", "", "", WebData);
                if (eResultXML.SelectSingleNode("//success").InnerText != "yes")
                {
                    if (ol.BackOrderIndicator == "Y")
                    {
                        errors += eResultXML.SelectSingleNode("//data").InnerText + "\n\n";
                    }
                    else
                    {
                        // Barrys fall back code - set back order to yes
                        WebData = "";
                        WebData += MertexConnection.CustomerRef + "\r\n";   //ol.CustomerAccountCode + "\r\n";
                        WebData += ol.OrderLineNumber + "\r\n";
                        WebData += ol.ProductCode + "\r\n";
                        WebData += ol.OrderQuantity + "\r\n";
                        // Following section fields removed since they are to be left blank by specification.
                        WebData += "\r\n"; //ol.SampleIndicator + 
                        WebData += "Y\r\n"; //ol.BackOrderIndicator +
                        WebData += "\r\n"; // ol.ReserveIndicator +
                        WebData += "\r\n"; // ol.DiscountPercentage +
                        WebData += "\r\n"; // ol.NumberOf + 
                        WebData += "\r\n"; // ol.Quantity + 
                        WebData += ol.LineNotes + "\r\n";
                        WebData += ol.OrderUnit + "\r\n";
                        WebData += ol.CustomersOwnReference + "\r\n";
                        WebData += "\r\n"; // ol.AutoUpdateKey
                        WebData += "\r\n"; //ol.WithCuttingIndicator
                        WebData += "\r\n"; // ol.SpecificBatchInd
                        WebData += ol.OrderLinePrice + "\r\n";
                        WebData += ol.OrderLineValue + "\r\n";
                        WebData += ol.OrderLineVATCode + "\r\n";
                        WebData += ol.OrderLineVATValue + "\r\n";
                        WebData += "\r\n";
                        WebData += ol.SundryCode + "\r\n";
                        eResultXML = MertexConnection.Call("B2C_SALES_ORDER_ALLOC", "", "", WebData);
                        if (eResultXML.SelectSingleNode("//success").InnerText != "yes")
                        {
                            errors += eResultXML.SelectSingleNode("//data").InnerText + "\n\n";
                            Logger.Error("MertexRepository.cs - B2C_SALES_ORDER_ALLOC Error: " + eResultXML.SelectSingleNode("//data").InnerText);
                        }
                    }
                }
            }

            if (errors.Length > 0)
            {
                XmlElement xn = xdd.CreateElement("success");
                xn.InnerText = "no";
                xdd.DocumentElement.AppendChild(xn);

                XmlElement xnd = xdd.CreateElement("data");
                xnd.InnerText = errors;
                xdd.DocumentElement.AppendChild(xnd);
            }
            else
            {
                XmlElement xn = xdd.CreateElement("success");
                xn.InnerText = "yes";
                xdd.DocumentElement.AppendChild(xn);

                XmlElement xnd = xdd.CreateElement("data");
                xnd.InnerText = "";
                xdd.DocumentElement.AppendChild(xnd);
            }

            return xdd;
        }

        private XmlDocument AddHeader(Order order)
        {
            String WebData = "";
            WebData += MertexConnection.CustomerRef + "\r\n"; // order.CustomerAccountCode + "\r\n";
            WebData += order.DeliveryReference + "\r\n";
            WebData += order.DeliveryAddress.Name + "|";
            WebData += order.DeliveryAddress.Address1 + "|";
            WebData += order.DeliveryAddress.Address2 + "|";
            WebData += order.DeliveryAddress.Address3 + "|";
            WebData += order.DeliveryAddress.Address4 + "|";
            WebData += order.DeliveryAddress.Postcode.ToUpper() + "|";
            WebData += order.DeliveryAddress.TeleNo + "|";
            WebData += order.DeliveryAddress.DomesticTradeIndicator + "|";
            WebData += order.DeliveryAddress.CountryCode + "|\r\n";
            WebData += order.Email + "\r\n";
            WebData += order.CustomerOrderReference + "\r\n";
            WebData += order.HaulierCode.Trim() + "\r\n";
            WebData += order.PriorityOrderIndicator + "\r\n";
            WebData += order.GiftWrapIndicator + "\r\n";
            WebData += order.PartDeliveryRequested + "\r\n";
            WebData += order.OrderValue.Trim() + "\r\n";
            WebData += order.CarriageServiceCode.Trim() + "\r\n";
            WebData += order.CarriageVATCode.Trim() + "\r\n";
            WebData += order.CarriageValue.Trim() + "\r\n";
            WebData += order.CarriageVATValue.Trim() + "\r\n";
            WebData += order.OrderNo;

            Logger.Trace("Adding Header:\r\n\"" + WebData + "\"");

            return MertexConnection.Call("B2C_SALES_ORDER_DELDETS", "", "", WebData);
        }

        public XmlDocument ConfirmOrder()
        {
            //  String WebData = ec.CustomerRef + "\r\n";
            //  return ec.CallNoXML("B2C_SALES_ORDER_CREATE", "", "", WebData);

            String WebData = MertexConnection.CustomerRef + "\r\n";
            XmlDocument xdd = MertexConnection.CallNoXML("B2C_SALES_ORDER_CREATE", "", "", WebData);
            Logger.Trace("Confirm Order: " + xdd.InnerText);
            return xdd;
        }

        public XmlDocument BasketDetails(String BasketID)
        {
            return MertexConnection.Call("B2C_GET_BASKET", BasketID, "", "");
        }

        public XmlDocument AllocateBasket(String BasketID)
        {
            return MertexConnection.CallNoXML("B2C_GET_BASKET", BasketID, "", "");
        }

        public XmlDocument DeleteBasket()
        {
            //looks like this wasn't ever done... which is fine but slightly unclear.
            //calling clear_basket with no parameters?! maddening
            Logger.Trace("MertexRepository.cs - DeleteBasket", "Clearing basket");
            return MertexConnection.Call("CLEAR_BASKET", "", "", "");
        }

        public XmlDocument OrderStatus(String OrderID)
        {
            // Create a new response object which we will populate
            XmlDocument oXmlResponse = new XmlDocument();
            oXmlResponse.AppendChild(oXmlResponse.CreateElement("OrderStatus"));

            // Get the order status from mertex
            XmlDocument eResultXML = MertexConnection.Call("LIST_ORDER_LINES", "ORDERNO=\"" + OrderID + "\"", "LINE ORDNO DATEONFILE QTYORD.ORD WI.STDPROD TEXT.DESC CUS.ORD.REF WI.STATUS WI.PARCEL.LINK", "");
            Logger.Trace("MertexRepository.cs - OrderStatus - OrderID: " + OrderID.ToString(), eResultXML.InnerXml);
            // Iterate over OrderLines
            foreach (XmlNode oXmlRecord in eResultXML.DocumentElement.SelectNodes("data/results/Detail/record"))
            {
                XmlElement oXmlOrderLine = oXmlResponse.CreateElement("OrderLine");

                oXmlOrderLine.SetAttribute("Code", oXmlRecord.SelectSingleNode("fd[@id='C5']/@value").InnerText);
                oXmlOrderLine.SetAttribute("Status", oXmlRecord.SelectSingleNode("fd[@id='C8']/@value").InnerText);

                // Grab the shipping query field
                XmlNode oXmlTrackingQuery = oXmlRecord.SelectSingleNode("fd[@id='C9']/@value");

                if (oXmlTrackingQuery != null)
                {
                    // Make sure a shipping query is available
                    string strTrackingQuery = oXmlTrackingQuery.InnerText;

                    if (strTrackingQuery.Length > 0)
                    {
                        // Split the query into call + value
                        string[] arrTrackingQuery = strTrackingQuery.Split(new char[] { ',' });

                        // Run the shipping query against mertex to get detailed information
                        XmlDocument oXmlTrackingResult = MertexConnection.Call(arrTrackingQuery[0], "@ID=\"" + arrTrackingQuery[1] + "\"", "", "");

                        // Get the tracking code for this line item
                        XmlNode oXmlTracking = oXmlTrackingResult.DocumentElement.SelectSingleNode("data/results/Detail/record/fd[@id='C4']/@value");
                        if (oXmlTracking != null)
                        {
                            oXmlOrderLine.SetAttribute("TrackingURL", oXmlTracking.InnerText);
                        }
                    }
                }

                oXmlResponse.DocumentElement.AppendChild(oXmlOrderLine);
            }

            return oXmlResponse;
        }

        public XmlDocument OrderStatuses(String[] OrderIDs)
        {
            XmlDocument xdd = new XmlDocument();
            xdd.LoadXml("<orders></orders>");
            foreach (String OrderID in OrderIDs)
            {
                if (OrderID.Length > 0)
                {
                    XmlDocument eResultXML = MertexConnection.Call("LIST_ORDER_LINES", "ORDERNO=\"" + OrderID + "\"", "LINE ORDNO DATEONFILE QTYORD.ORD WI.STDPROD TEXT.DESC CUS.ORD.REF WI.STATUS WI.PARCEL.LINK", "");

                    if (eResultXML.SelectSingleNode("//success").InnerText == "yes")
                    {
                        XmlElement xe = xdd.CreateElement("order");

                        xe.SetAttribute("orderid", OrderID);

                        xdd.DocumentElement.AppendChild(xe);

                        foreach (XmlNode xn in eResultXML.DocumentElement.SelectNodes("data/results/Detail/record"))
                        {
                            XmlElement xol = xdd.CreateElement("OrderLine");
                            xol.AppendChild(xdd.ImportNode(xn, true));
                            xe.AppendChild(xol);

                            XmlNode oXmlCode = xn.SelectSingleNode("fd[@id='C5']/@value");
                            XmlNode oXmlStatus = xn.SelectSingleNode("fd[@id='C8']/@value");
                            XmlNode oXmlTrackingQuery = xn.SelectSingleNode("fd[@id='C9']/@value");

                            if (oXmlCode != null) { xol.SetAttribute("Code", oXmlCode.InnerText); }
                            if (oXmlStatus != null) { xol.SetAttribute("Status", oXmlStatus.InnerText); }

                            if (oXmlTrackingQuery != null)
                            {
                                // Make sure a shipping query is available
                                string strTrackingQuery = oXmlTrackingQuery.InnerText;

                                if (strTrackingQuery.Length > 0)
                                {
                                    // Split the query into call + value
                                    string[] arrTrackingQuery = strTrackingQuery.Split(new char[] { ',' });

                                    // Run the shipping query against mertex to get detailed information
                                    XmlDocument oXmlTrackingResult = MertexConnection.Call(arrTrackingQuery[0], "@ID=\"" + arrTrackingQuery[1] + "\"", "", "");

                                    // Get the tracking code for this line item
                                    XmlNode oXmlTracking = oXmlTrackingResult.DocumentElement.SelectSingleNode("data/results/Detail/record/fd[@id='C4']/@value");
                                    if (oXmlTracking != null)
                                    {
                                        xol.SetAttribute("TrackingURL", oXmlTracking.InnerText);
                                    }
                                }
                            }
                        }
                        xe.SetAttribute("success", "yes");
                    }
                    else
                    {
                        xdd = new XmlDocument();
                        xdd.LoadXml(eResultXML.OuterXml);

                        XmlElement xe = xdd.CreateElement("order");
                        xe.SetAttribute("orderid", OrderID);
                        xe.SetAttribute("success", "no");
                        xdd.DocumentElement.AppendChild(xe);
                    }
                }
            }
            return xdd;
        }

        public XmlDocument ListCountries()
        {
            return MertexConnection.Call("LIST_COUNTRY", "", "", "");
        }

        public XmlDocument ListVATCodes()
        {
            return MertexConnection.Call("B2C_LIST_VAT_CODE", "", "", "");
        }
    }

    public class MertexStock
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        public MertexConnection MertexConnection;

        public MertexStock(MertexConnection MertexConnection)
        {
            this.MertexConnection = MertexConnection;
        }

        public XmlDocument StockStatus(String[] StockCodes)
        {
            XmlDocument xdd = new XmlDocument();
            xdd.LoadXml("<stockcodes></stockcodes>");
            foreach (String StockCode in StockCodes)
            {
                if (StockCode.Length > 0)
                {
                    XmlDocument eResultXML = MertexConnection.Call("B2C_STOCK_ENQUIRY", StockCode, "PRODCODE PRODDESC WI.TOTFREE WI.DUEIN.SEL WI.DUEIN.OSQTY", "");

                    if (eResultXML.SelectSingleNode("//success").InnerText == "yes")
                    {
                        XmlElement xe = xdd.CreateElement("stock");
                        xe.SetAttribute("stockcode", StockCode);

                        if (eResultXML.SelectNodes("//Detail/record/fd").Count > 0)
                        {
                            xe.SetAttribute("qty", eResultXML.SelectSingleNode("//Detail/record/fd[@id='C3']").Attributes["value"].Value.ToString());
                            xe.SetAttribute("duein", eResultXML.SelectSingleNode("//Detail/record/fd[@id='C4']").Attributes["value"].Value.ToString());
                            xe.SetAttribute("dueinqty", eResultXML.SelectSingleNode("//Detail/record/fd[@id='C5']").Attributes["value"].Value.ToString());
                        }
                        xdd.DocumentElement.AppendChild(xe);

                    }
                    else
                    {
                        xdd = new XmlDocument();
                        xdd.LoadXml(eResultXML.OuterXml);
                    }
                }
            }
            return xdd;
        }

        public XmlDocument StockStatus(String StockCode)
        {
            XmlDocument xdd = new XmlDocument();
            xdd.LoadXml("<stockcodes></stockcodes>");
            XmlDocument eResultXML = MertexConnection.Call("B2C_STOCK_ENQUIRY", StockCode, "PRODCODE PRODDESC WI.TOTFREE WI.DUEIN.SEL WI.DUEIN.OSQTY", "");
            if (eResultXML.SelectSingleNode("//success").InnerText == "yes")
            {

                Logger.Debug("EC", "Single Stock Lookup XML IS: " + eResultXML.OuterXml);
                XmlElement xe = xdd.CreateElement("stock");
                xe.SetAttribute("stockcode", StockCode);

                if (eResultXML.SelectNodes("//Detail/record/fd").Count > 0)
                {
                    xe.SetAttribute("qty", eResultXML.SelectSingleNode("//Detail/record/fd[@id='C3']").Attributes["value"].Value.ToString());
                    xe.SetAttribute("duein", eResultXML.SelectSingleNode("//Detail/record/fd[@id='C4']").Attributes["value"].Value.ToString());
                    xe.SetAttribute("dueinqty", eResultXML.SelectSingleNode("//Detail/record/fd[@id='C5']").Attributes["value"].Value.ToString());
                }
                xdd.DocumentElement.AppendChild(xe);
            }
            else
            {
                xdd = new XmlDocument();
                xdd.LoadXml(eResultXML.OuterXml);
            }
            return xdd;
        }

        public String StockRaw(String codes)
        {
            String sRet = "";
            String vl = MertexConnection.VerifyLogin();
            if (vl.Length == 0)
            {
                String[] sc;
                int i = 1;
                sc = new String[i];
                for (int j = 0; j < i; j++)
                {
                    sc[j] = "G/DG31184";
                }
                sRet += MertexConnection.CallRaw("B2C_STOCK_ENQUIRY", sc, "PRODCODE PRODDESC WI.TOTFREE WIDUEIN.SEL WI.DUEIN.OSQTY", "");
            }
            return sRet;
        }
    }
}