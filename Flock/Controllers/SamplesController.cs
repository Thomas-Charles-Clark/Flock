//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using Flock.Models;
//using System.Net;
//using Flock.ViewModels;
//using Newtonsoft.Json;
//using System.Xml;
//using System.Data.SqlClient;
//using Mertexer;
//using NLog;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.AspNet.Identity;
//using System.Configuration;

//namespace Flock.Controllers
//{
//    [Authorize(Roles = "Admin,CustomerService")]
//    public class SamplesController : Controller
//    {
//        private static Logger Logger = LogManager.GetCurrentClassLogger();

//        public SamplesController() { }

//        public SamplesController(UserManager userManager)
//        {
//            try
//            {
//                UserManager = userManager;
//            }
//            catch (Exception ex)
//            {
//                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
//            }
//        }

//        private UserManager _userManager;
//        public UserManager UserManager
//        {
//            get
//            {
//                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
//            }
//            private set
//            {
//                _userManager = value;
//            }
//        }


//        // GET: Samples
//        //public ActionResult Index()
//        //{
//        //    var db = new FlockDataContext();
            
//        //    var Model = new SamplesViewModel()
//        //    {
//        //        Countries = db.Countries
//        //                        .Where(p => p.DeletedDT == null)
//        //                        .Select(p => new SampleCountry() { Name = p.Name, Code = p.Code, ThreeDigitCode = p.ThreeDigitCode, CountryID = p.CountryID })
//        //                        .OrderBy(p => p.Name)
//        //                        .ToList(),

//        //        Hauliers = db.TradeHauliers.ToList(),
//        //        Carriages = db.TradeCarriages.ToList(),
//        //        Languages = db.Languages.Where(p => p.DeletedDT == null).Select(p => new SampleLanguage() { LanguageID = p.LanguageID, Name = p.Name }).ToList(),
//        //        DefaultOrderTaker = UserManager.FindById(User.Identity.GetUserId()).DefaultOrderTaker
//        //    };

//        //    return View(Model);
//        //}

//        // POST: Account Number AutoComplete
//        [HttpPost]
//        public ActionResult AccountNumberAutocomplete(PassAutocomplete objAutocomplete)
//        {
//            var results = new FlockDataContext().TradeAccountNumber_Autocomplete(objAutocomplete.term).ToList();

//            return Json(results, JsonRequestBehavior.DenyGet);
//        }

//        [HttpPost]
//        public ActionResult OperatorAutocomplete(PassAutocomplete objAutocomplete)
//        {
//            //|| p.Name.Contains(objAutocomplete.term)
//            var results = new FlockDataContext().Trade_MertexOperators.Where(p => p.Code.StartsWith(objAutocomplete.term)).Distinct().ToList();

//            return Json(results, JsonRequestBehavior.DenyGet);
//        }

//        [HttpPost]
//        public ActionResult ProductCodeAutocomplete(PassProductCodeAutoComplete objAutocomplete)
//        {
//            var results = new FlockDataContext().ProductCode_Autocomplete(objAutocomplete.term, objAutocomplete.tradePriceListId).ToList();
            
//            return Json(results, JsonRequestBehavior.DenyGet);
//        }

//        [HttpPost]
//        public ActionResult FetchAddresses(PassTradeAccountNumber objAccountNumber)
//        {
//            var queryResults = new FlockDataContext().TradeAccountNumber_FetchAddresses(objAccountNumber.accountNumber);

//            var results = new TradeAccountAddressReturn()
//            {
//                PrimaryAddresses = queryResults.GetResult<TradeAccountPrimaryAddress>().ToList(),
//                SecondaryAddresses = queryResults.GetResult<TradeAccountSecondaryAddress>().ToList()
//            };

//            return Json(results, JsonRequestBehavior.DenyGet);
//        }

//        [HttpPost]
//        public ActionResult DispatchAutocomplete(PassAutocomplete objAutocomplete)
//        {
//            var results = new FlockDataContext().TradeCarriage_Autocomplete(objAutocomplete.term).ToList();

