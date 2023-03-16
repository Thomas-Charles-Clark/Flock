using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;
using Flock.DAL;
using System.ComponentModel.DataAnnotations;

namespace Flock.ViewModels
{
    public class PromoCodeViewModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;
        public PromoCodeViewModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public PromoCodeViewModel Build(short? promotionCodeID)
        {
            PromoCodeViewModel Model = new PromoCodeViewModel();

            Model.PromoCode_DeliveryBands = new List<PromotionCode_DeliveryBand>();
            Model.PromoCode_Members = new List<Member>();
            Model.PromoCode_MemberSegments = new List<PromotionCode_MemberSegment>();
            Model.PromoCode_Products = new List<vwPromoCodeProductImage>();
            Model.PromoCode_ProductTypes = new List<PromotionCode_ProductType>();


            if (promotionCodeID != null)
            {
                PromotionCode pc = UoW.GetRepository<PromotionCode>().GetAll().Where(p => p.PromotionCodeID == promotionCodeID).Single();

                Model.PromotionCodeID = promotionCodeID;
                Model.PromotionGroupID = pc.PromotionGroupID;
                Model.Code = pc.Code;
                Model.Description = pc.Description;
                Model.OnDate = pc.OnDate;
                Model.OffDate = pc.OffDate;
                Model.MinimumSpend = pc.MinimumSpend;
                Model.MaxUses = pc.MaxUses;
                Model.MaxUsesPerUser = pc.MaxUsesPerUser;
                Model.TransactionSequenceRestriction = pc.TransactionSequenceRestriction;
                Model.PriceListID = pc.PriceListID;
                Model.Discount = pc.Discount;
                Model.DiscountType = pc.DiscountType;
                Model.FreeGiftProductID = pc.FreeGiftProductID;
                Model.FreeGiftQuantity = pc.FreeGiftQuantity;
                Model.GiftBoxPriceEx = pc.GiftBoxPriceEx;
                Model.GiftBoxPriceInc = pc.GiftBoxPriceInc;
                Model.DiscountBundles = pc.DiscountBundles;
                Model.CreatedDT = pc.CreatedDT;

                Model.ApplyDiscountToTradePrice = pc.ApplyDiscountToTradePrice;
                Model.ApplyDiscountToSalePrice = pc.ApplyDiscountToSalePrice;
                Model.ApplyDiscountToFullPrice = pc.ApplyDiscountToFullPrice;
                
                Model.PromoCode_DeliveryBands = UoW.GetRepository<PromotionCode_DeliveryBand>().GetAll().Where(p => p.PromotionCodeID == Model.PromotionCodeID).ToList();
                if (Model.PromoCode_DeliveryBands.Count > 0)
                {
                    Model.DeliveryBandPriceEx = Model.PromoCode_DeliveryBands[0].PriceEx;
                    Model.DeliveryBandPriceInc = Model.PromoCode_DeliveryBands[0].PriceInc;
                }
                
                Model.PromoCode_MemberSegments = UoW.GetRepository<PromotionCode_MemberSegment>().GetAll().Where(p => p.PromotionCodeID == Model.PromotionCodeID).ToList();
                Model.PromoCode_ProductTypes = UoW.GetRepository<PromotionCode_ProductType>().GetAll().Where(p => p.PromotionCodeID == Model.PromotionCodeID).ToList();

                var MemberLinker = UoW.GetRepository<PromotionCode_Member>().GetAll().Where(p => p.PromotionCodeID == promotionCodeID).Select(p => p.MemberID).ToList();
                Model.PromoCode_Members = UoW.GetRepository<Member>().GetAll().Where(p => MemberLinker.Contains(p.MemberID)).ToList();

                var ProductLinker = UoW.GetRepository<PromotionCode_Product>().GetAll().Where(p => p.PromotionCodeID == promotionCodeID).Select(p => p.ProductID).ToList();
                Model.PromoCode_Products = UoW.GetRepository<vwPromoCodeProductImage>().GetAll().Where(p => p.PromotionCodeID == promotionCodeID).ToList();
            }
            else
            {
                Model.ApplyDiscountToFullPrice = true;
            }

            return Model;
        }
    }

    public class PromoCodeViewModel : BaseViewModel
    {
        public short? PromotionCodeID { get; set; }

        public byte PromotionGroupID { get; set; }

        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [Required]
        [StringLength(60)]
        public string Description { get; set; }

        public DateTime? OnDate { get; set; }

        public DateTime? OffDate { get; set; }

        public decimal? MinimumSpend { get; set; }

        public short? MaxUses { get; set; }

        public byte? MaxUsesPerUser { get; set; }

        public byte TransactionSequenceRestriction { get; set; }

        public short Used { get; set; }

        public byte? PriceListID { get; set; }

        public decimal? Discount { get; set; }

        public bool DiscountType { get; set; }


        public bool ApplyDiscountToFullPrice { get; set; }

        public bool ApplyDiscountToTradePrice { get; set; }

        public bool ApplyDiscountToSalePrice { get; set; }

        public bool DiscountBundles { get; set; }

        public int? FreeGiftProductID { get; set; }

        public byte? FreeGiftQuantity { get; set; }

        public DateTime CreatedDT { get; set; }

        public decimal DeliveryBandPriceInc { get; set; }

        public decimal DeliveryBandPriceEx { get; set; }

        public decimal? GiftBoxPriceInc { get; set; }

        public decimal? GiftBoxPriceEx { get; set; }

        public List<PromotionCode_DeliveryBand> PromoCode_DeliveryBands { get; set; }

        public List<Member> PromoCode_Members { get; set; }

        public List<PromotionCode_MemberSegment> PromoCode_MemberSegments { get; set; }

        public List<vwPromoCodeProductImage> PromoCode_Products { get; set; }

        public List<PromotionCode_ProductType> PromoCode_ProductTypes { get; set; }
    }
}