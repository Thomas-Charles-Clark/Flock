using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Flock.DAL;
using Flock.Models;

namespace Flock.ViewModels
{
    public class GiftVoucherViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public GiftVoucherViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public GiftVoucherViewModel Build(short? GiftVoucherID)
        {
            GiftVoucherViewModel Model = new GiftVoucherViewModel();
            GiftVoucher gv = UoW.GetRepository<GiftVoucher>().GetAll().Where(p => p.GiftVoucherID == GiftVoucherID).Single();

            Model.GiftVoucherID = GiftVoucherID;

            Model.PriceListID = gv.PriceListID;

            Model.Code = gv.Code;

            Model.ValueInc = gv.ValueInc;

            Model.ValueEx = gv.ValueEx;

            Model.AuthorisedDT = gv.AuthorisedDT;

            Model.ExpiryDT = gv.ExpiryDT;

            Model.UsedDT = gv.UsedDT;

            Model.CancelledDT = gv.CancelledDT;

            Model.RedeemTransactionID = gv.RedeemTransactionID;

            Model.SaleTransactionID = gv.SaleTransactionID;

            Model.CreatorUserID = gv.CreatorUserID;

            Model.CreatedDT = gv.CreatedDT;

            Model.UsedDT = gv.UsedDT;

            return Model;
        }
    }

    public class GiftVoucherViewModel : BaseViewModel
    {
        public short? GiftVoucherID { get; set; }

        public byte GiftVoucherTypeID { get; set; }

        public int? CreditTransactionID { get; set; }

        public string Reason { get; set; }

        public byte? PriceListID { get; set; }

        public string Code { get; set; }

        public decimal ValueInc { get; set; }

        public decimal ValueEx { get; set; }

        public DateTime? AuthorisedDT { get; set; }

        public DateTime? ExpiryDT { get; set; }

        public DateTime? UsedDT { get; set; }

        public DateTime? CancelledDT { get; set; }

        public int? RedeemTransactionID { get; set; }

        public int? SaleTransactionID { get; set; }

        public string CreatorUserID { get; set; }

        public DateTime CreatedDT { get; set; }
    }
}