//            return Json(results, JsonRequestBehavior.DenyGet);
//        }

//        [HttpPost]
//        public ActionResult PCAFind(PassAddressLookupSearch objSearch)
//        {
//            try
//            {
//                if (objSearch.id == null)
//                {
//                    var client = new PCAFind.PostcodeAnywhere_SoapClient();

//                    var results = client.Capture_Interactive_Find_v1_00(
//                        Key: ConfigurationManager.AppSettings["PCA-API-Flock"],
//                        Text: objSearch.text,
//                        Container: objSearch.container,
//                        Origin: objSearch.origin,
//                        Countries: objSearch.countries,
//                        Limit: 12,
//                        Language: "en-GB"
//                    );

//                    return Json(results, JsonRequestBehavior.DenyGet);
//                }
//                else
//                {
//                    var client = new PCARetrieve.PostcodeAnywhere_SoapClient();

//                    var results = client.Capture_Interactive_Retrieve_v1_00(
//                        Key: ConfigurationManager.AppSettings["PCA-API-Flock"],
//                        Id: objSearch.id,
//                        Field1Format: null,
//                        Field2Format: null,
//                        Field3Format: null,
//                        Field4Format: null,
//                        Field5Format: null,
//                        Field6Format: null,
//                        Field7Format: null,
//                        Field8Format: null,
//                        Field9Format: null,
//                        Field10Format: null,
//                        Field11Format: null,
//                        Field12Format: null,
//                        Field13Format: null,
//                        Field14Format: null,
//                        Field15Format: null,
//                        Field16Format: null,
//                        Field17Format: null,
//                        Field18Format: null,
//                        Field19Format: null,
//                        Field20Format: null
//                    );

//                    return Json(results, JsonRequestBehavior.DenyGet);
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { error = ex.Message, errorEx = ex.ToString() }, JsonRequestBehavior.DenyGet);
//            }
//        }
        
//        /*
//        Mertex Mertex = new Mertex(OrderHead.Rows[0]["MertexUserName"].ToString());

//        Order Order = null;

//        Order = Mertex.OrderController.BuildOrder(
//            OrderHead.Rows[0],
//            OrderLines,
//            GiftVouchers,
//            GiftBoxes,
//            GiftBoxedItems);

//        XmlDocument FailedReturn = new XmlDocument();
//        XmlElement xDocEl = FailedReturn.CreateElement("root");
//        FailedReturn.AppendChild(xDocEl);

//        string basketGUID = Guid.NewGuid().ToString();

//        Mertex.OrderController.PlaceOrder(OrderHead.Rows[0]["MemberID"] + "*" + Order.CustomerAccountCode + "*" + basketGUID, Order, FailedReturn);

//        NLog.LogManager.GetCurrentClassLogger().Warn(Order.ToString());

//        if (FailedReturn.InnerText != "")
//        {
//            throw new Exception("PlaceOrder returned Failure: " + FailedReturn.InnerText);
//        }

//             */
//        [HttpPost]
//        public ActionResult TestMertex(string accountNumber)
//        {
//            var result = new SampleOrderReturn()
//            {
//                state = "failure",
//                message = "general failure"
//            };

//            try
//            {
//                var mertex = new Mertex(accountNumber, true, true);
                
//                result.state = "success";
//                result.message = "";

//                return Json(result, JsonRequestBehavior.DenyGet);
//            }
//            catch (WebTexRecordException)
//            {
//                result.state = "needLogin";
//                result.message = String.Format(@"<p>Please check that a WebTex login for account number {0} exists.</p>
//                    <p>If this WebTex login is of type ""Agent"", you <strong>must</strong> create a second login associated to this account number of type ""Customer"".</p>
//                    <p><strong>You must not alter the type of the existing WebTex login. Follow procedure and create a new login for this account number.</strong></p>
//                    <p>Please ensure that the new WebTex login's Sample Order Entry field is set to 'Y'.</p>", accountNumber);

