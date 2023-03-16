using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.DAL.SignalR;
using Flock.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using NLog;
using System.Data.Common;
using Microsoft.AspNet.SignalR;
using MertexAccountChecks;

namespace Flock.ViewModels
{
    public class MemberViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;
        public MemberViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public MemberViewModel Build(int? MemberID)
        {
            MemberViewModel Model = new MemberViewModel();

            Member m = UoW.GetRepository<Member>().GetAll().Where(p => p.MemberID == MemberID).Single();

            Model.MemberID = m.MemberID;
            Model.DefaultMemberAddressID = m.DefaultMemberAddressID;
            Model.GUID = m.GUID;
            Model.CreatedDT = m.CreatedDT;
            Model.ModifiedDT = m.ModifiedDT;
            Model.CountryID = m.CountryID;
            Model.LanguageID = m.LanguageID;
            Model.FullName = m.FullName;
            Model.Email = m.Email;
            Model.EmailConfirmed = m.EmailConfirmed;
            Model.Password = m.Password;
            Model.OldPassword = m.OldPassword;
            return Model;
        }

        public MemberAddressEditModel BuildAddressEditModel(long MemberAddressID)
        {
            var Model = new MemberAddressEditModel();

            var ma = UoW.GetRepository<MemberAddress>().GetAll().Where(p => p.MemberAddressID == MemberAddressID).Single();

            Model.MemberAddressID = ma.MemberAddressID;
            Model.MemberID = ma.MemberID;
            Model.FullName = ma.FullName;
            Model.AddressLine1 = ma.AddressLine1;
            Model.AddressLine2 = ma.AddressLine2;
            Model.Town = ma.Town;
            Model.County = ma.County;
            Model.Postcode = ma.Postcode;
            Model.CountryID = ma.CountryID;
            Model.PhoneNumber = ma.PhoneNumber;
            Model.CreatedDT = ma.CreatedDT;
            Model.DeletedDT = ma.DeletedDT;

            return Model;
        }
    }

    public class MemberViewModel : BaseViewModel
    {
        public int? MemberID { get; set; }

        public long? DefaultMemberAddressID { get; set; }

        public Guid GUID { get; set; }

        public DateTime CreatedDT { get; set; }

        public DateTime? ModifiedDT { get; set; }

        public DateTime? LoginDT { get; set; }

        public short? CountryID { get; set; }

        public byte? LanguageID { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [StringLength(64)]
        public string Password { get; set; }

        [StringLength(40)]
        public string OldPassword { get; set; }
    }

    public class MemberDetailViewModelBuilder
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UoW;
        private string ConnectionString;
        //private DbCommand BasketCommand;
        //private DbCommand VisitorInstancesCommand;
        private int MemberID;

        public MemberDetailViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
            ConnectionString = this.UoW.Context.Connection.ConnectionString;
        }

        public MemberDetailViewModel Build(int? MemberID)
        {
            MemberDetailViewModel Model = new MemberDetailViewModel();

            Model.TradeAccountChecksResult = null;

            var Transactions = UoW.GetRepository<Transaction>().GetAll().Where(p => p.MemberID == MemberID).ToList();//.Select(p => p.TransactionID).ToList();

            Model.Member = UoW.GetRepository<Member>().GetAll().Where(p => p.MemberID == MemberID).SingleOrDefault();

            Model.Transactions = Transactions;//new List<Order>();


            //if (Model.Member.MertexCustomerAccountNumber != null)
            //{
            //    // plug in the Mertex Customer Checker, run the checkout check, show results..
            //    try
            //    {
            //        Model.TradeAccountChecksResult = MertexAccountChecks.TradeCheckoutChecks.Check(Model.Member.MertexCustomerAccountNumber, Model.Member.Email, null);
            //    }
            //    catch(MertexAccountException mae)
            //    {
            //        Model.TradeAccountChecksResult = new TradeCheckoutCheckResult() { CanPlaceProductOrders = false, CanPlaceSampleOrders = false, AccountOpenClosedText = "FAILED TO MATCH ACCOUNT NUMBER TO EMAIL " + mae.Message };
            //    }
            //}
            
            //foreach (var o in Orders)
            //{
            //    var Order = new Order();
            //    Order.OrderHeadID = o.Select(p => p.OrderHeadID).Single();
            //    Order.MemberID = MemberID;
            //    Order.CreatedDT = o.CreatedDT;
            //    Order.Status = o.Status;
            //    Order.OrderLines = UoW.GetRepository<OrderLine>().GetAll().Where(p => p.OrderHeadID == o.OrderHeadID).ToList();
            //}

            Model.Basket = UoW.Context.Member_Basket_Select(MemberID, null).ToList();

            Model.AddressBook = UoW.GetRepository<MemberAddress>().GetAll().Where(p => p.MemberID == MemberID).ToList();// new List<MemberAddress>();

            Model.VisitorInstances = UoW.GetRepository<Visitor>().GetAll().Where(p => p.MemberID == MemberID).ToList();

            //BasketCommand = UoW.Context.GetCommand(UoW.GetRepository<Member_Basket>().GetAll().Where(p => p.MemberID == MemberID));
            //RegisterNewBasketDependency();
            
            //VisitorInstancesCommand = UoW.Context.GetCommand(UoW.GetRepository<Visitor>().GetAll().Where(p => p.MemberID == MemberID));
            //RegisterNewVisitorDependency();

            this.MemberID = MemberID.Value;
            
            return Model;
        }

        void RegisterNewBasketDependency()
        {
            //var SqlConnection = new SqlConnection(ConnectionString);
            //SqlConnection.Open();

            //var SqlCommand = new SqlCommand(BasketCommand.CommandText, SqlConnection);

            //foreach (var p in BasketCommand.Parameters)
            //{
            //    var x = p as SqlParameter;
            //    var y = new SqlParameter(x.ParameterName, x.SqlDbType, x.Size);
            //    y.Value = x.Value;

            //    SqlCommand.Parameters.Add(y);
            //}

            //SqlDependency Dependency = new SqlDependency(SqlCommand);
            //Dependency.OnChange += BasketDependency_OnChange;
            
            //SqlDependency.Start(ConnectionString);

            //SqlCommand.ExecuteReader();

            //SqlConnection.Close();
        }

        void BasketDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //var dep = sender as SqlDependency;
            //dep.OnChange -= BasketDependency_OnChange;
            
            //MemberBasketPusher.Instance.BroadcastMemberBasket(MemberID);
            
            ////Logger.Info("Change detected: " + e.Info.ToString()); 

            //RegisterNewBasketDependency();
        }

        void RegisterNewVisitorDependency()
        {
            //var SqlConnection = new SqlConnection(ConnectionString);
            //SqlConnection.Open();

            //var SqlCommand = new SqlCommand(VisitorInstancesCommand.CommandText, SqlConnection);

            //foreach (var p in BasketCommand.Parameters)
            //{
            //    var x = p as SqlParameter;
            //    var y = new SqlParameter(x.ParameterName, x.SqlDbType, x.Size);
            //    y.Value = x.Value;

            //    SqlCommand.Parameters.Add(y);
            //}

            //SqlDependency Dependency = new SqlDependency(SqlCommand);
            //Dependency.OnChange += VisitorDependency_OnChange;

            //SqlDependency.Start(ConnectionString);

            //SqlCommand.ExecuteReader();

            //SqlConnection.Close();
        }

        void VisitorDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //var dep = sender as SqlDependency;
            //dep.OnChange -= VisitorDependency_OnChange;

            //VisitorPusher.Instance.BroadcastVisitor(MemberID);

            //RegisterNewVisitorDependency();
        }
    }

    public class MemberDetailViewModel
    {
        public int? MemberID { get; set; }

        public Member Member { get; set; }

        public TradeCheckoutCheckResult TradeAccountChecksResult { get; set; }

        public List<Member_Basket_SelectResult> Basket { get; set; }

        public List<Flock.Models.Transaction> Transactions { get; set; }

        public List<Flock.Models.MemberAddress> AddressBook { get; set; }

        public List<Flock.Models.Visitor> VisitorInstances { get; set; }
    }

    public class MemberAddressEditModel
    {
        public long MemberAddressID { get; set; }
        public int MemberID { get; set; }
        public bool PrimaryAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(60)]
        public string AddressLine1 { get; set; }

        [Required]
        [StringLength(60)]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(50)]
        public string Town { get; set; }

        [Required]
        [StringLength(40)]
        public string County { get; set; }

        [StringLength(20)]
        public string Postcode { get; set; }

        [Required]
        public short CountryID { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public DateTime CreatedDT { get; set; }

        public DateTime? DeletedDT { get; set; }
    }

    public class Order 
    {
        public int OrderHeadID { get; set; }
        public int? MemberID { get; set; }
        public DateTime CreatedDT { get; set; }
        public string Status { get; set; }
        public List<OrderLine> OrderLines { get; set; }
    }

    public class Basket
    {
        public long BasketID { get; set; }
        public byte MarketID { get; set; }
        public int MemberID { get; set; }
        public DateTime CreatedDT { get; set; }
        public DateTime ModifiedDT { get; set; }
        public bool Deleted { get; set; }
        public bool Converted { get; set; }
        public bool Expired { get; set; }

        public List<Basket_Product> Products { get; set; }
        public List<Basket_Voucher> Vouchers { get; set; }
    }
}