//                return Json(result, JsonRequestBehavior.DenyGet);
//            }
//        }

//        //[HttpPost]
//        //[ValidateInput(false)]
//        //public ActionResult PlaceOrder(string order)
//        //{
//        //    System.IO.StringReader sReader = new System.IO.StringReader(order);
//        //    JsonReader jReader = new JsonTextReader(sReader);
//        //    SampleOrder objOrder = new JsonSerializer().Deserialize<SampleOrder>(jReader);

//        //    var result = new SampleOrderReturn()
//        //    {
//        //        state = "failure",
//        //        message = "general failure"
//        //    };
            
//        //    try
//        //    {
//        //        var mertex = new Mertex(objOrder.account.AccountNumber, true, true);
//        //        var orderHeadId = new FlockDataContext().TradeSampleOrder_Submit(User.Identity.GetUserId(), objOrder);
//        //        if (orderHeadId == null)
//        //        {
//        //            Response.StatusCode = 500;
//        //            result.state = "failure";
//        //            result.message = "Failed to create transaction";

//        //            return Content(result.message);
//        //        }
//        //        else
//        //        {
//        //            result.state = "success";
//        //            result.message = orderHeadId.Value.ToString();
//        //        }
//        //    }
//        //    catch (WebTexRecordException)
//        //    {
//        //        result.state = "needLogin";
//        //        result.message = String.Format(@"<p>You must create a WebTex login for account number {0}.</p><p>You can keep this page open and place the order immediately after creating the WebTex login.</p>", objOrder.account.AccountNumber);

//        //        return Json(result, JsonRequestBehavior.DenyGet);// Json(new sampleOrderReturn() { result = wtre.Message }, JsonRequestBehavior.DenyGet);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        Response.StatusCode = 500;
//        //        result.state = "failure";
//        //        result.message = e.ToString();

//        //        return Content(e.ToString());
//        //    }

//        //    return Json(result, JsonRequestBehavior.DenyGet);
//        //}
//    }

//    public class SampleCountry
//    {
//        public string Name { get; set; }
//        public string Code { get; set; }
//        public string ThreeDigitCode { get; set; }
//        public short CountryID { get; set; }
//    }

//    public class SampleLanguage
//    {
//        public byte LanguageID { get; set; }
//        public string Name { get; set; }
//    }


//    public class SampleLine
//    {
//        public string Code { get; set; } //Needs trimming of /R , /C
//        public short QuantityTotal { get; set; }
//        public string LineRef { get; set; }
//        public string Status { get; set; }
//        public decimal Price { get; set; }
//        public decimal UnitPrice { get; set; }
//    }

//    public class SampleDeliveryAddress
//    {
//        [Required]
//        public string Name { get; set; }

//        [Required]
//        public string AddressLine1 { get; set; }

//        public string AddressLine2 { get; set; }

//        public string AddressLine3 { get; set; }

//        public string AddressLine4 { get; set; }

//        [Required]
//        public string Postcode { get; set; }

//        [Required]
//        public string Country { get; set; }

//        [Required]
//        public string CountryCode { get; set; }

//        [Required]
//        public short CountryID { get; set; }
//    }

//    public class SampleOrder
//    {
//        [Required]
//        public SampleAccount account { get; set; }

//        [Required]
//        public SampleDeliveryAddress selectedAddress { get; set; }

//        [Required]
//        public SampleDispatchMethod selectedDispatchMethod { get; set; }

//        public string customerOrderReference { get; set; }

//        [Required]
//        public bool customerPayingForDispatch { get; set; }

//        [Required]
//        public bool customerWantsOrderConfirmation { get; set; }

//        [Required]
//        public byte selectedOrderConfirmationLanguage { get; set; }

//        [Required]
//        public List<SampleLine> basket { get; set; }

//        [StringLength(10)]
//        public string orderTaker { get; set; }
//    }

//    public class SampleDispatchMethod
//    {
//        public string Name { get; set; }

//        [Required]
//        public string HAULCODE { get; set; }

//        [Required]
//        public string CARRCODE { get; set; }
//    }

//    public class SampleOrderReturn
//    {
//        public string state { get; set; }
//        public string message { get; set; }
//    }

//    public class SampleAccount
//    {
//        public string AccountNumber { get; set; }
//        public string Email { get; set; }
//        public string Name { get; set; }
//    }
//    public class PassAddressLookupSearch
//    {
//        public string id { get; set; }
//        public string text { get; set; }
//        public string container { get; set; }
//        public string origin { get; set; }
//        public string countries { get; set; }
//        public string limit { get; set; }
//        public string language { get; set; }
//    }


//    public class PassAddressLookupRefine
//    {
//        public String refinement { get; set; }
//        public String moniker { get; set; }
//    }


//    public class PassAddressLookupSearchResult
//    {
//        public String text { get; set; }
//        public String moniker { get; set; }
//        public bool isMultipleAddresses { get; set; }
//    }


//    public class PassAddressLookupDetail
//    {
//        public String moniker { get; set; }
//    }


//    public class PassAddressLookupLine
//    {
//        public String label { get; set; }
//        public String data { get; set; }
//    }


//    public class TradeAccountAddressReturn
//    {
//        public List<TradeAccountPrimaryAddress> PrimaryAddresses { get; set; }

//        public List<TradeAccountSecondaryAddress> SecondaryAddresses { get; set; }
//    }

//    public class PassTradeAccountNumber
//    {
//        public string accountNumber { get; set; }
//    }

//    public class PassProductCodeAutoComplete
//    {
//        [StringLength(50)]
//        public string term { get; set; }

//        public short tradePriceListId { get; set; }
//    }

//    public class PassDesignID
//    {
//        public int designId { get; set; }
//    }

//    public class TradeAccountPrimaryAddress
//    {
//        public string Name { get; set; }
//        public string AddressLine1 { get; set; }
//        public string AddressLine2 { get; set; }
//        public string AddressLine3 { get; set; }
//        public string AddressLine4 { get; set; }
//        public string Postcode { get; set; }
//        public string Country { get; set; }
//        public string CountryCode { get; set; }
//        public short? CountryID { get; set; }
//    }

//    public class TradeAccountSecondaryAddress
//    {
//        public string DeliveryReference { get; set; }
//        public string Name { get; set; }
//        public string AddressLine1 { get; set; }
//        public string AddressLine2 { get; set; }
//        public string AddressLine3 { get; set; }
//        public string AddressLine4 { get; set; }
//        public string Postcode { get; set; }
//        public string Country { get; set; }
//        public string CountryCode { get; set; }
//        public short? CountryID { get; set; }
//    }
//}

//namespace Flock.ViewModels
//{
//    public class SamplesViewModel
//    {
//        public List<Controllers.SampleCountry> Countries { get; set; }

//        public List<TradeHaulier> Hauliers { get; set; }

//        public List<TradeCarriage> Carriages { get; set; }

//        public List<Controllers.SampleLanguage> Languages { get; set; }

//        public string DefaultOrderTaker { get; set; }
//    }
//}

//namespace Flock.Models
//{
//    public partial class FlockDataContext
//    {
//        //public Int32? TradeSampleOrder_Submit(string userId, Controllers.SampleOrder objOrder)
//        //{
//        //    try
//        //    {
//        //        System.Data.DataSet ds = new System.Data.DataSet();

//        //        DataTable DeliveryAddress = new DataTable();
//        //        DeliveryAddress.Columns.Add("FullName", typeof(string));
//        //        DeliveryAddress.Columns.Add("CompanyName", typeof(string));
//        //        DeliveryAddress.Columns.Add("AddressLine1", typeof(string));
//        //        DeliveryAddress.Columns.Add("AddressLine2", typeof(string));
//        //        DeliveryAddress.Columns.Add("Town", typeof(string));
//        //        DeliveryAddress.Columns.Add("County", typeof(string));
//        //        DeliveryAddress.Columns.Add("Postcode", typeof(string));
//        //        DeliveryAddress.Columns.Add("CountryID", typeof(short));
//        //        DeliveryAddress.Columns.Add("PhoneNumber", typeof(string));

//        //        DeliveryAddress.Rows.Add(
//        //            objOrder.selectedAddress.Name,
//        //            null,
//        //            objOrder.selectedAddress.AddressLine1,
//        //            objOrder.selectedAddress.AddressLine2,
//        //            objOrder.selectedAddress.AddressLine3,
//        //            objOrder.selectedAddress.AddressLine4,
//        //            objOrder.selectedAddress.Postcode,
//        //            objOrder.selectedAddress.CountryID,
//        //            null
//        //        );

//        //        DataTable Basket = new DataTable();
//        //        Basket.Columns.Add("Code", typeof(string));
//        //        Basket.Columns.Add("Quantity", typeof(short));
//        //        Basket.Columns.Add("LineRef", typeof(string));
//        //        Basket.Columns.Add("Discontinued", typeof(bool));
//        //        Basket.Columns.Add("UnitPrice", typeof(decimal));

//        //        for (var i = 0; i < objOrder.basket.Count; i++)
//        //        {
//        //            Basket.Rows.Add(
//        //                objOrder.basket[i].Code,
//        //                objOrder.basket[i].QuantityTotal,
//        //                objOrder.basket[i].LineRef,
//        //                objOrder.basket[i].Status == "D" ? true : false,
//        //                objOrder.basket[i].UnitPrice);
//        //        }

//        //        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
//        //        SqlCommand cmd = new SqlCommand();

//        //        cmd = new SqlCommand("adm.SampleTransaction_Generate", con);
//        //        cmd.CommandType = CommandType.StoredProcedure;

//        //        cmd.Parameters.AddWithValue("@FlockUserID", userId);
//        //        cmd.Parameters.AddWithValue("@MertexCustomerAccountNumber", objOrder.account.AccountNumber);
//        //        cmd.Parameters.AddWithValue("@CustomerOrderReference", objOrder.customerOrderReference);
//        //        cmd.Parameters.AddWithValue("@Email", objOrder.account.Email);
//        //        SqlParameter deliveryAddress = cmd.Parameters.AddWithValue("@DeliveryAddress", DeliveryAddress);
//        //        cmd.Parameters.AddWithValue("@HaulCode", objOrder.selectedDispatchMethod.HAULCODE);
//        //        cmd.Parameters.AddWithValue("@CarrCode", objOrder.selectedDispatchMethod.CARRCODE);
//        //        cmd.Parameters.AddWithValue("@CustomerPayingForDispatch", objOrder.customerPayingForDispatch);
//        //        SqlParameter sampleBasket = cmd.Parameters.AddWithValue("@SampleBasket", Basket);
//        //        cmd.Parameters.Add("@OrderHeadID", SqlDbType.Int);
//        //        cmd.Parameters.AddWithValue("@CustomerWantsOrderConfirmation", objOrder.customerWantsOrderConfirmation);
//        //        cmd.Parameters.AddWithValue("@LanguageID", objOrder.selectedOrderConfirmationLanguage);
//        //        cmd.Parameters.AddWithValue("@OrderTaker", objOrder.orderTaker);

//        //        deliveryAddress.SqlDbType = SqlDbType.Structured;
//        //        deliveryAddress.TypeName = "adm.AddressTableType";
                
//        //        sampleBasket.SqlDbType = SqlDbType.Structured;
//        //        sampleBasket.TypeName = "adm.SampleBasketTableType2";

//        //        cmd.Parameters["@OrderHeadID"].Direction = ParameterDirection.Output;

//        //        con.Open();

//        //        cmd.ExecuteNonQuery();

//        //        con.Close();

//        //        int? result = cmd.Parameters["@OrderHeadID"].Value as int?;

//        //        return result;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        throw ex;
//        //    }
//        //}
//    }
//}