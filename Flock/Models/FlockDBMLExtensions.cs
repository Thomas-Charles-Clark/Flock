using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Web;
using DevExpress.Web.Mvc;
using System.Web.Script.Serialization;
using DevExpress.Web;
using Flock.Models;
using Flock.ViewModels;

namespace Flock.Models
{
    public class TradeSwitchOption
    {
        public string Name { get; set; }
        public int? Value { get; set; }
    }

    public class PassAutocomplete
    {
        public string term { get; set; }
    }

    public class AttributeResultTable
    {
        public short AttributeID { get; set; }
        public byte AttributeGroupID { get; set; }
    }
    public class ProductTypeSuggestionResult
    {
        public byte? ProductTypeID { get; set; }
    }
    public class BrandSuggestionResult
    {
        public byte? BrandID { get; set; }
    }
    public class LaunchSuggestionResult
    {
        public short? LaunchID { get; set; }
    }

    public class SalePriceValidateResult
    {
        public int ProductID { get; set; }
    }

    public class ImageInspectHotspotsResult
    {
        public long? ProductID { get; set; }
        public float? X { get; set; }
        public float? Y { get; set; }
        public string ProductName { get; set; }
        public string Code { get; set; }
        public char Status { get; set; }
        public long? Front { get; set; }
        public int? ProductPageID { get; set; }
        public string ProductPageName { get; set; }
        public string URLKeywords { get; set; }
        public byte? WebStatusID { get; set; }
        public DateTime? DeletedDT { get; set; }
    }

    public class ImageInspectUsageResult_Product
    {
        public string ImageUse { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public char Status { get; set; }
        public int ProductID { get; set; }
        public byte SortOrder { get; set; }
        public DateTime? DeletedDT { get; set; }
    }

    public class ImageInspectUsageResult_ProductPage
    {
        public string Template { get; set; }
        public string Name { get; set; }
        public byte WebStatusID { get; set; }
        public string ImageUse { get; set; }
        public int ProductPageID { get; set; }
        public byte SortOrder { get; set; }
        public DateTime? DeletedDT { get; set; }
    }

    public class ImageInspectUsageResult_Collection
    {
        public string ImageUse { get; set; }
        public string Name { get; set; }
        public byte WebStatusID { get; set; }
        public int CollectionID { get; set; }
        public byte SortOrder { get; set; }
        public DateTime? DeletedDT { get; set; }
    }

    public class ImageInspectUsageResult_Pods
    {
        public string ImageUse { get; set; }
        public string Name { get; set; }
        public int PageID { get; set; }
        public byte SortOrder { get; set; }
        public DateTime? DeletedDT { get; set; }
    }

    public class ImageInspectUsageResult_Slideshow
    {
        public string Name { get; set; }
        public byte WebStatusID { get; set; }
        public byte SortOrder { get; set; }
        public DateTime? DeletedDT { get; set; }
    }

    public class PPRPPS_TotalCount
    {
        public int TotalCount { get; set; }
    }

    public class PPRPPS_One
    {
        public int ProductPageID { get; set; }
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public long? ImageID { get; set; }
        public byte? StatusID { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public bool Clearance { get; set; }
    }

    public class PPRPPS_Two
    {
        public int ProductPageID { get; set; }
        public int RPP_ProductPageID { get; set; }
        public string RPP_Heading { get; set; }
        public string RPP_SubHeading { get; set; }
        public long? RPP_ImageID { get; set; }
        public byte? RPP_StatusID { get; set; }
        public byte? RPP_SortOrder { get; set; }
        public string RPP_Name { get; set; }
        public string RPP_ProductType { get; set; }
        public bool Clearance { get; set; }
    }

    public class ProductPageAlternateColourwaySelect_First
    {
        public int DesignID { get; set; }
        public string Name { get; set; }
        public byte ProductTypeID { get; set; }
        public short LaunchID { get; set; }
        public byte? BrandID { get; set; }
        public int ProductPageCount { get; set; }
    }

    public class ProductPageAlternateColourwaySelect_Second
    {
        public int DesignID { get; set; }
        public int ProductPageID { get; set; }
        public byte ProductTypeID { get; set; }
        public string Name { get; set; }
        public byte SortOrder { get; set; }
        public byte? BrandID { get; set; }
        public long? ImageID { get; set; }
    }

    public class ImageUsage_Product
    {
        public long ImageID { get; set; }
        public int ProductID { get; set; }
        public string Detail { get; set; }
    }

    public class ImageUsage_ProductPage
    {
        public long ImageID { get; set; }
        public int ProductPageID { get; set; }
        public string Detail { get; set; }
    }

    public class ImageUsage_Page
    {
        public long ImageID { get; set; }
        public int PageID { get; set; }
        public string Detail { get; set; }
    }

    public class ImageUsage_Collection
    {
        public long ImageID { get; set; }
        public short CollectionID { get; set; }
        public string Detail { get; set; }
    }
    
    public class ImageUsage_Slideshow
    {
        public long ImageID { get; set; }
        public long SlideShowID { get; set; }
        public string Detail { get; set; }
    } 

    public class ImageUsage_Article
    {
        public long ImageID { get; set; }
        public int ArticleID { get; set; }
        public string Detail { get; set; }
    }

    public class ImageUsage_Animation
    {
        public long ImageID { get; set; }
        public short AnimationID { get; set; }
        public string Detail { get; set; }
    }

    //public class ProductPage_Attribute_Generate_AttributeResult
    //{
    //    public short AttributeID { get; set; }
    //    public string Name { get; set; }
    //    public byte SortOrder { get; set; }
    //}

    public class ProductPage_Attribute_Generate_ProductPageResult
    {
        public int? ProductPageID { get; set; }
        public string Name { get; set; }
        public long? ImageID { get; set; }
        public byte? WebStatus { get; set; }
    }

    public class ProductPage_Attribute_Generate_AttributeMapResult
    {
        public int ProductPageID { get; set; }
        public short AttributeID { get; set; }
    }

    public class Quality_Overview_Generate_ProductTypeResult
    {
        public byte ProductTypeID { get; set; }
        public string Name { get; set; }
    }

    public class Quality_Overview_Generate_LaunchResult
    {
        public short LaunchID { get; set; }
        public string Name { get; set; }
    }

    public class Quality_Overview_Generate_QualityCollectionResult
    {
        public Int16 QualityID { get; set; }
        public string Name { get; set; }
    }

    public class Quality_Overview_Generate_BrandResult
    {
        public byte BrandID { get; set; }
        public string Name { get; set; }
    }

    public class Quality_Overview_Generate_QualityResult
    {
        public Int16 QualityID { get; set; }
        public Byte ProductTypeID { get; set; }
        public Int16 FirstLaunchID { get; set; }
        public Int16 LastLaunchID { get; set; }
        public Int32 ProductCount { get; set; }
        public Int16 CollectionCount { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }
        public Double? Width { get; set; }
        public Double? Weight { get; set; }
        public Double? HorizontalPatternRepeat { get; set; }
        public Double? VerticalPatternRepeat { get; set; }
        public String Composition { get; set; }
        public Int32? Martindale { get; set; }
        public Byte BrandID { get; set; }
        public String UsageIDs { get; set; }
        public String AftercareIDs { get; set; }
        public String FinishIDs { get; set; }
        public String TechnicalDescriptionIDs { get; set; }
    }

    public class Quality_Overview_Detail_StatusResult
    {
        public byte StatusID { get; set; }
        public string Name { get; set; }
    }

    public class Quality_Overview_Detail_ProductPageResult
    {
        public short QualityID { get; set; }
        public int ProductID { get; set; }
        public string Code { get; set; }
        public byte ProductTypeID { get; set; }
        public int? ProductPageID { get; set; }
        public long? ImageID { get; set; }
        public byte StatusID { get; set; }
        public string MetaDescription { get; set; }
        public string SubHeading { get; set; }
        public string Description { get; set; }
    }

    public class SalesOverviewSummary
    {
        public decimal? Total { get; set; }
        public decimal? AOV { get; set; }
        public int? Orders { get; set; }
        public int? AverageOrdersPerDay { get; set; }
        public decimal? AverageTotalQuantity { get; set; }
        public int? FirstOrders { get; set; }
        public int? RepeatOrders { get; set; }
    }

    public class SalesOverviewTransaction
    {
        public int TransactionID { get; set; }
        public decimal TransactionTotal { get; set; }
        public DateTime TransactionDate { get; set; }
        public short? TotalQuantity { get; set; }
        public string Email { get; set; }
        public long TransactionSequence { get; set; }
        public int MemberID { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public long CheckoutID { get; set; }
        public string PaymentType { get; set; }
    }

    public class ProductImage_Export_ProductImageResult
    {
        public string Launch { get; set; }
        public string Brand { get; set; }
        public string ProductType { get; set; }
        public string Collection { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SizeMetric { get; set; }
        public string SizeImperial { get; set; }
        public long? ImageID { get; set; }
        public string Path { get; set; }
        public string ImageFileName { get; set; }
    }

    public class ProductImage_Export_LaunchResult
    {
        public short LaunchID { get; set; }
        public string Launch { get; set; }
    }

    public class ProductImage_Export_BrandResult
    {
        public short LaunchID { get; set; }
        public byte BrandID { get; set; }
        public string Brand { get; set; }
    }

    public class ProductImage_Export_ProductTypeResult
    {
        public short LaunchID { get; set; }
        public byte BrandID { get; set; }
        public byte ProductTypeID { get; set; }
        public string ProductType { get; set; }
    }

    public class ProductImage_Export_CollectionResult
    {
        public short LaunchID { get; set; }
        public byte BrandID { get; set; }
        public byte ProductTypeID { get; set; }
        public short CollectionID { get; set; }
        public string Collection { get; set; }
    }




    public class CollectionImage_Export_CollectionImageResult
    {
        public string Launch { get; set; }
        public string Brand { get; set; }
        public string ProductType { get; set; }
        public string Collection { get; set; }
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string Description { get; set; }
        public long? ImageID { get; set; }
        public string Path { get; set; }
        public string ImageFileName { get; set; }
    }

    public class CollectionImage_Export_LaunchResult
    {
        public short LaunchID { get; set; }
        public string Launch { get; set; }
    }

    public class CollectionImage_Export_BrandResult
    {
        public short LaunchID { get; set; }
        public byte BrandID { get; set; }
        public string Brand { get; set; }
    }

    public class CollectionImage_Export_ProductTypeResult
    {
        public short LaunchID { get; set; }
        public byte BrandID { get; set; }
        public byte ProductTypeID { get; set; }
        public string ProductType { get; set; }
    }

    public class CollectionImage_Export_CollectionResult
    {
        public short LaunchID { get; set; }
        public byte BrandID { get; set; }
        public byte ProductTypeID { get; set; }
        public short CollectionID { get; set; }
        public string Collection { get; set; }
    }


    public class ProductPage_Language_SelectList_ProductPageResult
    {
        public int ProductPageID { get; set; }
        public string Name { get; set; }
        public byte? BrandID { get; set; }
        public short? LaunchID { get; set; }
        public byte ProductTypeID { get; set; }
        public byte TemplateID { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
        public bool LifestyleProduct { get; set; }
    }

    //public class SiteSection_SelectResult
    //{
    //    public byte SiteSectionID { get; set; }
    //    public string Name { get; set; }
    //}

    public class Brand_SelectResult
    {
        public byte BrandID { get; set; }
        public string Name { get; set; }
    }

    public class Launch_SelectResult
    {
        public short LaunchID { get; set; }
        public string Name { get; set; }
        public DateTime LaunchDate { get; set; }
    }

    public class ProductPage_TemplateSelectResult
    {
        public byte TemplateID { get; set; }
        public string Name { get; set; }
    }

    public class ProductType_SelectResult
    {
        public byte ProductTypeID { get; set; }
        public string Name { get; set; }
    }

    public class Status_SelectResult
    {
        public byte StatusID { get; set; }
        public string Name { get; set; }
    }

    public class Template_SelectResult
    {
        public byte TemplateID { get; set; }
        public string Name { get; set; }
    }

    public class ProductPage_Language_SelectDetail_ProductPageResult
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string Description { get; set; }
        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
    }

    public class ProductPage_Language_SelectDetail_TranslationResult
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string Description { get; set; }
        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
    }

    public class ProductPage_Language_SelectDetail_ReviewRequiredResult
    {
        public bool Heading { get; set; }
        public bool SubHeading { get; set; }
        public bool Description { get; set; }
        public bool TitleTag { get; set; }
        public bool MetaDescription { get; set; }
    }

    public class ProductPage_Language_SelectDetail_ImageResult
    {
        public long ImageID { get; set; }
    }

    public class HtmlSection_Language_SelectList_HtmlSectionResult
    {
        public int PageID { get; set; }
        public string PageName { get; set; }
        public long HTMLSectionID { get; set; }
        public bool? TradeSwitch { get; set; }
        public string Name { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class HtmlSection_Language_SelectDetail_HtmlSectionResult
    {
        public string Contents { get; set; }
    }

    public class HtmlSection_Language_SelectDetail_TranslationResult
    {
        public string Contents { get; set; }
    }

    public class HtmlSection_Language_SelectDetail_ReviewRequiredResult
    {
        public bool Contents { get; set; }
    }





    public class Snippet_Language_SelectList_SnippetResult
    {
        public short SnippetID { get; set; }
        public string Name { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class Snippet_Language_SelectDetail_SnippetResult
    {
        public string HTMLContent { get; set; }
    }

    public class Snippet_Language_SelectDetail_TranslationResult
    {
        public string HTMLContent { get; set; }
    }

    public class Snippet_Language_SelectDetail_ReviewRequiredResult
    {
        public bool HTMLContent { get; set; }
    }




    public class Page_Language_SelectList_PageResult
    {
        public int PageID { get; set; }
        public byte SiteSectionID { get; set; }
        public string Name { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class Page_Language_SelectDetail_PageResult
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string URLKeywords { get; set; }
        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
        public string Description { get; set; }
    }

    public class TechInfo_Usage_Language_SelectList_TechInfo_UsageResult
    {
        public short UsageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }
    public class TechInfo_Type_Language_SelectList_TechInfo_TypeResult
    {
        public short TypeID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }
    public class TechInfo_Test_Language_SelectList_TechInfo_TestResult
    {
        public short TestID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }
    public class TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult
    {
        public short PatternMatchID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }
    public class TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult
    {
        public short FlamcodeID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }
    public class TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult
    {
        public short DomesticFRID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }
    public class TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult
    {
        public short ContractFRID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }
    public class TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult
    {
        public short CompositionID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }
    public class TechInfo_Aftercare_Language_SelectList_AftercareResult
    {
        public short AftercareID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }
    public class TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult
    {
        public short AdditionalInfoID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class Page_Language_SelectDetail_TranslationResult
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string URLKeywords { get; set; }
        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
        public string Description { get; set; }
    }

    public class Page_Language_SelectDetail_ReviewRequiredResult
    {
        public bool Heading { get; set; }
        public bool SubHeading { get; set; }
        public bool URLKeywords { get; set; }
        public bool Description { get; set; }
        public bool TitleTag { get; set; }
        public bool MetaDescription { get; set; }
    }

    public class Page_Language_SelectDetail_ImageResult
    {
        public long ImageID { get; set; }
    }

    public class EmailText_Language_SelectList_EmailTextResult
    {
        public short EmailTextID { get; set; }
        public int EmailTemplateID { get; set; }
        public byte LanguageID { get; set; }
        public string Text { get; set; }
        public string TranslatedText { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class EmailText_Language_SelectList_EmailTemplateResult
    {
        public int EmailTemplateID { get; set; }
        public string Name { get; set; }
    }

    public class Resource_Language_SelectList_ResourceResult
    {
        public short ResourceID { get; set; }
        public short CountryID { get; set; }
        public byte LanguageID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string TranslatedValue { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class Announcement_Language_SelectList_AnnouncementResult
    {
        public short AnnouncementID { get; set; }
        public byte LanguageID { get; set; }
        public string Text { get; set; }
        public string TranslatedText { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class Aftercare_Language_SelectList_AftercareResult
    {
        public byte AftercareID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class AttributeGroup_Language_SelectList_AttributeGroupResult
    {
        public byte AttributeGroupID { get; set; }
        public byte LanguageID { get; set; }
        public string Heading { get; set; }
        public string TranslatedHeading { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class Attribute_Language_SelectList_AttributeResult
    {
        public short AttributeID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class SlideShowImage_Language_SelectList_SlideShowImageResult
    {
        public Byte SiteSectionID { get; set; }
        public String Name { get; set; }
        public Int32 PageID { get; set; }
        public Int64 PageSectionID { get; set; }
        public Int64 SlideShowID { get; set; }
        public Int64 ImageID { get; set; }
        public byte LanguageID { get; set; }
        public string OverlayHeading { get; set; }
        public string OverlayText { get; set; }
        public string TranslatedOverlayHeading { get; set; }
        public string TranslatedOverlayText { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class OrderStatus_Language_SelectList_OrderStatusResult
    {
        public byte OrderStatusID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class SiteSection_Language_SelectList_SiteSectionResult
    {
        public byte SiteSectionID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class StockStatus_Language_SelectList_StockStatusResult
    {
        public byte StockStatusID { get; set; }
        public byte LanguageID { get; set; }
        public string StockStatus { get; set; }
        public string TranslatedStockStatus { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class Menu_Language_SelectList_MenuResult
    {
        public byte MenuID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public string MessageHTML { get; set; }
        public string TranslatedName { get; set; }
        public string TranslatedMessageHTML { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class MenuItem_Language_SelectList_MenuItemResult
    {
        public byte MenuID { get; set; }
        public short MenuItemID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string ExternalURL { get; set; }
        public string QueryStringParameters { get; set; }
        public string TranslatedName { get; set; }
        public string TranslatedExternalURL { get; set; }
        public string TranslatedQueryStringParameters { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class Menu_SelectResult
    {
        public byte MenuID { get; set; }
        public string Name { get; set; }
    }

    public class CallToAction_Language_SelectList_CallToActionResult
    {
        public int CallToActionID { get; set; }
        public byte LanguageID { get; set; }
        public string Text { get; set; }
        public string TranslatedText { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class OrderFormGroup_Language_SelectList_OrderFormGroupResult
    {
        public short OrderFormGroupID { get; set; }
        public byte LanguageID { get; set; }
        public string OrderFormGroup { get; set; }
        public string TranslatedOrderFormGroup { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class ArticleType_Language_SelectList_ArticleTypeResult
    {
        public byte ArticleTypeID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class ArticleSubType_Language_SelectList_ArticleSubTypeResult
    {
        public byte ArticleSubTypeID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class Module_Language_SelectList_ModuleResult
    {
        public Int16 ModuleID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class ModuleOption_Language_SelectList_ModuleOptionResult
    {
        public Int32 ModuleOptionID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult
    {
        public Int16 ModuleOptionGroupID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public string Description { get; set; }
        public string TranslatedDescription { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class ReturnReason_Language_SelectList_ReturnReasonResult
    {
        public byte ReturnReasonID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }
    
    public class Room_Language_SelectList_RoomResult
    {
        public byte RoomID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class DeliveryOption_Language_SelectList_DeliveryOptionResult
    {
        public byte DeliveryOptionID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult
    {
        public byte GiftVoucherValidationID { get; set; }
        public byte LanguageID { get; set; }
        public string OriginalMessage { get; set; }
        public string TranslatedMessage { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    #region Product Translation Classes

    public class Product_Language_SelectList_ProductResult
    {
        public int ProductID { get; set; }
        public short LaunchID { get; set; }
        public byte LanguageID { get; set; }
        public string Code { get; set; }
        public string OrderFormName { get; set; }
        public string TranslatedOrderFormName { get; set; }
        public string BasketName { get; set; }
        public string TranslatedBasketName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    #endregion

    #region Form Translation Classes 
    
    public class Form_Language_SelectList_FormResult
    {
        public short FormID { get; set; }
        public byte LanguageID { get; set; }
        public string Heading { get; set; }
        public string TranslatedHeading { get; set; }
        public string SubHeading { get; set; }
        public string TranslatedSubHeading { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    #endregion

    #region Colour Translation Classes

    public class Pattern_Language_SelectList_PatternResult
    {
        public byte PatternID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    #endregion

    #region Colour Translation Classes

    public class Colour_Language_SelectList_ColourResult
    {
        public byte ColourID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    #endregion

    #region Image Translation Classes

    public class Image_Language_SelectList_ImageResult
    {
        public long ImageID { get; set; }
        public string Name { get; set; }
        public byte LanguageID { get; set; }
        public string TitleText { get; set; }
        public string TranslatedTitleText { get; set; }
        public string AltText { get; set; }
        public string TranslatedAltText { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    #endregion

    #region PageSection Translation Classes

    public class PageSection_Language_SelectList_PageSectionResult
    {
        public long PageSectionID { get; set; }
        public bool? TradeSwitch { get; set; }
        public string Heading { get; set; }
        public string TranslatedHeading { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    #endregion

    #region Finish Translation Classes

    public class Finish_Language_SelectList_FinishResult
    {
        public byte FinishID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    #endregion

    #region ProductType Translation Classes

    public class ProductType_Language_SelectList_ProductTypeResult
    {
        public byte ProductTypeID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public string URLKeywords { get; set; }
        public string TranslatedURLKeywords { get; set; }
        public string NamePlural { get; set; }
        public string TranslatedNamePlural { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    #endregion

    #region TechnicalDescription Translation Classes

    public class TechnicalDescription_Language_SelectList_TechnicalDescriptionResult
    {
        public short TechnicalDescriptionID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    #endregion

    #region Usage Translation Classes

    public class Usage_Language_SelectList_UsageResult
    {
        public byte UsageID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    #endregion

    #region Collection Translation Classes

    public class Collection_Language_SelectList_CollectionResult
    {
        public short CollectionID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public byte BrandID { get; set; }
        public short LaunchID { get; set; }
        public byte ProductTypeID { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class Collection_Language_SelectDetail_CollectionResult
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string Description { get; set; }
        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
    }

    public class Collection_Language_SelectDetail_TranslationResult
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string Description { get; set; }
        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
    }

    public class Collection_Language_SelectDetail_ReviewRequiredResult
    {
        public bool Heading { get; set; }
        public bool SubHeading { get; set; }
        public bool Description { get; set; }
        public bool TitleTag { get; set; }
        public bool MetaDescription { get; set; }
    }

    public class Collection_Language_SelectDetail_ImageResult
    {
        public long? ImageID { get; set; }
    }

    #endregion

    #region Article Translation Classes

    public class Article_Language_SelectList_ArticleResult
    {
        public int ArticleID { get; set; }
        public byte? ArticleSubTypeID { get; set; }
        public string Name { get; set; }
        public byte? BrandID { get; set; }
        public byte StatusID { get; set; }
        public bool? Stage { get; set; }
        public byte FieldsToReview { get; set; }
    }

    public class Article_Language_SelectDetail_ArticleResult
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string Contents { get; set; }
        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
    }

    public class Article_Language_SelectDetail_TranslationResult
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string Contents { get; set; }
        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
    }

    public class Article_Language_SelectDetail_ReviewRequiredResult
    {
        public bool Heading { get; set; }
        public bool SubHeading { get; set; }
        public bool Contents { get; set; }
        public bool TitleTag { get; set; }
        public bool MetaDescription { get; set; }
    }

    public class Article_Language_SelectDetail_ImageResult
    {
        public long ImageID { get; set; }
    }

    public class ArticleSubType_SelectResult
    {
        public byte ArticleSubTypeID { get; set; }
        public string Name { get; set; }
    }
    #endregion

    public class Module_SelectResult
    {
        public Int16 ModuleID { get; set; }
        public string Name { get; set; }
    }

    public class ModuleOption_SelectResult
    {
        public Int32 ModuleOptionID { get; set; }
        public string Name { get; set; }
    }

    public class ModuleOptionGroup_SelectResult
    {
        public Int16 ModuleOptionGroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class MenuBuilder_MenuSelectResult
    {
        public byte MenuID { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public string MessageHTML { get; set; }
        public string TradeMessageHTML { get; set; }
        public byte SortOrder { get; set; }
        public int? PageID { get; set; }
        public string ExternalURL { get; set; }
        public bool? NewWindow { get; set; }
        public short? FontClassID { get; set; }
        public bool NoFollow { get; set; }
        public String QueryStringParameters { get; set; }
        public bool TrackExternalURL { get; set; }
        public DateTime? OnDate { get; set; }
        public DateTime? OffDate { get; set; }
        public byte? StatusID { get; set; }
        public bool? TradeSwitch { get; set; }
    }

    public class MenuBuilder_MenuCountrySelectResult
    {
        public byte MenuID { get; set; }
        public short CountryID { get; set; }
        public string Name { get; set; }
    }

    #region Tasks
    public class TaskGenerate_StatusCountResult
    {
        public Byte TaskTypeID { get; set; }
        public Byte TaskStatusID { get; set; }
        public Int32 Count { get; set; }
    }

    public class TaskGenerate_Perf_New_Count_Result
    {
        public Byte TaskTypeID { get; set; }
        public Int32 LastSevenDays { get; set; }
        public Decimal SixMonthAvg { get; set; }
    }

    public class TaskGenerate_Perf_Completed_Count_Result
    {
        public Byte TaskTypeID { get; set; }
        public Int32 LastSevenDays { get; set; }
        public Decimal SixMonthAvg { get; set; }
    }

    public class TaskGenerate_TaskTypeResult
    {
        public Byte TaskTypeID { get; set; }
        public String Name { get; set; }
        public Byte SortOrder { get; set; }
    }

    public class TaskGenerate_PriorityResult
    {
        public Byte PriorityID { get; set; }
        public String Name { get; set; }
    }

    public class TaskGenerate_TaskStatusResult
    {
        public Byte TaskStatusID { get; set; }
        public String Name { get; set; }
    }

    public class TaskGenerate_TaskResult
    {
        public Int16 TaskID { get; set; }
        public Int16 ParentTaskID { get; set; }
        public Int16? CategoryID { get; set; }
        public Int16? ProjectID { get; set; }
        public Int16 SortOrder { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Int16 EstimateHours { get; set; }
        public DateTime CreatedDT { get; set; }
        public DateTime? StartedDT { get; set; }
        public DateTime? CompletedDT { get; set; }
        public DateTime? ScheduledStart { get; set; }
        public DateTime? Deadline { get; set; }
        public Byte TaskTypeID { get; set; }
        public Byte PriorityID { get; set; }
        public Byte TaskStatusID { get; set; }
        public Decimal ROI { get; set; }
    }

    public class TaskGenerate_TaskNoteResult
    {
        public Int32 TaskNoteID { get; set; }
        public Int16 TaskID { get; set; }
        public string Note { get; set; }
    }

    public class TaskRender_TaskResult
    {
        public Int16 TaskID { get; set; }
        public Int16 ParentTaskID { get; set; }
        public Int16 SortOrder { get; set; }
        public Byte TaskTypeID { get; set; }
        public Byte SiteID { get; set; }
        public Byte PriorityID { get; set; }
        public Int16? CategoryID { get; set; }
        public DateTime CreatedDT { get; set; }
        public DateTime? StartedDT { get; set; }
        public DateTime? CompletedDT { get; set; }
        public DateTime? DeletedDT { get; set; }
        public DateTime? ScheduledStart { get; set; }
        public DateTime? Deadline { get; set; }
        public Int16? EstimateHours { get; set; }
        public String Name { get; set; }
        public Byte TaskStatusID { get; set; }
    }

    public class TaskSearch_HighlightResult
    {
        public Int16 TaskID { get; set; }
    }

    #endregion

    public class PageSection_ProductPage_Select_CoreResult
    {
        public long PageSectionID { get; set; }
        public int ProductPageID { get; set; }
        public short SortOrder { get; set; }
        public string Name { get; set; }
        public byte ProductTypeID { get; set; }
        public long? ImageID { get; set; }
        public byte WebStatusID { get; set; }
        public int? SalesRank { get; set; }
        public decimal? ProductRating { get; set; }
        public short? ProductRatingCount { get; set; }
        public byte? ColourID { get; set; }
        public short? LaunchID { get; set; }
        public bool Clearance { get; set; }
        public DateTime? DeletedDT { get; set; } 
    }

    public class PageSection_ProductPage_Select_RelatedProductPageResult
    {
        public int ProductPageID { get; set; }
        public int RelatedProductPageCount { get; set; }
    }

    public class PageSection_ProductPage_Select_SecondaryProductResult
    {
        public int ProductPageID { get; set; }
        public int SecondaryProductCount { get; set; }
    }

    public class PageSection_ProductPage_Select_PriceStockResult
    {
        public int ProductPageID { get; set; }
        public decimal? PriceIncMin { get; set; }
        public decimal? PriceIncMax { get; set; }
        public decimal? SalePriceIncMin { get; set; }
        public decimal? SalePriceIncMax { get; set; }
        public int PrimaryProductCount { get; set; }
        public decimal TotalStock { get; set; }
        public short LaunchID { get; set; }
    }

    public class PageSection_ProductPage_Select_ColourResult
    {
        public byte ColourID { get; set; }
        public string Name { get; set; }
    }

    public class PageSection_ProductPage_Select_LaunchResult
    {
        public short LaunchID { get; set; }
        public DateTime LaunchDate { get; set; }
    }

    public class PageSection_ProductPage_Select_StatusResult
    {
        public byte WebStatusID { get; set; }
        public string Name { get; set; }
    }


    public class ModuleOptionSortDisplay
    {
        public Int32 ModuleOptionID { get; set; }
        public Int16 ModuleID { get; set; }
        public Int16? ModuleOptionGroupID { get; set; }
        public String Name { get; set; }
        public String Code { get; set; }
        public Int32 ProductPageID { get; set; }
        public Int64 ModuleOptionImageID { get; set; }
        public Int16 SortOrder { get; set; }
    }

    public class ProductPage_Generate_Select_CoreResult
    {
        public Int32 MerchandiserID { get; set; }
        public Int32 ProductCount { get; set; }
        public String Heading { get; set; }
        public Byte BrandID { get; set; }
        public Int32 ImageCount { get; set; }
        public Byte ProductTypeID { get; set; }
        public Byte? NamingConventionID { get; set; }
    }

    public class ProductPage_Generate_Select_ProductResult
    {
        public Int32 MerchandiserID { get; set; }
        public String ProductCode { get; set; }
        public String BasketName { get; set; }
        public String Design { get; set; }
        public String Colourway { get; set; }
        public String ProductCategory { get; set; }
        public Byte BrandID { get; set; }
        public Byte ProductTypeID { get; set; }
        public Int16 LaunchID { get; set; }
        public Byte? NamingConventionID { get; set; }
    }

    public class Product_ReasonNotImportedResult
    {
        public String Code { get; set; }
        public String Name { get; set; }
        public String ProductStatus { get; set; }
        public String ProductGroupCode { get; set; }
        public String ItemTypeCode { get; set; }
        public String BasketName { get; set; }
        public DateTime LaunchDate { get; set; }
        public Int16 LaunchID { get; set; }
        public Int32? ProductID { get; set; }
        public Int32? ProductPageID { get; set; }
        public Int16? ProductCategoryID { get; set; }
        public Int32? MerchandiserID { get; set; }
        public Int32? CorrespondingMerchandiserID { get; set; }
        public Byte? NamingConventionID { get; set; }
        public Byte? ProductTypeID { get; set; }
        public Byte? TemplateID { get; set; }
        public String Status { get; set; }
        public String Errors { get; set; }
    }

    public partial class FlockDataContext : System.Data.Linq.DataContext
    {
        partial void OnCreated()
        {
            this.CommandTimeout = 3600;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.ModuleOption_SortOrder_Select")]
        [ResultType(typeof(ModuleOptionGroup))]
        [ResultType(typeof(ModuleOptionSortDisplay))]
        public IMultipleResults ModuleOptio3n_SortOrder_Select(Int16 ModuleID)
        {
            IExecuteResult result = this.ExecuteMethodCall(
                this, (MethodInfo)MethodInfo.GetCurrentMethod(),
                ModuleID);

            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.ProductPage_Generate_ErrorInspector")]
        [ResultType(typeof(Product_ReasonNotImportedResult))]
        public IMultipleResults ProductPage_Generate_ErrorInspector_GenerateMultiple(Int16? LaunchID)
        {
            IExecuteResult result = this.ExecuteMethodCall(
                this, (MethodInfo)MethodInfo.GetCurrentMethod(), LaunchID);

            return (IMultipleResults)result.ReturnValue;
        }
        
        public Flock.ViewModels.TaskSearchViewModel Task_Search(
            String searchTerm = null,
            String status = null,
            Byte? taskTypeID = null,
            Byte? siteID = null)
        {
            var Model = new Flock.ViewModels.TaskSearchViewModel();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ToString()))
            {
                DataTable TaskStatusIDs = new DataTable();
                TaskStatusIDs.Columns.Add("StatusID", typeof(Byte));

                status = status == null ? "1,2" : status;

                foreach (var t in status.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    TaskStatusIDs.Rows.Add(new object[] { Convert.ToByte(t) });
                }

                SqlCommand cmd = new SqlCommand("tsk.Task_Search", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
                cmd.Parameters.AddWithValue("@TaskTypeID", taskTypeID);
                cmd.Parameters.AddWithValue("@SiteID", siteID);

                SqlParameter oT = cmd.Parameters.AddWithValue("@StatusIDs", TaskStatusIDs);
                oT.ParameterName = "@StatusIDs";
                oT.Value = TaskStatusIDs;
                oT.SqlDbType = SqlDbType.Structured;
                oT.TypeName = "dbo.StatusIDsTableType";

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Model.TaskHighlights = new List<TaskSearch_HighlightResult>();
                Model.Tasks = new List<TaskRender_TaskResult>();

                while (reader.Read())
                {
                    Model.TaskHighlights.Add(new TaskSearch_HighlightResult
                    {
                        TaskID = reader.GetInt16(0)
                    });
                }

                reader.NextResult();

                while (reader.Read())
                {
                    Model.Tasks.Add(new TaskRender_TaskResult
                    {
                        TaskID = reader.GetInt16(0),
                        ParentTaskID = reader.GetInt16(1),
                        SortOrder = reader.GetInt16(2),
                        TaskTypeID = reader.GetByte(3),
                        SiteID = reader.GetByte(4),
                        PriorityID = reader.GetByte(5),
                        CategoryID = reader.IsDBNull(6) ? null : (Int16?)reader.GetInt16(6),
                        CreatedDT = reader.GetDateTime(7),
                        StartedDT = reader.IsDBNull(8) ? null : (DateTime?)reader.GetDateTime(8),
                        CompletedDT = reader.IsDBNull(9) ? null : (DateTime?)reader.GetDateTime(9),
                        DeletedDT = reader.IsDBNull(10) ? null : (DateTime?)reader.GetDateTime(10),
                        ScheduledStart = reader.IsDBNull(11) ? null : (DateTime?)reader.GetDateTime(11),
                        Deadline = reader.IsDBNull(12) ? null : (DateTime?)reader.GetDateTime(12),
                        EstimateHours = reader.IsDBNull(13) ? null : (Int16?)reader.GetInt16(13),
                        Name = reader.IsDBNull(14) ? null : reader.GetString(14),
                        TaskStatusID = reader.GetByte(15)
                    });
                }
            }

            return Model;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.ModuleOption_SortOrder_Select")]
        [ResultType(typeof(ModuleOptionGroup))]
        [ResultType(typeof(ModuleOptionSortDisplay))]
        public IMultipleResults ModuleOption_SortOrder_Select(Int16 ModuleID)
        {
            IExecuteResult result = this.ExecuteMethodCall(
                this, (MethodInfo)MethodInfo.GetCurrentMethod(),
                ModuleID);

            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.ProductPage_Generate_Select")]
        [ResultType(typeof(ProductPage_Generate_Select_CoreResult))]
        [ResultType(typeof(ProductPage_Generate_Select_ProductResult))]
        [ResultType(typeof(Brand_SelectResult))]
        [ResultType(typeof(ProductType_SelectResult))]
        [ResultType(typeof(Launch_SelectResult))]
        public IMultipleResults ProductPage_Generate_Select(Int16? LaunchID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), LaunchID);

            return (IMultipleResults)result.ReturnValue;
        }

        public Boolean ProductPage_Generate_PreflightCheck(String MerchandiserIDs, Int16? LaunchID)
        {
            Boolean returnValue = false;

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ToString()))
            {
                DataTable MerchandiserIDsTable = new DataTable();
                MerchandiserIDsTable.Columns.Add("MerchandiserID", typeof(Int32));

                foreach (var t in MerchandiserIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    MerchandiserIDsTable.Rows.Add(new object[] { Convert.ToInt32(t) });
                }

                SqlCommand cmd = new SqlCommand("adm.ProductPage_Generate_PreflightCheck", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter oT = new SqlParameter();
                oT.ParameterName = "@MerchandiserIDs";
                oT.Value = MerchandiserIDsTable;
                oT.SqlDbType = SqlDbType.Structured;
                oT.TypeName = "dbo.MerchandiserIDsTableType";

                SqlParameter r = new SqlParameter();
                r.ParameterName = "@PreflightCheckPassed";
                r.Direction = ParameterDirection.Output;
                r.SqlDbType = SqlDbType.Bit;
                r.Size = 1;

                if(LaunchID.HasValue)
                    cmd.Parameters.AddWithValue("@LaunchID", LaunchID);
                
                cmd.Parameters.Add(oT);
                cmd.Parameters.Add(r);

                conn.Open();
                cmd.ExecuteNonQuery();
                returnValue = r.Value as Boolean? ?? default(Boolean);
                conn.Close();

            }

            return returnValue;
        }
        
        public Boolean ProductPage_Generate(String MerchandiserIDs, DateTime? OnDate)
        {
            Boolean returnValue = false;

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ToString()))
            {
                DataTable MerchandiserIDsTable = new DataTable();
                MerchandiserIDsTable.Columns.Add("MerchandiserID", typeof(Int32));

                foreach (var t in MerchandiserIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    MerchandiserIDsTable.Rows.Add(new object[] { Convert.ToInt32(t) });
                }

                SqlCommand cmd = new SqlCommand("adm.ProductPage_Generate", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                SqlParameter oT = new SqlParameter();
                oT.ParameterName = "@MerchandiserIDs";
                oT.Value = MerchandiserIDsTable;
                oT.SqlDbType = SqlDbType.Structured;
                oT.TypeName = "dbo.MerchandiserIDsTableType";

                SqlParameter r = new SqlParameter();
                r.ParameterName = "@Result";
                r.Direction = ParameterDirection.Output;
                r.SqlDbType = SqlDbType.Bit;
                r.Size = 1;

                if (OnDate.HasValue)
                    cmd.Parameters.Add("@OnDate", SqlDbType.DateTime).Value = OnDate;
                else
                    cmd.Parameters.Add("@OnDate", SqlDbType.DateTime).Value = DBNull.Value;

                cmd.Parameters.Add(oT);
                cmd.Parameters.Add(r);

                conn.Open();
                cmd.ExecuteNonQuery();
                returnValue = r.Value as Boolean? ?? default(Boolean);
                conn.Close();

            }

            return returnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.PageSection_ProductPage_Select")]
        [ResultType(typeof(PageSection_ProductPage_Select_CoreResult))]
        [ResultType(typeof(PageSection_ProductPage_Select_RelatedProductPageResult))]
        [ResultType(typeof(PageSection_ProductPage_Select_SecondaryProductResult))]
        [ResultType(typeof(PageSection_ProductPage_Select_PriceStockResult))]
        [ResultType(typeof(PageSection_ProductPage_Select_ColourResult))]
        [ResultType(typeof(PageSection_ProductPage_Select_LaunchResult))]
        [ResultType(typeof(PageSection_ProductPage_Select_StatusResult))]
        public IMultipleResults PageSection_ProductPage_Select(long PageSectionID, byte? PriceListID, short? CountryID)
        {
            IExecuteResult result = this.ExecuteMethodCall(
                this, (MethodInfo)MethodInfo.GetCurrentMethod(),
                PageSectionID, 
                PriceListID,
                CountryID);

            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "tsk.Task_Generate")]
        [ResultType(typeof(TaskGenerate_StatusCountResult))]
        [ResultType(typeof(TaskGenerate_Perf_New_Count_Result))]
        [ResultType(typeof(TaskGenerate_Perf_Completed_Count_Result))]
        [ResultType(typeof(TaskRender_TaskResult))]
        [ResultType(typeof(TaskGenerate_TaskNoteResult))]
        [ResultType(typeof(TaskGenerate_TaskTypeResult))]
        [ResultType(typeof(TaskGenerate_PriorityResult))]
        [ResultType(typeof(TaskGenerate_TaskStatusResult))]
        public IMultipleResults Task_Generate(
            String searchTerm = null, 
            Boolean active = true, 
            Boolean queued = true, 
            Boolean closed = false)
        {
            IExecuteResult result = this.ExecuteMethodCall(
                this, (MethodInfo)MethodInfo.GetCurrentMethod(), 
                searchTerm, 
                active, 
                queued, 
                closed);
            return (IMultipleResults)result.ReturnValue;
        }
        
        public string ProductPage_SearchSortOrder_Update(List<ViewModels.ProductPageSortOrder> ProductPages)
        {
            DataSet ds = new DataSet();

            DataTable ProductPageIDsTable = new DataTable();
            ProductPageIDsTable.Columns.Add("ProductPageID", typeof(int));
            ProductPageIDsTable.Columns.Add("SortOrder", typeof(int));
            
            foreach (var t in ProductPages)
            {
                ProductPageIDsTable.Rows.Add(t.ProductPageID, t.SortOrder);
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.ProductPage_SearchSortOrder_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter oT = cmd.Parameters.AddWithValue("@ProductPageIDs", ProductPageIDsTable);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "dbo.ProductPageSortOrderTableType";
            
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string Task_Hierarchy_Update(
            Flock.ViewModels.TaskShuffleViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable Tasks = new DataTable();
            Tasks.Columns.Add("TaskID", typeof(Int16));
            Tasks.Columns.Add("ParentTaskID", typeof(Int16));
            Tasks.Columns.Add("SortOrder", typeof(Int16));

            foreach (var t in model.Tasks)
            {
                Tasks.Rows.Add(new object[] { t.item_id, t.parent_id == null ? "0" : t.parent_id, t.sortOrder });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("tsk.Task_Hierarchy_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter oT = cmd.Parameters.AddWithValue("@Tasks", Tasks);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "tsk.TaskShuffleTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public bool ProductPage_Country_BulkUpdate(List<Int32> ProductPageIDs, List<Int16> CountryIDs)
        {
            DataSet ds = new DataSet();

            DataTable ProductPages = new DataTable();
            ProductPages.Columns.Add("ProductPageID", typeof(Int32));

            foreach (var t in ProductPageIDs)
            {
                ProductPages.Rows.Add(t);
            }

            DataTable Countries = new DataTable();
            Countries.Columns.Add("CountryID", typeof(Int16));

            foreach (var t in CountryIDs)
            {
                Countries.Rows.Add(t);
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.ProductPage_Country_BulkUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter oT = cmd.Parameters.AddWithValue("@ProductPageIDs", ProductPages);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "dbo.ProductPageIDsTableType";

            SqlParameter oL = cmd.Parameters.AddWithValue("@CountryIDs", Countries);
            oL.SqlDbType = SqlDbType.Structured;
            oL.TypeName = "dbo.CountryIDsTableType";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return true;
        }

        public string PageSection_ProductPage_Merge(
           int TargetPageSectionID, int SourcePageSectionID, string KeyString, Boolean Replace)
        {
            DataSet ds = new DataSet();

            DataTable ProductPages = new DataTable();
            ProductPages.Columns.Add("ProductPageID", typeof(int));

            if(KeyString != null && KeyString != "")
            {
                foreach (var t in KeyString.Split(new char[] {','}))
                {
                    ProductPages.Rows.Add(Convert.ToInt32(t));
                }
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.PageSection_ProductPage_Merge", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SourcePageSectionID", SourcePageSectionID);

            cmd.Parameters.AddWithValue("@TargetPageSectionID", TargetPageSectionID);

            cmd.Parameters.AddWithValue("@Replace", Replace);

            SqlParameter oT = cmd.Parameters.AddWithValue("@ProductPages", ProductPages);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "dbo.ProductPageIDsTableType";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.TechInfo_AdditionalInfo_Language_SelectList")]
        [ResultType(typeof(TechInfo_AdditionalInfo_Language_SelectList_AdditionalInfoResult))]
        public IMultipleResults TechInfo_AdditionalInfo_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.TechInfo_Aftercare_Language_SelectList")]
        [ResultType(typeof(TechInfo_Aftercare_Language_SelectList_AftercareResult))]
        public IMultipleResults TechInfo_Aftercare_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.TechInfo_Composition_Language_SelectList")]
        [ResultType(typeof(TechInfo_Composition_Language_SelectList_TechInfo_CompositionResult))]
        public IMultipleResults TechInfo_Composition_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.TechInfo_ContractFR_Language_SelectList")]
        [ResultType(typeof(TechInfo_ContractFR_Language_SelectList_TechInfo_ContractFRResult))]
        public IMultipleResults TechInfo_ContractFR_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.TechInfo_DomesticFR_Language_SelectList")]
        [ResultType(typeof(TechInfo_DomesticFR_Language_SelectList_TechInfo_DomesticFRResult))]
        public IMultipleResults TechInfo_DomesticFR_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.TechInfo_Flamcode_Language_SelectList")]
        [ResultType(typeof(TechInfo_Flamcode_Language_SelectList_TechInfo_FlamcodeResult))]
        public IMultipleResults TechInfo_Flamcode_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.TechInfo_PatternMatch_Language_SelectList")]
        [ResultType(typeof(TechInfo_PatternMatch_Language_SelectList_TechInfo_PatternMatchResult))]
        public IMultipleResults TechInfo_PatternMatch_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.TechInfo_Test_Language_SelectList")]
        [ResultType(typeof(TechInfo_Test_Language_SelectList_TechInfo_TestResult))]
        public IMultipleResults TechInfo_Test_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.TechInfo_Type_Language_SelectList")]
        [ResultType(typeof(TechInfo_Type_Language_SelectList_TechInfo_TypeResult))]
        public IMultipleResults TechInfo_Type_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.TechInfo_Usage_Language_SelectList")]
        [ResultType(typeof(TechInfo_Usage_Language_SelectList_TechInfo_UsageResult))]
        public IMultipleResults TechInfo_Usage_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }




        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Menu_Select")]
        [ResultType(typeof(MenuBuilder_MenuSelectResult))]
        [ResultType(typeof(MenuBuilder_MenuCountrySelectResult))]
        public IMultipleResults Menu_Select()
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod());
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.PageSection_Language_SelectList")]
        [ResultType(typeof(PageSection_Language_SelectList_PageSectionResult))]
        public IMultipleResults PageSection_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Image_Language_SelectList")]
        [ResultType(typeof(Image_Language_SelectList_ImageResult))]
        public IMultipleResults Image_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Finish_Language_SelectList")]
        [ResultType(typeof(Finish_Language_SelectList_FinishResult))]
        public IMultipleResults Finish_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Resource_Language_SelectList")]
        [ResultType(typeof(Resource_Language_SelectList_ResourceResult))]
        public IMultipleResults Resource_Language_SelectList(short countryLanguageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), countryLanguageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "ads.Announcement_Language_SelectList")]
        [ResultType(typeof(Announcement_Language_SelectList_AnnouncementResult))]
        public IMultipleResults Announcement_Language_SelectList(byte LanguageID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), LanguageID);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Aftercare_Language_SelectList")]
        [ResultType(typeof(Aftercare_Language_SelectList_AftercareResult))]
        public IMultipleResults Aftercare_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.TechnicalDescription_Language_SelectList")]
        [ResultType(typeof(TechnicalDescription_Language_SelectList_TechnicalDescriptionResult))]
        public IMultipleResults TechnicalDescription_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Usage_Language_SelectList")]
        [ResultType(typeof(Usage_Language_SelectList_UsageResult))]
        public IMultipleResults Usage_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Pattern_Language_SelectList")]
        [ResultType(typeof(Pattern_Language_SelectList_PatternResult))]
        public IMultipleResults Pattern_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Colour_Language_SelectList")]
        [ResultType(typeof(Colour_Language_SelectList_ColourResult))]
        public IMultipleResults Colour_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Form_Language_SelectList")]
        [ResultType(typeof(Form_Language_SelectList_FormResult))]
        public IMultipleResults Form_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Product_Language_SelectList")]
        [ResultType(typeof(Product_Language_SelectList_ProductResult))]
        public IMultipleResults Product_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.ProductType_Language_SelectList")]
        [ResultType(typeof(ProductType_Language_SelectList_ProductTypeResult))]
        public IMultipleResults ProductType_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.AttributeGroup_Language_SelectList")]
        [ResultType(typeof(AttributeGroup_Language_SelectList_AttributeGroupResult))]
        public IMultipleResults AttributeGroup_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Attribute_Language_SelectList")]
        [ResultType(typeof(Attribute_Language_SelectList_AttributeResult))]
        public IMultipleResults Attribute_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.EmailText_Language_SelectList")]
        [ResultType(typeof(EmailText_Language_SelectList_EmailTextResult))]
        public IMultipleResults EmailText_Language_SelectList(byte LanguageID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), LanguageID);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.SiteSection_Language_SelectList")]
        [ResultType(typeof(SiteSection_Language_SelectList_SiteSectionResult))]
        public IMultipleResults SiteSection_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.SlideShow_Image_Language_SelectList")]
        [ResultType(typeof(SlideShowImage_Language_SelectList_SlideShowImageResult))]
        public IMultipleResults SlideShowImage_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }


        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.OrderStatus_Language_SelectList")]
        [ResultType(typeof(OrderStatus_Language_SelectList_OrderStatusResult))]
        public IMultipleResults OrderStatus_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.StockStatus_Language_SelectList")]
        [ResultType(typeof(StockStatus_Language_SelectList_StockStatusResult))]
        public IMultipleResults StockStatus_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Menu_Language_SelectList")]
        [ResultType(typeof(Menu_Language_SelectList_MenuResult))]
        public IMultipleResults Menu_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.MenuItem_Language_SelectList")]
        [ResultType(typeof(MenuItem_Language_SelectList_MenuItemResult))]
        public IMultipleResults MenuItem_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.CallToAction_Language_SelectList")]
        [ResultType(typeof(CallToAction_Language_SelectList_CallToActionResult))]
        public IMultipleResults CallToAction_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.OrderFormGroup_Language_SelectList")]
        [ResultType(typeof(OrderFormGroup_Language_SelectList_OrderFormGroupResult))]
        public IMultipleResults OrderFormGroup_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.ArticleType_Language_SelectList")]
        [ResultType(typeof(ArticleType_Language_SelectList_ArticleTypeResult))]
        public IMultipleResults ArticleType_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.ArticleSubType_Language_SelectList")]
        [ResultType(typeof(ArticleSubType_Language_SelectList_ArticleSubTypeResult))]
        public IMultipleResults ArticleSubType_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }



        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Module_Language_SelectList")]
        [ResultType(typeof(Module_Language_SelectList_ModuleResult))]
        public IMultipleResults Module_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.ModuleOption_Language_SelectList")]
        [ResultType(typeof(ModuleOption_Language_SelectList_ModuleOptionResult))]
        public IMultipleResults ModuleOption_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.ModuleOptionGroup_Language_SelectList")]
        [ResultType(typeof(ModuleOptionGroup_Language_SelectList_ModuleOptionGroupResult))]
        public IMultipleResults ModuleOptionGroup_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }


        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Room_Language_SelectList")]
        [ResultType(typeof(Room_Language_SelectList_RoomResult))]
        public IMultipleResults Room_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }


        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.ReturnReason_Language_SelectList")]
        [ResultType(typeof(ReturnReason_Language_SelectList_ReturnReasonResult))]
        public IMultipleResults ReturnReason_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.DeliveryOption_Language_SelectList")]
        [ResultType(typeof(DeliveryOption_Language_SelectList_DeliveryOptionResult))]
        public IMultipleResults DeliveryOption_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.GiftVoucherValidation_Language_SelectList")]
        [ResultType(typeof(GiftVoucherValidation_Language_SelectList_GiftVoucherValidationResult))]
        public IMultipleResults GiftVoucherValidation_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.HtmlSection_Language_SelectList")]
        [ResultType(typeof(HtmlSection_Language_SelectList_HtmlSectionResult))]
        public IMultipleResults HtmlSection_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.HtmlSection_Language_SelectDetail")]
        [ResultType(typeof(HtmlSection_Language_SelectDetail_HtmlSectionResult))]
        [ResultType(typeof(HtmlSection_Language_SelectDetail_TranslationResult))]
        [ResultType(typeof(HtmlSection_Language_SelectDetail_ReviewRequiredResult))]
        public IMultipleResults HtmlSection_Language_SelectDetail(long HtmlSectionID, byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), HtmlSectionID, languageId);
            return (IMultipleResults)result.ReturnValue;
        }


        public string HtmlSection_Language_Save(
            byte languageId,
            int HtmlSectionID,
            string OriginalContents,
            string TranslatedContents)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("HTMLSectionID", typeof(int));
            OriginalText.Columns.Add("Contents", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("HTMLSectionID", typeof(int));
            TranslatedText.Columns.Add("Contents", typeof(string));

                OriginalText.Rows.Add(new object[] { HtmlSectionID, OriginalContents });

                TranslatedText.Rows.Add(new object[] { HtmlSectionID, TranslatedContents });
            
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.HtmlSection_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "HtmlSectionTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "HtmlSectionTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }



        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Snippet_Language_SelectList")]
        [ResultType(typeof(Snippet_Language_SelectList_SnippetResult))]
        public IMultipleResults Snippet_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }


        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Snippet_Language_SelectDetail")]
        [ResultType(typeof(Snippet_Language_SelectDetail_SnippetResult))]
        [ResultType(typeof(Snippet_Language_SelectDetail_TranslationResult))]
        [ResultType(typeof(Snippet_Language_SelectDetail_ReviewRequiredResult))]
        public IMultipleResults Snippet_Language_SelectDetail(short SnippetID, byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), SnippetID, languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        public string Snippet_Language_Save(
            byte languageId,
            short SnippetID,
            string OriginalContents,
            string TranslatedContents)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("SnippetID", typeof(int));
            OriginalText.Columns.Add("Contents", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("SnippetID", typeof(int));
            TranslatedText.Columns.Add("Contents", typeof(string));

            OriginalText.Rows.Add(new object[] { SnippetID, OriginalContents });

            TranslatedText.Rows.Add(new object[] { SnippetID, TranslatedContents });

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Snippet_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "SnippetTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "SnippetTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string Image_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateImageGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ImageID", typeof(long));
            OriginalText.Columns.Add("TitleText", typeof(string));
            OriginalText.Columns.Add("AltText", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ImageID", typeof(long));
            TranslatedText.Columns.Add("TitleText", typeof(string));
            TranslatedText.Columns.Add("AltText", typeof(string));

            foreach (var pt in model.Images)
            {
                OriginalText.Rows.Add(new object[] { pt.ImageID, pt.TitleText, pt.AltText });

                TranslatedText.Rows.Add(new object[] { pt.ImageID, pt.TranslatedTitleText, pt.TranslatedAltText });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Image_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "ImageTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "ImageTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string PageSection_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslatePageSectionGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("PageSectionID", typeof(long));
            OriginalText.Columns.Add("Heading", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("PageSectionID", typeof(long));
            TranslatedText.Columns.Add("Heading", typeof(string));

            foreach (var pt in model.PageSections)
            {
                OriginalText.Rows.Add(new object[] { pt.PageSectionID, pt.Heading });

                TranslatedText.Rows.Add(new object[] { pt.PageSectionID, pt.TranslatedHeading });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.PageSection_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "PageSectionTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "PageSectionTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }
        public string EmailText_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateEmailTextGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("EmailTextID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("EmailTextID", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var pt in model.EmailTexts)
            {
                OriginalText.Rows.Add(new object[] { pt.EmailTextID, pt.Text });

                TranslatedText.Rows.Add(new object[] { pt.EmailTextID, pt.TranslatedText });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.EmailText_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "EmailTextTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "EmailTextTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string Finish_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateFinishGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("FinishID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("FinishID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var pt in model.Finishes)
            {
                OriginalText.Rows.Add(new object[] { pt.FinishID, pt.Name });

                TranslatedText.Rows.Add(new object[] { pt.FinishID, pt.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Finish_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "FinishTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "FinishTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string Aftercare_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateAftercareGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("AftercareID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("AftercareID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var pt in model.Aftercares)
            {
                OriginalText.Rows.Add(new object[] { pt.AftercareID, pt.Name });

                TranslatedText.Rows.Add(new object[] { pt.AftercareID, pt.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Aftercare_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "AftercareTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "AftercareTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string TechnicalDescription_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateTechnicalDescriptionGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("TechnicalDescriptionID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("TechnicalDescriptionID", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var pt in model.TechnicalDescriptions)
            {
                OriginalText.Rows.Add(new object[] { pt.TechnicalDescriptionID, pt.Name });

                TranslatedText.Rows.Add(new object[] { pt.TechnicalDescriptionID, pt.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.TechnicalDescription_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "TechnicalDescriptionTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "TechnicalDescriptionTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string Usage_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateUsageGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("UsageID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("UsageID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var pt in model.Usages)
            {
                OriginalText.Rows.Add(new object[] { pt.UsageID, pt.Name });

                TranslatedText.Rows.Add(new object[] { pt.UsageID, pt.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Usage_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "UsageTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "UsageTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string Pattern_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslatePatternGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("PatternID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("PatternID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var pt in model.Patterns)
            {
                OriginalText.Rows.Add(new object[] { pt.PatternID, pt.Name });

                TranslatedText.Rows.Add(new object[] { pt.PatternID, pt.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Pattern_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "PatternTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "PatternTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string Colour_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateColourGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ColourID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ColourID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var pt in model.Colours)
            {
                OriginalText.Rows.Add(new object[] { pt.ColourID, pt.Name });

                TranslatedText.Rows.Add(new object[] { pt.ColourID, pt.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Colour_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "ColourTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "ColourTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string Form_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateFormGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("FormID", typeof(int));
            OriginalText.Columns.Add("Heading", typeof(string));
            OriginalText.Columns.Add("SubHeading", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("FormID", typeof(int));
            TranslatedText.Columns.Add("Heading", typeof(string));
            TranslatedText.Columns.Add("SubHeading", typeof(string));

            foreach (var pt in model.Forms)
            {
                OriginalText.Rows.Add(new object[] { pt.FormID, pt.Heading, pt.SubHeading, });

                TranslatedText.Rows.Add(new object[] { pt.FormID, pt.TranslatedHeading, pt.TranslatedSubHeading });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Form_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "FormTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "FormTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string Product_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateProductGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ProductID", typeof(int));
            OriginalText.Columns.Add("OrderFormName", typeof(string));
            OriginalText.Columns.Add("BasketName", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ProductID", typeof(int));
            TranslatedText.Columns.Add("OrderFormName", typeof(string));
            TranslatedText.Columns.Add("BasketName", typeof(string));

            foreach (var pt in model.Products)
            {
                OriginalText.Rows.Add(new object[] { pt.ProductID, pt.OrderFormName, pt.BasketName });

                TranslatedText.Rows.Add(new object[] { pt.ProductID, pt.TranslatedOrderFormName, pt.TranslatedBasketName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Product_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "ProductTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "ProductTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string ProductType_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateProductTypeGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ProductTypeID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ProductTypeID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            DataTable OriginalNamePlural = new DataTable();
            OriginalNamePlural.Columns.Add("ProductTypeID", typeof(byte));
            OriginalNamePlural.Columns.Add("Name", typeof(string));

            DataTable TranslatedNamePlural = new DataTable();
            TranslatedNamePlural.Columns.Add("ProductTypeID", typeof(byte));
            TranslatedNamePlural.Columns.Add("Name", typeof(string));

            DataTable OriginalURLKeywords = new DataTable();
            OriginalURLKeywords.Columns.Add("ProductTypeID", typeof(byte));
            OriginalURLKeywords.Columns.Add("Name", typeof(string));

            DataTable TranslatedURLKeywords = new DataTable();
            TranslatedURLKeywords.Columns.Add("ProductTypeID", typeof(byte));
            TranslatedURLKeywords.Columns.Add("Name", typeof(string));

            foreach (var pt in model.ProductTypes)
            {
                OriginalText.Rows.Add(new object[] { pt.ProductTypeID, pt.Name });

                TranslatedText.Rows.Add(new object[] { pt.ProductTypeID, pt.TranslatedName });

                OriginalNamePlural.Rows.Add(new object[] { pt.ProductTypeID, pt.NamePlural });

                TranslatedNamePlural.Rows.Add(new object[] { pt.ProductTypeID, pt.TranslatedNamePlural });

                OriginalURLKeywords.Rows.Add(new object[] { pt.ProductTypeID, pt.URLKeywords });

                TranslatedURLKeywords.Rows.Add(new object[] { pt.ProductTypeID, pt.TranslatedURLKeywords });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.ProductType_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "ProductTypeTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "ProductTypeTextTableType";
            SqlParameter oP = cmd.Parameters.AddWithValue("@OriginalPluralText", OriginalNamePlural);
            oP.SqlDbType = SqlDbType.Structured;
            oP.TypeName = "ProductTypeTextTableType";
            SqlParameter tP = cmd.Parameters.AddWithValue("@TranslatedPluralText", TranslatedNamePlural);
            tP.SqlDbType = SqlDbType.Structured;
            tP.TypeName = "ProductTypeTextTableType";
            SqlParameter oU = cmd.Parameters.AddWithValue("@OriginalURLKeywordsText", OriginalURLKeywords);
            oP.SqlDbType = SqlDbType.Structured;
            oP.TypeName = "ProductTypeTextTableType";
            SqlParameter tU = cmd.Parameters.AddWithValue("@TranslatedURLKeywordsText", TranslatedURLKeywords);
            tP.SqlDbType = SqlDbType.Structured;
            tP.TypeName = "ProductTypeTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string Announcement_Language_Save(
            byte languageId,
            TranslateAnnouncementGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("AnnouncementID", typeof(short));
            OriginalText.Columns.Add("Text", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("AnnouncementID", typeof(short));
            TranslatedText.Columns.Add("Text", typeof(string));

            foreach (var r in model.Announcements)
            {
                OriginalText.Rows.Add(new object[] { r.AnnouncementID, r.Text });

                TranslatedText.Rows.Add(new object[] { r.AnnouncementID, r.TranslatedText });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("ads.Announcement_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "AnnouncementTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "AnnouncementTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string Resource_Language_Save(
            short countryLanguageId,
            Flock.ViewModels.TranslateResourceGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ResourceID", typeof(short));
            OriginalText.Columns.Add("CountryID", typeof(short));
            OriginalText.Columns.Add("Value", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ResourceID", typeof(short));
            TranslatedText.Columns.Add("CountryID", typeof(short));
            TranslatedText.Columns.Add("Value", typeof(string));

            foreach (var r in model.Resources)
            {
                OriginalText.Rows.Add(new object[] { r.ResourceID, r.CountryID, r.Value });

                TranslatedText.Rows.Add(new object[] { r.ResourceID, r.CountryID, r.TranslatedValue });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Resource_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CountryLanguageID", countryLanguageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "ResourceTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "ResourceTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string AttributeGroup_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateAttributeGroupGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("AttributeGroupID", typeof(byte));
            OriginalText.Columns.Add("Heading", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("AttributeGroupID", typeof(byte));
            TranslatedText.Columns.Add("Heading", typeof(string));

            foreach (var ag in model.AttributeGroups)
            {
                OriginalText.Rows.Add(new object[] { ag.AttributeGroupID, ag.Heading });

                TranslatedText.Rows.Add(new object[] { ag.AttributeGroupID, ag.TranslatedHeading });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.AttributeGroup_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "AttributeGroupTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "AttributeGroupTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string Attribute_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateAttributeGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("AttributeID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("AttributeID", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var a in model.Attributes)
            {
                OriginalText.Rows.Add(new object[] { a.AttributeID, a.Name });

                TranslatedText.Rows.Add(new object[] { a.AttributeID, a.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Attribute_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "AttributeTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "AttributeTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string SiteSection_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateSiteSectionGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("SiteSectionID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("SiteSectionID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var o in model.SiteSections)
            {
                OriginalText.Rows.Add(new object[] { o.SiteSectionID, o.Name });

                TranslatedText.Rows.Add(new object[] { o.SiteSectionID, o.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.SiteSection_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "SiteSectionTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "SiteSectionTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string SlideShowImage_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateSlideShowImageGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("SlideShowID", typeof(Int64));
            OriginalText.Columns.Add("ImageID", typeof(Int64));
            OriginalText.Columns.Add("OverlayHeading", typeof(string));
            OriginalText.Columns.Add("OverlayText", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("SlideShowID", typeof(Int64));
            TranslatedText.Columns.Add("ImageID", typeof(Int64));
            TranslatedText.Columns.Add("OverlayHeading", typeof(string));
            TranslatedText.Columns.Add("OverlayText", typeof(string));

            foreach (var o in model.SlideShowImages)
            {
                OriginalText.Rows.Add(new object[] { o.SlideShowID, o.ImageID, o.OverlayHeading, o.OverlayText });

                TranslatedText.Rows.Add(new object[] { o.SlideShowID, o.ImageID, o.TranslatedOverlayHeading, o.TranslatedOverlayText });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.SlideShow_Image_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "SlideShowImageTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "SlideShowImageTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string OrderStatus_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateOrderStatusGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("OrderStatusID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("OrderStatusID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var o in model.OrderStatuses)
            {
                OriginalText.Rows.Add(new object[] { o.OrderStatusID, o.Name });

                TranslatedText.Rows.Add(new object[] { o.OrderStatusID, o.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.OrderStatus_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "OrderStatusTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "OrderStatusTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string StockStatus_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateStockStatusGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("StockStatusID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("StockStatusID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var s in model.StockStatuses)
            {
                OriginalText.Rows.Add(new object[] { s.StockStatusID, s.StockStatus });

                TranslatedText.Rows.Add(new object[] { s.StockStatusID, s.TranslatedStockStatus });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.StockStatus_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "StockStatusTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "StockStatusTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string Menu_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateMenuGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("MenuID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));
            OriginalText.Columns.Add("MessageHTML", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("MenuID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));
            TranslatedText.Columns.Add("MessageHTML", typeof(string));

            foreach (var m in model.Menus)
            {
                OriginalText.Rows.Add(new object[] { m.MenuID, m.Name, m.MessageHTML });

                TranslatedText.Rows.Add(new object[] { m.MenuID, m.TranslatedName, m.TranslatedMessageHTML });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Menu_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "MenuTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "MenuTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string MenuItem_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateMenuItemGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("MenuItemID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));
            OriginalText.Columns.Add("ExternalURL", typeof(string));
            OriginalText.Columns.Add("QueryStringParameters", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("MenuItemID", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));
            TranslatedText.Columns.Add("ExternalURL", typeof(string));
            TranslatedText.Columns.Add("QueryStringParameters", typeof(string));

            foreach (var mi in model.MenuItems)
            {
                OriginalText.Rows.Add(new object[] { mi.MenuItemID, mi.Name, mi.ExternalURL, mi.QueryStringParameters });

                TranslatedText.Rows.Add(new object[] { mi.MenuItemID, mi.TranslatedName, mi.TranslatedExternalURL, mi.TranslatedQueryStringParameters });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.MenuItem_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "MenuItemTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "MenuItemTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string CallToAction_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateCallToActionGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("CallToActionID", typeof(int));
            OriginalText.Columns.Add("Text", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("CallToActionID", typeof(int));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var mi in model.CallsToAction)
            {
                OriginalText.Rows.Add(new object[] { mi.CallToActionID, mi.Text });

                TranslatedText.Rows.Add(new object[] { mi.CallToActionID, mi.TranslatedText });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.CallToAction_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "CallToActionTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "CallToActionTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string OrderFormGroup_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateOrderFormGroupGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("OrderFormGroupID", typeof(short));
            OriginalText.Columns.Add("OrderFormGroup", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("OrderFormGroupID", typeof(short));
            TranslatedText.Columns.Add("OrderFormGroup", typeof(string));

            foreach (var ofg in model.OrderFormGroups)
            {
                OriginalText.Rows.Add(new object[] { ofg.OrderFormGroupID, ofg.OrderFormGroup });

                TranslatedText.Rows.Add(new object[] { ofg.OrderFormGroupID, ofg.TranslatedOrderFormGroup });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.OrderFormGroup_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "OrderFormGroupTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "OrderFormGroupTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string ArticleType_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateArticleTypeGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ArticleTypeID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ArticleTypeID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var at in model.ArticleTypes)
            {
                OriginalText.Rows.Add(new object[] { at.ArticleTypeID, at.Name });

                TranslatedText.Rows.Add(new object[] { at.ArticleTypeID, at.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.ArticleType_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "ArticleTypeTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "ArticleTypeTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string ArticleSubType_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateArticleSubTypeGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ArticleSubTypeID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ArticleSubTypeID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var ast in model.ArticleSubTypes)
            {
                OriginalText.Rows.Add(new object[] { ast.ArticleSubTypeID, ast.Name });

                TranslatedText.Rows.Add(new object[] { ast.ArticleSubTypeID, ast.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.ArticleSubType_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "ArticleSubTypeTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "ArticleSubTypeTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string Module_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateModuleGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ModuleID", typeof(Int16));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ModuleID", typeof(Int16));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var ast in model.Modules)
            {
                OriginalText.Rows.Add(new object[] { ast.ModuleID, ast.Name });

                TranslatedText.Rows.Add(new object[] { ast.ModuleID, ast.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Module_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "ModuleTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "ModuleTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string ModuleOption_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateModuleOptionGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ModuleOptionID", typeof(Int32));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ModuleOptionID", typeof(Int16));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var ast in model.ModuleOptions)
            {
                OriginalText.Rows.Add(new object[] { ast.ModuleOptionID, ast.Name });

                TranslatedText.Rows.Add(new object[] { ast.ModuleOptionID, ast.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.ModuleOption_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "ModuleOptionTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "ModuleOptionTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string ModuleOptionGroup_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateModuleOptionGroupGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ModuleOptionGroupID", typeof(Int16));
            OriginalText.Columns.Add("Name", typeof(string));
            OriginalText.Columns.Add("Description", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ModuleOptionID", typeof(Int16));
            TranslatedText.Columns.Add("Name", typeof(string));
            TranslatedText.Columns.Add("Description", typeof(string));

            foreach (var ast in model.ModuleOptionGroups)
            {
                OriginalText.Rows.Add(new object[] { ast.ModuleOptionGroupID, ast.Name, ast.Description });
                
                TranslatedText.Rows.Add(new object[] { ast.ModuleOptionGroupID, ast.TranslatedName, ast.TranslatedDescription });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.ModuleOptionGroup_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "ModuleOptionGroupTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "ModuleOptionGroupTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }



        public string Room_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateRoomGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("RoomID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("RoomID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var rr in model.Rooms)
            {
                OriginalText.Rows.Add(new object[] { rr.RoomID, rr.Name });

                TranslatedText.Rows.Add(new object[] { rr.RoomID, rr.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Room_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "RoomTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "RoomTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string ReturnReason_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateReturnReasonGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ReturnReasonID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ReturnReasonID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var rr in model.ReturnReasons)
            {
                OriginalText.Rows.Add(new object[] { rr.ReturnReasonID, rr.Name });

                TranslatedText.Rows.Add(new object[] { rr.ReturnReasonID, rr.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.ReturnReason_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "ReturnReasonTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "ReturnReasonTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string DeliveryOption_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateDeliveryOptionGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("DeliveryOptionID", typeof(byte));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("DeliveryOptionID", typeof(byte));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var dO in model.DeliveryOptions)
            {
                OriginalText.Rows.Add(new object[] { dO.DeliveryOptionID, dO.Name });

                TranslatedText.Rows.Add(new object[] { dO.DeliveryOptionID, dO.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.DeliveryOption_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "DeliveryOptionTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "DeliveryOptionTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        public string GiftVoucherValidation_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateGiftVoucherValidationGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("GiftVoucherValidationID", typeof(byte));
            OriginalText.Columns.Add("Message", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("GiftVoucherValidationID", typeof(byte));
            TranslatedText.Columns.Add("Message", typeof(string));

            foreach (var gvv in model.GiftVoucherValidations)
            {
                OriginalText.Rows.Add(new object[] { gvv.GiftVoucherValidationID, gvv.OriginalMessage });

                TranslatedText.Rows.Add(new object[] { gvv.GiftVoucherValidationID, gvv.TranslatedMessage });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.GiftVoucherValidation_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "GiftVoucherValidationTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "GiftVoucherValidationTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        /* Tech Info */

        public string TechInfo_AdditionalInfo_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateTechInfo_AdditionalInfoGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("AdditionalInfoID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("AdditionalInfo", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var gvv in model.TechInfo_AdditionalInfos)
            {
                OriginalText.Rows.Add(new object[] { gvv.AdditionalInfoID, gvv.Name });

                TranslatedText.Rows.Add(new object[] { gvv.AdditionalInfoID, gvv.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.TechInfo_AdditionalInfo_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "TechInfo_AdditionalInfoTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "TechInfo_AdditionalInfoTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string TechInfo_Aftercare_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateTechInfo_AftercareGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("AftercareID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("Aftercare", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var gvv in model.TechInfo_Aftercares)
            {
                OriginalText.Rows.Add(new object[] { gvv.AftercareID, gvv.Name });

                TranslatedText.Rows.Add(new object[] { gvv.AftercareID, gvv.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.TechInfo_Aftercare_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "TechInfo_AftercareTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "TechInfo_AftercareTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string TechInfo_Composition_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateTechInfo_CompositionGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("CompositionID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("Composition", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var gvv in model.TechInfo_Compositions)
            {
                OriginalText.Rows.Add(new object[] { gvv.CompositionID, gvv.Name });

                TranslatedText.Rows.Add(new object[] { gvv.CompositionID, gvv.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.TechInfo_Composition_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "TechInfo_CompositionTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "TechInfo_CompositionTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string TechInfo_ContractFR_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateTechInfo_ContractFRGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ContractFRID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ContractFR", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var gvv in model.TechInfo_ContractFRs)
            {
                OriginalText.Rows.Add(new object[] { gvv.ContractFRID, gvv.Name });

                TranslatedText.Rows.Add(new object[] { gvv.ContractFRID, gvv.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.TechInfo_ContractFR_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "TechInfo_ContractFRTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "TechInfo_ContractFRTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string TechInfo_DomesticFR_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateTechInfo_DomesticFRGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("DomesticFRID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("DomesticFR", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var gvv in model.TechInfo_DomesticFRs)
            {
                OriginalText.Rows.Add(new object[] { gvv.DomesticFRID, gvv.Name });

                TranslatedText.Rows.Add(new object[] { gvv.DomesticFRID, gvv.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.TechInfo_DomesticFR_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "TechInfo_DomesticFRTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "TechInfo_DomesticFRTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string TechInfo_Flamcode_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateTechInfo_FlamcodeGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("FlamcodeID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("Flamcode", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var gvv in model.TechInfo_Flamcodes)
            {
                OriginalText.Rows.Add(new object[] { gvv.FlamcodeID, gvv.Name });

                TranslatedText.Rows.Add(new object[] { gvv.FlamcodeID, gvv.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.TechInfo_Flamcode_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "TechInfo_FlamcodeTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "TechInfo_FlamcodeTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string TechInfo_PatternMatch_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateTechInfo_PatternMatchGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("PatternMatchID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("PatternMatch", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var gvv in model.TechInfo_PatternMatchs)
            {
                OriginalText.Rows.Add(new object[] { gvv.PatternMatchID, gvv.Name });

                TranslatedText.Rows.Add(new object[] { gvv.PatternMatchID, gvv.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.TechInfo_PatternMatch_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "TechInfo_PatternMatchTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "TechInfo_PatternMatchTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string TechInfo_Test_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateTechInfo_TestGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("TestID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("Test", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var gvv in model.TechInfo_Tests)
            {
                OriginalText.Rows.Add(new object[] { gvv.TestID, gvv.Name });

                TranslatedText.Rows.Add(new object[] { gvv.TestID, gvv.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.TechInfo_Test_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "TechInfo_TestTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "TechInfo_TestTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string TechInfo_Type_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateTechInfo_TypeGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("TypeID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("Type", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var gvv in model.TechInfo_Types)
            {
                OriginalText.Rows.Add(new object[] { gvv.TypeID, gvv.Name });

                TranslatedText.Rows.Add(new object[] { gvv.TypeID, gvv.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.TechInfo_Type_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "TechInfo_TypeTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "TechInfo_TypeTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        public string TechInfo_Usage_Language_Save(
            byte languageId,
            Flock.ViewModels.TranslateTechInfo_UsageGridViewModel model)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("UsageID", typeof(short));
            OriginalText.Columns.Add("Name", typeof(string));

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("Usage", typeof(short));
            TranslatedText.Columns.Add("Name", typeof(string));

            foreach (var gvv in model.TechInfo_Usages)
            {
                OriginalText.Rows.Add(new object[] { gvv.UsageID, gvv.Name });

                TranslatedText.Rows.Add(new object[] { gvv.UsageID, gvv.TranslatedName });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.TechInfo_Usage_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "TechInfo_UsageTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "TechInfo_UsageTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }


        #region Article Translation

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Article_Language_SelectList")]
        [ResultType(typeof(Article_Language_SelectList_ArticleResult))]
        [ResultType(typeof(ArticleSubType_SelectResult))]
        [ResultType(typeof(Brand_SelectResult))]
        [ResultType(typeof(Status_SelectResult))]
        public IMultipleResults Article_Language_SelectList(byte LanguageID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), LanguageID);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Article_Language_SelectDetail")]
        [ResultType(typeof(Article_Language_SelectDetail_ArticleResult))]
        [ResultType(typeof(Article_Language_SelectDetail_TranslationResult))]
        [ResultType(typeof(Article_Language_SelectDetail_ReviewRequiredResult))]
        [ResultType(typeof(Article_Language_SelectDetail_ImageResult))]
        public IMultipleResults Article_Language_SelectDetail(short ArticleID, byte LanguageID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), ArticleID, LanguageID);
            return (IMultipleResults)result.ReturnValue;
        }

        public string Article_Language_Save(
            int articleID,
            byte languageId,
            Article_Language_SelectDetail_ArticleResult originalText,
            Article_Language_SelectDetail_TranslationResult translatedText)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ArticleID", typeof(int));
            OriginalText.Columns.Add("Heading", typeof(string));
            OriginalText.Columns.Add("SubHeading", typeof(string));
            OriginalText.Columns.Add("Contents", typeof(string));
            OriginalText.Columns.Add("MetaDescription", typeof(string));
            OriginalText.Columns.Add("TitleTag", typeof(string));

            OriginalText.Rows.Add(new object[] { articleID, originalText.Heading, originalText.SubHeading, originalText.Contents, originalText.MetaDescription, originalText.TitleTag });

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ArticleID", typeof(int));
            TranslatedText.Columns.Add("Heading", typeof(string));
            TranslatedText.Columns.Add("SubHeading", typeof(string));
            TranslatedText.Columns.Add("Contents", typeof(string));
            TranslatedText.Columns.Add("MetaDescription", typeof(string));
            TranslatedText.Columns.Add("TitleTag", typeof(string));

            TranslatedText.Rows.Add(new object[] { articleID, translatedText.Heading, translatedText.SubHeading, translatedText.Contents, translatedText.MetaDescription, translatedText.TitleTag });

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Article_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "ArticleTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "ArticleTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        #endregion



        #region Collection Translation

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Collection_Language_SelectDetail")]
        [ResultType(typeof(Collection_Language_SelectDetail_CollectionResult))]
        [ResultType(typeof(Collection_Language_SelectDetail_TranslationResult))]
        [ResultType(typeof(Collection_Language_SelectDetail_ReviewRequiredResult))]
        [ResultType(typeof(Collection_Language_SelectDetail_ImageResult))]
        public IMultipleResults Collection_Language_SelectDetail(short CollectionID, byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), CollectionID, languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Collection_Language_SelectList")]
        [ResultType(typeof(Collection_Language_SelectList_CollectionResult))]
        [ResultType(typeof(Brand_SelectResult))]
        [ResultType(typeof(Launch_SelectResult))]
        [ResultType(typeof(ProductType_SelectResult))]
        [ResultType(typeof(Status_SelectResult))]
        public IMultipleResults Collection_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        public string Collection_Language_Save(
            short collectionId,
            byte languageId,
            Collection_Language_SelectDetail_CollectionResult originalText, 
            Collection_Language_SelectDetail_TranslationResult translatedText)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("CollectionID", typeof(short));
            OriginalText.Columns.Add("Heading", typeof(string));
            OriginalText.Columns.Add("SubHeading", typeof(string));
            OriginalText.Columns.Add("Description", typeof(string));
            OriginalText.Columns.Add("MetaDescription", typeof(string));
            OriginalText.Columns.Add("TitleTag", typeof(string));

            OriginalText.Rows.Add(new object[] { collectionId, originalText.Heading, originalText.SubHeading, originalText.Description, originalText.MetaDescription, originalText.TitleTag });

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("CollectionID", typeof(short));
            TranslatedText.Columns.Add("Heading", typeof(string));
            TranslatedText.Columns.Add("SubHeading", typeof(string));
            TranslatedText.Columns.Add("Description", typeof(string));
            TranslatedText.Columns.Add("MetaDescription", typeof(string));
            TranslatedText.Columns.Add("TitleTag", typeof(string));

            TranslatedText.Rows.Add(new object[] { collectionId, translatedText.Heading, translatedText.SubHeading, translatedText.Description, translatedText.MetaDescription, translatedText.TitleTag });

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Collection_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "CollectionTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "CollectionTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }

        #endregion

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.ProductPage_Language_SelectDetail")]
        [ResultType(typeof(ProductPage_Language_SelectDetail_ProductPageResult))]
        [ResultType(typeof(ProductPage_Language_SelectDetail_TranslationResult))]
        [ResultType(typeof(ProductPage_Language_SelectDetail_ReviewRequiredResult))]
        [ResultType(typeof(ProductPage_Language_SelectDetail_ImageResult))]
        public IMultipleResults ProductPage_Language_SelectDetail(int ProductPageID, byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), ProductPageID, languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.ProductPage_Language_SelectList")]
        [ResultType(typeof(ProductPage_Language_SelectList_ProductPageResult))]
        [ResultType(typeof(Brand_SelectResult))]
        [ResultType(typeof(Launch_SelectResult))]
        [ResultType(typeof(ProductType_SelectResult))]
        [ResultType(typeof(Status_SelectResult))]
        public IMultipleResults ProductPage_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        public string ProductPage_Language_Save(
            int ProductPageID,
            byte languageId,
            ProductPage_Language_SelectDetail_ProductPageResult originalText,
            ProductPage_Language_SelectDetail_TranslationResult translatedText)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("ProductPageID", typeof(int));
            OriginalText.Columns.Add("Heading", typeof(string));
            OriginalText.Columns.Add("SubHeading", typeof(string));
            OriginalText.Columns.Add("Description", typeof(string));
            OriginalText.Columns.Add("MetaDescription", typeof(string));
            OriginalText.Columns.Add("TitleTag", typeof(string));

            OriginalText.Rows.Add(new object[] { ProductPageID, originalText.Heading, originalText.SubHeading, originalText.Description, originalText.MetaDescription, originalText.TitleTag });

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("ProductPageID", typeof(int));
            TranslatedText.Columns.Add("Heading", typeof(string));
            TranslatedText.Columns.Add("SubHeading", typeof(string));
            TranslatedText.Columns.Add("Description", typeof(string));
            TranslatedText.Columns.Add("MetaDescription", typeof(string));
            TranslatedText.Columns.Add("TitleTag", typeof(string));

            TranslatedText.Rows.Add(new object[] { ProductPageID, translatedText.Heading, translatedText.SubHeading, translatedText.Description, translatedText.MetaDescription, translatedText.TitleTag });

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.ProductPage_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "ProductPageTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "ProductPageTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }



        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Page_Language_SelectDetail")]
        [ResultType(typeof(Page_Language_SelectDetail_PageResult))]
        [ResultType(typeof(Page_Language_SelectDetail_TranslationResult))]
        [ResultType(typeof(Page_Language_SelectDetail_ReviewRequiredResult))]
        [ResultType(typeof(Page_Language_SelectDetail_ImageResult))]
        public IMultipleResults Page_Language_SelectDetail(int PageID, byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), PageID, languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.Page_Language_SelectList")]
        [ResultType(typeof(Page_Language_SelectList_PageResult))]
        [ResultType(typeof(Brand_SelectResult))]
        [ResultType(typeof(Launch_SelectResult))]
        [ResultType(typeof(ProductType_SelectResult))]
        [ResultType(typeof(Status_SelectResult))]
        public IMultipleResults Page_Language_SelectList(byte languageId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), languageId);
            return (IMultipleResults)result.ReturnValue;
        }

        public string Page_Language_Save(
            int PageID,
            byte languageId,
            Page_Language_SelectDetail_PageResult originalText,
            Page_Language_SelectDetail_TranslationResult translatedText)
        {
            DataSet ds = new DataSet();

            DataTable OriginalText = new DataTable();
            OriginalText.Columns.Add("PageID", typeof(short));
            OriginalText.Columns.Add("Heading", typeof(string));
            OriginalText.Columns.Add("SubHeading", typeof(string));
            OriginalText.Columns.Add("URLKeywords", typeof(string));
            OriginalText.Columns.Add("MetaDescription", typeof(string));
            OriginalText.Columns.Add("TitleTag", typeof(string));
            OriginalText.Columns.Add("Description", typeof(string));

            OriginalText.Rows.Add(new object[] { PageID, originalText.Heading, originalText.SubHeading, originalText.URLKeywords, originalText.MetaDescription, originalText.TitleTag, originalText.Description });

            DataTable TranslatedText = new DataTable();
            TranslatedText.Columns.Add("PageID", typeof(short));
            TranslatedText.Columns.Add("Heading", typeof(string));
            TranslatedText.Columns.Add("SubHeading", typeof(string));
            TranslatedText.Columns.Add("URLKeywords", typeof(string));
            TranslatedText.Columns.Add("MetaDescription", typeof(string));
            TranslatedText.Columns.Add("TitleTag", typeof(string));
            TranslatedText.Columns.Add("Description", typeof(string));

            TranslatedText.Rows.Add(new object[] { PageID, translatedText.Heading, translatedText.SubHeading, translatedText.URLKeywords, translatedText.MetaDescription, translatedText.TitleTag, translatedText.Description });

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.Page_Language_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LanguageID", languageId);
            SqlParameter oT = cmd.Parameters.AddWithValue("@OriginalText", OriginalText);
            oT.SqlDbType = SqlDbType.Structured;
            oT.TypeName = "PageTextTableType";
            SqlParameter tT = cmd.Parameters.AddWithValue("@TranslatedText", TranslatedText);
            tT.SqlDbType = SqlDbType.Structured;
            tT.TypeName = "PageTextTableType";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return "ok";
        }




        //[adm].[SalePrice_Update]
        public string SalePrice_Overview_Update(List<long> SalePriceIDs, DateTime? OnDate, DateTime? OffDate, decimal? Discount, int? StockMinimum, bool OverwriteConflicts)
        {
            DataSet ds = new DataSet();

            DataTable salePrices = new DataTable();
            salePrices.Columns.Add("SalePriceID", typeof(long));

            foreach(long l in SalePriceIDs)
            {
                salePrices.Rows.Add(new object[] { l });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd = new SqlCommand("adm.SalePrice_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter sP = cmd.Parameters.AddWithValue("@SalePriceIDs", salePrices);
            sP.SqlDbType = SqlDbType.Structured;
            sP.TypeName = "SalePriceIDsTableType";

            cmd.Parameters.AddWithValue("@OnDate", OnDate);
            cmd.Parameters.AddWithValue("@OffDate", OffDate);
            cmd.Parameters.AddWithValue("@Discount", Discount);
            cmd.Parameters.AddWithValue("@StockMinimum", StockMinimum);
            cmd.Parameters.AddWithValue("@OverwriteConflicts", OverwriteConflicts);

            SqlParameter output = new SqlParameter();
            output.Direction = ParameterDirection.Output;
            output.ParameterName = "@ErrorMessage";
            output.SqlDbType = SqlDbType.NVarChar;
            output.Size = 50;
            cmd.Parameters.Add(output);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return output.Value.ToString();
        }


        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "wtf.CollectionImage_Export")]
        [ResultType(typeof(CollectionImage_Export_CollectionImageResult))]
        [ResultType(typeof(CollectionImage_Export_LaunchResult))]
        [ResultType(typeof(CollectionImage_Export_BrandResult))]
        [ResultType(typeof(CollectionImage_Export_ProductTypeResult))]
        [ResultType(typeof(CollectionImage_Export_CollectionResult))]
        public IMultipleResults CollectionImage_Export(short LaunchID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), LaunchID);
            return (IMultipleResults)result.ReturnValue;
        }





        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "wtf.ProductImage_Export")]
        [ResultType(typeof(ProductImage_Export_ProductImageResult))]
        [ResultType(typeof(ProductImage_Export_LaunchResult))]
        [ResultType(typeof(ProductImage_Export_BrandResult))]
        [ResultType(typeof(ProductImage_Export_ProductTypeResult))]
        [ResultType(typeof(ProductImage_Export_CollectionResult))]
        public IMultipleResults ProductImage_Export(short LaunchID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), LaunchID);
            return (IMultipleResults)result.ReturnValue;
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name="adm.Sales_Overview")]
        [ResultType(typeof(SalesOverviewSummary))]
        [ResultType(typeof(SalesOverviewTransaction))]
        public IMultipleResults Sales_Overview_Generate(DateTime From, DateTime To, byte PriceListID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), From, To, PriceListID);
            return ((IMultipleResults)result.ReturnValue);
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "adm.PromotionCode_Product_Merge")]
        public int PromotionCode_Product_Merge([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "PromotionCodeID", DbType = "SmallInt")] short promotionCodeID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "PageSectionID", DbType = "BigInt")] long pageSectionID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), promotionCodeID, pageSectionID);
            return ((int)(result.ReturnValue));
        }

        [Function(Name = "adm.Quality_Overview_Generate")]
        [ResultType(typeof(Quality_Overview_Generate_ProductTypeResult))]
        [ResultType(typeof(Quality_Overview_Generate_LaunchResult))]
        [ResultType(typeof(Quality_Overview_Generate_BrandResult))]
        [ResultType(typeof(Quality_Overview_Generate_QualityResult))]
        public IMultipleResults Quality_Overview_Generate()
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod());
            return ((IMultipleResults)result.ReturnValue);
        }

        [Function(Name = "adm.Quality_Overview_Detail_Generate")]
        [ResultType(typeof(Quality_Overview_Detail_StatusResult))]
        [ResultType(typeof(Quality_Overview_Detail_ProductPageResult))]
        public IMultipleResults Quality_Overview_Detail_Generate(short QualityID, byte ProductTypeID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), QualityID, ProductTypeID);
            return ((IMultipleResults)result.ReturnValue);
        }

        public void Quality_Overview_Detail_Save(string ProductPages, string Description, string MetaDescription, string SubHeading)
        {
            List<int> ProductPagesList = ProductPages.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(p => Convert.ToInt32(p)).ToList();

            DataTable ProductPageIDsTable = new DataTable();
            ProductPageIDsTable.Columns.Add("ProductPageID", typeof(int));
            foreach (int i in ProductPagesList)
            {
                ProductPageIDsTable.Rows.Add(i);
            }
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd = new SqlCommand("adm.Quality_Overview_Detail_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter q = cmd.Parameters.AddWithValue("@ProductPageIDs", ProductPageIDsTable);
            q.SqlDbType = SqlDbType.Structured;
            q.TypeName = "ProductPageIDsTableType";
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@MetaDescription", MetaDescription);
            cmd.Parameters.AddWithValue("@SubHeading", SubHeading);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
        }

        [Function(Name = "adm.ProductPage_Attribute_Generate")]
        //[ResultType(typeof(ProductPage_Attribute_Generate_AttributeResult))]
        [ResultType(typeof(ProductPage_Attribute_Generate_ProductPageResult))]
        [ResultType(typeof(ProductPage_Attribute_Generate_AttributeMapResult))]
        public IMultipleResults ProductPage_Attribute_Generate(byte ProductTypeID, byte AttributeGroupID, short? AttributeID, byte? toggle, string searchString)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), ProductTypeID, AttributeGroupID, AttributeID, toggle, searchString);
            return ((IMultipleResults)result.ReturnValue);
        }

        [Function(Name = "adm.ProductPage_Attribute_Generate")]
        //[ResultType(typeof(ProductPage_Attribute_Generate_AttributeResult))]
        [ResultType(typeof(ProductPage_Attribute_Generate_ProductPageResult))]
        [ResultType(typeof(ProductPage_Attribute_Generate_AttributeMapResult))]
        public IMultipleResults ProductPage_Attribute_Generate(byte ProductTypeID, byte AttributeGroupID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), ProductTypeID, AttributeGroupID);
            return ((IMultipleResults)result.ReturnValue);
        }

        public void ProductPage_Attribute_Save(int ProductPageID, byte AttributeGroupID, string Attributes)
        {
            List<short> AttributesList = Attributes.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(p => Convert.ToInt16(p)).ToList();

            DataTable AttributeIDsTable = new DataTable();
            AttributeIDsTable.Columns.Add("AttributeID", typeof(short));
            foreach (short i in AttributesList)
            {
                AttributeIDsTable.Rows.Add(i);
            }
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd = new SqlCommand("adm.ProductPage_Attribute_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter q = cmd.Parameters.AddWithValue("@AttributeIDs", AttributeIDsTable);
            q.SqlDbType = SqlDbType.Structured;
            q.TypeName = "AttributeIDsTableType";
            cmd.Parameters.AddWithValue("@ProductPageID", ProductPageID);
            cmd.Parameters.AddWithValue("@AttributeGroupID", AttributeGroupID);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
        }

        [Function(Name = "adm.Image_Usage")]
        [ResultType(typeof(ImageUsage_Product))]
        [ResultType(typeof(ImageUsage_ProductPage))]
        [ResultType(typeof(ImageUsage_Page))]
        [ResultType(typeof(ImageUsage_Slideshow))]
        [ResultType(typeof(ImageUsage_Collection))]
        [ResultType(typeof(ImageUsage_Article))]
        [ResultType(typeof(ImageUsage_Animation))]
        public IMultipleResults Image_Usage(long ImageID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), ImageID);
            return ((IMultipleResults)result.ReturnValue);
        }

        [Function(Name = "adm.ProductPage_AlternateColourway_SortOrder")]
        [ResultType(typeof(Int32))]
        [ResultType(typeof(ProductPageAlternateColourwaySelect_First))]
        [ResultType(typeof(ProductPageAlternateColourwaySelect_Second))]
        public IMultipleResults ProductPage_AlternateColourway_SortOrder()
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod());
            return ((IMultipleResults)result.ReturnValue);
        }

        
        [Function(Name = "adm.ProductPage_RelatedProductPage_Select")]
        [ResultType(typeof(PPRPPS_One))]
        [ResultType(typeof(PPRPPS_Two))]
        public IMultipleResults ProductPage_RelatedProductPage_Select(
            string SearchString,
            byte? PP_ProductTypeID,
            short? PP_CollectionID,
            byte? PP_BrandID,
            short? PP_LaunchID,
            byte? PP_TemplateID,
            byte? PP_StatusID,
            byte? RPP_ProductTypeID,
            short? RPP_CollectionID,
            byte? RPP_BrandID,
            short? RPP_LaunchID,
            byte? RPP_TemplateID,
            byte? RPP_StatusID,
            byte? RPP_Count,
            string RPP_CountOperator,
            int From,
            int To)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(),
                SearchString,
                PP_ProductTypeID,
                PP_CollectionID,
                PP_BrandID,
                PP_LaunchID,
                PP_TemplateID,
                PP_StatusID,
                RPP_ProductTypeID,
                RPP_CollectionID,
                RPP_BrandID,
                RPP_LaunchID,
                RPP_TemplateID,
                RPP_StatusID,
                RPP_Count,
                RPP_CountOperator,
                From,
                To);
            return ((IMultipleResults)result.ReturnValue);
        }

        [Function(Name = "adm.ProductPage_Attribute_Suggest")]
        [ResultType(typeof(ProductTypeSuggestionResult))]
        [ResultType(typeof(BrandSuggestionResult))]
        [ResultType(typeof(LaunchSuggestionResult))]
        [ResultType(typeof(AttributeResultTable))]
        public IMultipleResults ProductPage_Attribute_Suggest(int ProductPageID, bool JustDoIt)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), ProductPageID, JustDoIt);
            return ((IMultipleResults)result.ReturnValue);
        }

        [Function(Name = "adm.Image_Inspect")]
        [ResultType(typeof(Flock.Models.Image))]
        [ResultType(typeof(ImageInspectHotspotsResult))]
        [ResultType(typeof(ImageInspectUsageResult_Product))]
        [ResultType(typeof(ImageInspectUsageResult_ProductPage))]
        [ResultType(typeof(ImageInspectUsageResult_Collection))]
        [ResultType(typeof(ImageInspectUsageResult_Pods))]
        [ResultType(typeof(ImageInspectUsageResult_Slideshow))]
        public IMultipleResults Image_Inspect(long ImageID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod(), ImageID);
            return ((IMultipleResults)result.ReturnValue);
        }

        public void TranslationImport(List<byte> LanguageIDs, List<byte> LanguageTableIDs, bool PreserveNulls)
        {
            DataTable LanguageIDsTable = new DataTable();
            LanguageIDsTable.Columns.Add("LanguageID", typeof(byte));
            foreach (byte i in LanguageIDs)
                LanguageIDsTable.Rows.Add(i);

            DataTable LanguageTableIDsTable = new DataTable();
            LanguageTableIDsTable.Columns.Add("LanguageTableID", typeof(byte));
            foreach (byte i in LanguageTableIDs)
                LanguageIDsTable.Rows.Add(i);

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            cmd = new SqlCommand("tnt.Translation_Import", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter lIDs = cmd.Parameters.AddWithValue("@LanguageIDs", LanguageIDsTable);
            lIDs.SqlDbType = SqlDbType.Structured;
            lIDs.TypeName = "LanguageIDsTableType";

            SqlParameter ltIDs = cmd.Parameters.AddWithValue("@LanguageTableIDs", LanguageTableIDsTable);
            ltIDs.SqlDbType = SqlDbType.Structured;
            ltIDs.TypeName = "LanguageTableIDsTableType";

            cmd.Parameters.AddWithValue("@PreserveNulls", PreserveNulls);

            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
        }

        public DataSet SalePrice_Validate(List<int> ProductIDs, byte PriceListID, byte SalePriceTypeID, decimal Discount, DateTime OnDate, DateTime? OffDate, byte? StockMinimum)
        {
            DataTable ProductIDsTable = new DataTable();
            ProductIDsTable.Columns.Add("ProductID", typeof(int));
            foreach (int i in ProductIDs)
                ProductIDsTable.Rows.Add(i);

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd = new SqlCommand("adm.SalePrice_Validate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter p = cmd.Parameters.AddWithValue("@ProductIDs", ProductIDsTable);
            p.SqlDbType = SqlDbType.Structured;
            p.TypeName = "ProductIDsTableType";
            cmd.Parameters.AddWithValue("@PriceListID", PriceListID);
            cmd.Parameters.AddWithValue("@SalePriceTypeID", SalePriceTypeID);
            cmd.Parameters.AddWithValue("@Discount", Discount);
            cmd.Parameters.AddWithValue("@OnDate", OnDate);
            cmd.Parameters.AddWithValue("@OffDate", OffDate);
            cmd.Parameters.AddWithValue("@StockMinimum", StockMinimum);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return ds;
        }

        public DataTable Member_Import(List<Flock.Controllers.MailingListExcelRow> Members, short CountryID, byte LanguageID, byte DotDigitalAccountID, short? ThirdPartyID, bool SendWelcomeEmail)
        {
            DataTable MemberImportTable = new DataTable();
            MemberImportTable.Columns.Add("Email", typeof(string));
            MemberImportTable.Columns.Add("FullName", typeof(string));

            foreach (var m in Members) {
                MemberImportTable.Rows.Add(new object[] { m.EMAIL, m.FULLNAME });
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            DataTable ret = new DataTable();
            ret.Columns.Add("Email", typeof(string));

            cmd = new SqlCommand("adm.Member_Import", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            SqlParameter p = cmd.Parameters.AddWithValue("@MemberImport", MemberImportTable);
            p.SqlDbType = SqlDbType.Structured;
            p.TypeName = "MemberImportTableType";
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.Parameters.AddWithValue("@LanguageID", LanguageID);
            cmd.Parameters.AddWithValue("@DotDigitalAccountID", DotDigitalAccountID);
            cmd.Parameters.AddWithValue("@ThirdPartyID", ThirdPartyID);
            cmd.Parameters.AddWithValue("@SendWelcomeEmail", SendWelcomeEmail);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ret);
            con.Close();
            return ret;
        }

        public void DeliveryMatrix_Merge(short CountryID, byte DeliveryOptionID, string CourierProductTypes)
        {
            List<CourierProductTypeDatum> TypesList = new JavaScriptSerializer().Deserialize<List<CourierProductTypeDatum>>(CourierProductTypes).ToList();
            DataTable DeliveryMatrixTable = new DataTable();

            DeliveryMatrixTable.Columns.Add("CountryID", typeof(short));
            DeliveryMatrixTable.Columns.Add("DeliveryOptionID", typeof(byte));
            DeliveryMatrixTable.Columns.Add("ProductTypeID", typeof(byte));
            DeliveryMatrixTable.Columns.Add("CourierID", typeof(byte));

            foreach (CourierProductTypeDatum i in TypesList)
            {
                DeliveryMatrixTable.Rows.Add(i.CountryID, i.DeliveryOptionID, i.ProductTypeID, i.CourierID);
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("adm.DeliveryMatrix_Merge", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlParameter p = cmd.Parameters.AddWithValue("@DeliveryMatrix", DeliveryMatrixTable);
            p.SqlDbType = SqlDbType.Structured;
            p.TypeName = "DeliveryMatrixTableType";
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.Parameters.AddWithValue("@DeliveryOptionID", DeliveryOptionID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
        }

        public struct CourierProductTypeDatum
        {
            public short CountryID;
            public byte DeliveryOptionID;
            public byte ProductTypeID;
            public byte CourierID;
        };

        public void ProductPage_Image_Merge(int ProductPageID, string Images, byte TemplateImageID)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> ImagesList = ser.Deserialize<List<SortDatum>>(Images).ToList();

            DataTable ImageIDsTable = new DataTable();
            ImageIDsTable.Columns.Add("ImageID", typeof(long));
            ImageIDsTable.Columns.Add("SortOrder", typeof(byte));
            foreach (SortDatum i in ImagesList) {
                ImageIDsTable.Rows.Add(i.ID, i.SortOrder);
            }
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd = new SqlCommand("adm.ProductPage_Image_Merge", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter p = cmd.Parameters.AddWithValue("@ImageIDs", ImageIDsTable);
            p.SqlDbType = SqlDbType.Structured;
            p.TypeName = "ImageIDsSortOrderTableType";
            cmd.Parameters.AddWithValue("@ProductPageID", ProductPageID);
            cmd.Parameters.AddWithValue("@TemplateImageID", TemplateImageID);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
        }


        public void Article_Image_Merge(int ArticleID, string Images, byte TemplateImageID)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> ImagesList = ser.Deserialize<List<SortDatum>>(Images).ToList();

            DataTable ImageIDsTable = new DataTable();
            ImageIDsTable.Columns.Add("ImageID", typeof(long));
            ImageIDsTable.Columns.Add("SortOrder", typeof(byte));
            foreach (SortDatum i in ImagesList)
            {
                ImageIDsTable.Rows.Add(i.ID, i.SortOrder);
            }
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd = new SqlCommand("adm.Article_Image_Merge", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter p = cmd.Parameters.AddWithValue("@ImageIDs", ImageIDsTable);
            p.SqlDbType = SqlDbType.Structured;
            p.TypeName = "ImageIDsSortOrderTableType";
            cmd.Parameters.AddWithValue("@ArticleID", ArticleID);
            cmd.Parameters.AddWithValue("@TemplateImageID", TemplateImageID);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
        }

        public void Slideshow_Image_Merge(int SlideshowID, string Images)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> ImagesList = ser.Deserialize<List<SortDatum>>(Images).ToList();

            DataTable ImageIDsTable = new DataTable();
            ImageIDsTable.Columns.Add("ImageID", typeof(long));
            ImageIDsTable.Columns.Add("SortOrder", typeof(byte));
            foreach (SortDatum i in ImagesList)
            {
                ImageIDsTable.Rows.Add(i.ID, i.SortOrder);
            }
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd = new SqlCommand("adm.Slideshow_Image_Merge", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter p = cmd.Parameters.AddWithValue("@ImageIDs", ImageIDsTable);
            p.SqlDbType = SqlDbType.Structured;
            p.TypeName = "ImageIDsSortOrderTableType";
            cmd.Parameters.AddWithValue("@SlideshowID", SlideshowID);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
        }

        public void Collection_Image_Merge(short CollectionID, string Images, byte TemplateImageID)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> ImagesList = ser.Deserialize<List<SortDatum>>(Images).ToList();

            DataTable ImageIDsTable = new DataTable();
            ImageIDsTable.Columns.Add("ImageID", typeof(long));
            ImageIDsTable.Columns.Add("SortOrder", typeof(byte));
            foreach (SortDatum i in ImagesList)
            {
                ImageIDsTable.Rows.Add(i.ID, i.SortOrder);
            }
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd = new SqlCommand("adm.Collection_Image_Merge", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter p = cmd.Parameters.AddWithValue("@ImageIDs", ImageIDsTable);
            p.SqlDbType = SqlDbType.Structured;
            p.TypeName = "ImageIDsSortOrderTableType";
            cmd.Parameters.AddWithValue("@CollectionID", CollectionID);
            cmd.Parameters.AddWithValue("@TemplateImageID", TemplateImageID);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
        }

        public void Page_Image_Merge(int PageID, string Images, byte TemplateImageID)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SortDatum> ImagesList = ser.Deserialize<List<SortDatum>>(Images).ToList();

            DataTable ImageIDsTable = new DataTable();
            ImageIDsTable.Columns.Add("ImageID", typeof(long));
            ImageIDsTable.Columns.Add("SortOrder", typeof(byte));
            foreach (SortDatum i in ImagesList)
            {
                ImageIDsTable.Rows.Add(i.ID, i.SortOrder);
            }
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd = new SqlCommand("adm.Page_Image_Merge", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter p = cmd.Parameters.AddWithValue("@ImageIDs", ImageIDsTable);
            p.SqlDbType = SqlDbType.Structured;
            p.TypeName = "ImageIDsSortOrderTableType";
            cmd.Parameters.AddWithValue("@PageID", PageID);
            cmd.Parameters.AddWithValue("@TemplateImageID", TemplateImageID);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
        }

        public void Price_Insert(List<int> ProductIDs, List<byte> PriceLists)
        {
            DataTable ProductIDsTable = new DataTable();
            ProductIDsTable.Columns.Add("ProductID", typeof(int));
            foreach (int i in ProductIDs)
                ProductIDsTable.Rows.Add(i);

            DataTable PriceListIDsTable = new DataTable();
            PriceListIDsTable.Columns.Add("PriceListID", typeof(byte));
            foreach (byte pl in PriceLists)
                PriceListIDsTable.Rows.Add(pl);

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("adm.Price_Insert", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter p = cmd.Parameters.AddWithValue("@ProductIDs", ProductIDsTable);
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName = "ProductIDsTableType";
                SqlParameter pl = cmd.Parameters.AddWithValue("@PriceListIDs", PriceListIDsTable);
                pl.SqlDbType = SqlDbType.Structured;
                pl.TypeName = "PriceListIDsTableType";

                conn.Open();
                cmd.ExecuteReader();
            }
            //log e

            return;
        }

        public void SalePrice_Save(DataTable Products, string SalePriceEventName, byte PriceListID, byte SalePriceTypeID, decimal Discount, DateTime OnDate, DateTime? OffDate, byte? StockMinimum, bool OverwriteConflicts = false)
        {
            DataTable SalePricesTable = new DataTable();
            SalePricesTable.Columns.Add("ProductID", typeof(int));
            SalePricesTable.Columns.Add("PriceListID", typeof(short));
            SalePricesTable.Columns.Add("SalePriceTypeID", typeof(byte));
            SalePricesTable.Columns.Add("Discount", typeof(Decimal));
            SalePricesTable.Columns.Add("SalePriceInc", typeof(Decimal));
            SalePricesTable.Columns.Add("SalePriceEx", typeof(Decimal));
            SalePricesTable.Columns.Add("OnDate", typeof(DateTime));
            SalePricesTable.Columns.Add("OffDate", typeof(DateTime));
            SalePricesTable.Columns.Add("StockMinimum", typeof(byte));

            foreach (DataRow l in Products.Rows)
            {
                DataRow r = SalePricesTable.Rows.Add(
                    l.Field<int>("ProductID"),
                    PriceListID,
                    l.Field<byte>("SalePriceTypeID"),
                    l.Field<decimal>("Discount"),
                    l.Field<decimal>("SalePriceInc"),
                    l.Field<decimal>("SalePriceEx"),
                    l.Field<DateTime>("OnDate"),
                    l.Field<DateTime?>("OffDate"),
                    l.Field<System.Int16?>("StockMinimum"));
            }

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("adm.SalePrice_Save", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter p = cmd.Parameters.AddWithValue("@SalePrices", SalePricesTable);
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName = "SalePriceTableType";
                if (OverwriteConflicts)
                    cmd.Parameters.AddWithValue("@OverwriteConflicts", true);
                cmd.Parameters.AddWithValue("@SalePriceEventName", SalePriceEventName);
                conn.Open();
                cmd.ExecuteReader();
            }
        }


        //public void SalePrice_Update(MVCxGridViewBatchUpdateValues<vwSalePrice, long> updateValues)
        //{
        //    DataTable SalePricesTable = new DataTable();
        //    SalePricesTable.Columns.Add("SalePriceID", typeof(long));
        //    SalePricesTable.Columns.Add("PriceListID", typeof(short));
        //    SalePricesTable.Columns.Add("ProductID", typeof(int));
        //    SalePricesTable.Columns.Add("SalePriceTypeID", typeof(byte));
        //    SalePricesTable.Columns.Add("Discount", typeof(Decimal));
        //    SalePricesTable.Columns.Add("SalePriceInc", typeof(Decimal));
        //    SalePricesTable.Columns.Add("SalePriceEx", typeof(Decimal));
        //    SalePricesTable.Columns.Add("OnDate", typeof(DateTime));
        //    SalePricesTable.Columns.Add("OffDate", typeof(DateTime));

        //    foreach (var l in updateValues.Update)
        //    {
        //        DataRow r = SalePricesTable.Rows.Add(l.SalePriceID, DBNull.Value, DBNull.Value, l.SalePriceTypeID, l.Discount,l.SalePriceInc, l.SalePriceEx, l.OnDate, l.OffDate);
        //    }

        //    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("adm.SalePrice_Update", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Clear();
        //        SqlParameter p = cmd.Parameters.AddWithValue("@SalePrice", SalePricesTable);
        //        p.SqlDbType = SqlDbType.Structured;
        //        p.TypeName = "SalePriceTableType";

        //        conn.Open();
        //        cmd.ExecuteReader();
        //    }
        //    //log e
        //    return;
        //}

        public void Price_Delete(List<int> ProductIDs, List<byte> PriceLists)
        {
            DataTable ProductIDsTable = new DataTable();
            ProductIDsTable.Columns.Add("ProductID", typeof(int));
            foreach (int i in ProductIDs)
                ProductIDsTable.Rows.Add(i);

            DataTable PriceListIDsTable = new DataTable();
            PriceListIDsTable.Columns.Add("PriceListID", typeof(byte));
            foreach (byte pl in PriceLists)
                PriceListIDsTable.Rows.Add(pl);

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("adm.Price_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter p = cmd.Parameters.AddWithValue("@ProductIDs", ProductIDsTable);
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName = "ProductIDsTableType";
                SqlParameter pl = cmd.Parameters.AddWithValue("@PriceListIDs", PriceListIDsTable);
                pl.SqlDbType = SqlDbType.Structured;
                pl.TypeName = "PriceListIDsTableType";

                conn.Open();
                cmd.ExecuteReader();
            }
            //log e
            return;
        }

        public void ProductPage_Attribute_Update(int ProductPageID, byte? ProductTypeID, byte? BrandID, short? LaunchID, byte? ColourID, byte? PatternID, List<short> AttributeIDs)
        {
            DataTable AttributesTable = new DataTable();
            AttributesTable.Columns.Add("AttributeID", typeof(short));

            foreach (short s in AttributeIDs)
                AttributesTable.Rows.Add(s);

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("adm.ProductPage_Attribute_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@ProductPageID", SqlDbType.Int)).Value = ProductPageID;
                cmd.Parameters.Add(new SqlParameter("@ProductTypeID", SqlDbType.TinyInt)).Value = ProductTypeID;
                cmd.Parameters.Add(new SqlParameter("@BrandID", SqlDbType.TinyInt)).Value = BrandID;
                cmd.Parameters.Add(new SqlParameter("@LaunchID", SqlDbType.SmallInt)).Value = LaunchID;
                cmd.Parameters.Add(new SqlParameter("@ColourID", SqlDbType.TinyInt)).Value = ColourID;
                cmd.Parameters.Add(new SqlParameter("@PatternID", SqlDbType.TinyInt)).Value = PatternID;
                SqlParameter p = cmd.Parameters.AddWithValue("@AttributeIDs", AttributesTable);
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName = "AttributeIDsTableType";

                conn.Open();
                cmd.ExecuteReader();
            }
            // log e
            return;
        }

        public void Product_Attribute_UpdateFromProductPage(int ProductPageID, byte ProductTypeID, byte? BrandID, short? LaunchID, List<short> AttributeIDs, List<int> ProductIDs)
        {
            DataTable ProductsTable = new DataTable();
            ProductsTable.Columns.Add("ProductID", typeof(int));
            foreach (int i in ProductIDs)
                ProductsTable.Rows.Add(i);

            DataTable AttributesTable = new DataTable();
            AttributesTable.Columns.Add("AttributeID", typeof(short));

            foreach (short s in AttributeIDs)
                AttributesTable.Rows.Add(s);

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("adm.Product_Attribute_UpdateFromProductPage", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@ProductPageID", SqlDbType.Int)).Value = ProductPageID;
                cmd.Parameters.Add(new SqlParameter("@ProductTypeID", SqlDbType.TinyInt)).Value = ProductTypeID;
                cmd.Parameters.Add(new SqlParameter("@BrandID", SqlDbType.TinyInt)).Value = BrandID;
                cmd.Parameters.Add(new SqlParameter("@LaunchID", SqlDbType.SmallInt)).Value = LaunchID;
                SqlParameter p = cmd.Parameters.AddWithValue("@AttributeIDs", AttributesTable);
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName = "AttributeIDsTableType";
                SqlParameter q = cmd.Parameters.AddWithValue("@ProductIDs", ProductsTable);
                q.SqlDbType = SqlDbType.Structured;
                q.TypeName = "ProductIDsTableType";

                conn.Open();
                cmd.ExecuteReader();
            }
            // log e
            return;
        }

        public List<Order_ExportResult> Order_Export(byte PriceListID, List<Int32> OrderHeadIDs)
        {
            var results = new List<Order_ExportResult>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ToString()))
            {
                DataTable orderHeads = new DataTable();
                orderHeads.Columns.Add("OrderHeadID", typeof(Int32));
                
                foreach (var t in OrderHeadIDs)
                {
                    orderHeads.Rows.Add(t);
                }

                SqlCommand cmd = new SqlCommand("adm.Order_Export", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PriceListID", PriceListID);
                
                SqlParameter p = cmd.Parameters.AddWithValue("@OrderHeadIDs", orderHeads);
                p.Value = orderHeads;
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName = "OrderHeadIDsTableType";

                conn.Open();
                var reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    results.Add(new Order_ExportResult() {
                        ACCOUNT_NUMBER = reader.GetString(0),
                        ORDER_REFERNCE = reader.GetString(1),
                        DELIVERY_NAME = reader.GetString(2),
                        DELIVERY_ADDRESS_1 = reader.GetString(3),
                        DELIVERY_ADDRESS_2 = reader.GetString(4),
                        DELIVERY_ADDRESS_3 = reader.GetString(5),
                        DELIVERY_ADDRESS_4 = reader.GetString(6),
                        DELIVERY_POST_CODE = reader.GetString(7),
                        DELIVERY_COUNTRY_CODE = reader.GetString(8),
                        DELIVERY_TELEPHONE_NUMBER = reader.GetString(9),
                        CONFIRMATION_EMAIL_ADDRESS = reader.GetString(10),
                        PRODUCT_CODE = reader.GetString(11),
                        QTY = reader.GetString(12),
                        TYPE = reader.GetString(13),
                        LINE_REFERENCE = reader.GetString(14),
                        MEMO = reader.GetString(15),
                        LINE_TOTAL_EX_TAX = reader.IsDBNull(16) ? (decimal?)null : reader.GetDecimal(16),
                        LINE_TOTAL_TAX = reader.IsDBNull(17) ? (decimal?)null : reader.GetDecimal(17),
                        SHIPPING_CHARGE_EXCLUDING_TAX = reader.IsDBNull(18) ? (decimal?)null : reader.GetDecimal(18),
                        SHIPPING_CHARGE_TAX = reader.IsDBNull(19) ? (decimal?)null : reader.GetDecimal(19),
                        TOTAL_EXCLUDING_TAX = reader.IsDBNull(20) ? (decimal?)null : reader.GetDecimal(20),
                        TOTAL_TAX = reader.IsDBNull(21) ? (decimal?)null : reader.GetDecimal(21)
                    });
                }
                conn.Close();
            }

            return results;
        }



        public void ProductBundle_Price_Initialise(int? BundleID, List<int> ProductIDs, byte PriceListID)
        {
            DataTable ProductsTable = new DataTable();
            ProductsTable.Columns.Add("ProductID", typeof(int));
            foreach (int i in ProductIDs)
                ProductsTable.Rows.Add(i);


            
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("adm.ProductBundle_Price_Initialise", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.Add(new SqlParameter("@PriceListID", SqlDbType.TinyInt)).Value = PriceListID;
                
                SqlParameter q = cmd.Parameters.AddWithValue("@ProductIDs", ProductsTable);
                q.SqlDbType = SqlDbType.Structured;
                q.TypeName = "ProductIDsTableType";

                conn.Open();
                cmd.ExecuteReader();
            }

            return;
        }


        public void ProductBundle_Price_Delete(List<int> ProductIDs, byte PriceListID)
        {
            DataTable ProductsTable = new DataTable();
            ProductsTable.Columns.Add("ProductID", typeof(int));
            foreach (int i in ProductIDs)
                ProductsTable.Rows.Add(i);

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FlockConnectionString"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("adm.ProductBundle_Price_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@PriceListID", SqlDbType.TinyInt)).Value = PriceListID;

                SqlParameter q = cmd.Parameters.AddWithValue("@ProductIDs", ProductsTable);
                q.SqlDbType = SqlDbType.Structured;
                q.TypeName = "ProductIDsTableType";

                conn.Open();
                cmd.ExecuteReader();
            }

            return;
        }
    }
    public class Order_ExportResult
    {
        public String ACCOUNT_NUMBER { get; set; }
        public String ORDER_REFERNCE { get; set; }
        public String DELIVERY_NAME { get; set; }
        public String DELIVERY_ADDRESS_1 { get; set; }
        public String DELIVERY_ADDRESS_2 { get; set; }
        public String DELIVERY_ADDRESS_3 { get; set; }
        public String DELIVERY_ADDRESS_4 { get; set; }
        public String DELIVERY_POST_CODE { get; set; }
        public String DELIVERY_COUNTRY_CODE { get; set; }
        public String DELIVERY_TELEPHONE_NUMBER { get; set; }
        public String CONFIRMATION_EMAIL_ADDRESS { get; set; }
        public String PRODUCT_CODE { get; set; }
        public String QTY { get; set; }
        public String TYPE { get; set; }
        public String LINE_REFERENCE { get; set; }
        public String MEMO { get; set; }
        public decimal? LINE_TOTAL_EX_TAX { get; set; }
        public decimal? LINE_TOTAL_TAX { get; set; }
        public decimal? SHIPPING_CHARGE_EXCLUDING_TAX { get; set; }
        public decimal? SHIPPING_CHARGE_TAX { get; set; }
        public decimal? TOTAL_EXCLUDING_TAX { get; set; }
        public decimal? TOTAL_TAX { get; set; }
    }

    public static class QueryableChunked
    {
        public static IEnumerable<T> InRange<T, TValue>(
                this IQueryable<T> source,
                Expression<Func<T, TValue>> selector,
                int blockSize,
                IEnumerable<TValue> values)
        {
            MethodInfo method = null;
            foreach (MethodInfo tmp in typeof(Enumerable).GetMethods(
                    BindingFlags.Public | BindingFlags.Static))
            {
                if (tmp.Name == "Contains" && tmp.IsGenericMethodDefinition
                        && tmp.GetParameters().Length == 2)
                {
                    method = tmp.MakeGenericMethod(typeof(TValue));
                    break;
                }
            }
            if (method == null) throw new InvalidOperationException(
                   "Unable to locate Contains");
            foreach (TValue[] block in GetBlocks(values, blockSize))// values.GetBlocks(blockSize))
            {
                var row = Expression.Parameter(typeof(T), "row");
                var member = Expression.Invoke(selector, row);
                var keys = Expression.Constant(block, typeof(TValue[]));
                var predicate = Expression.Call(method, keys, member);
                var lambda = Expression.Lambda<Func<T, bool>>(
                      predicate, row);
                foreach (T record in source.Where(lambda))
                {
                    yield return record;
                }
            }
        }

        public static IEnumerable<T[]> GetBlocks<T>(
                this IEnumerable<T> source, int blockSize)
        {
            List<T> list = new List<T>(blockSize);
            foreach (T item in source)
            {
                list.Add(item);
                if (list.Count == blockSize)
                {
                    yield return list.ToArray();
                    list.Clear();
                }
            }
            if (list.Count > 0)
            {
                yield return list.ToArray();
            }
        }
    }
}
namespace Flock.ViewModels
{

    public class ProductTypeDatum
    {
        public byte ProductTypeID { get; set; }
        public string Name { get; set; }
        public string URLKeywords { get; set; }
    }

    public class StyleDatum
    {
        public short StyleID { get; set; }
        public string Name { get; set; }
    }

    public class PageDatum
    {
        public int PageID { get; set; }
        public string Name { get; set; }
    }

    public class TransactionDatum
    {
        public int TransactionID { get; set; }
        public string TransactionItems { get; set; }
        public string TransactionTotal { get; set; }
        public string TransactionPandP { get; set; }
        public string CreditTotal { get; set; }
    }

    public class WidthModeDatum
    {
        public byte WidthModeID { get; set; }
        public string Name { get; set; }
    }

    public class ArticleDatum
    {
        public int ArticleID { get; set; }
        public string Name { get; set; }
    }

    public class CollectionDatum
    {
        public short CollectionID { get; set; }
        public string Name { get; set; }
    }

    public class ProductPageDatum
    {
        public int ProductPageID { get; set; }
        public string Name { get; set; }
    }

    public class ProductDatum
    {
        public int ProductID { get; set; }
        public int? ProductPageID { get; set; }
        public string BasketName { get; set; }
        public string CostSterling { get; set; }
    }

    public static class LargeDatabaseDataProvider
    {
        const string LargeDatabaseDataContextKey = "DXLargeDatabaseDataContext";

        public static Flock.Models.FlockDataContext DB
        {
            get
            {
                if (HttpContext.Current.Items[LargeDatabaseDataContextKey] == null)
                    HttpContext.Current.Items[LargeDatabaseDataContextKey] = new Flock.Models.FlockDataContext();
                return (Flock.Models.FlockDataContext)HttpContext.Current.Items[LargeDatabaseDataContextKey];
            }
        }




        public static object GetSiteSectionIDs(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.SiteSections
                .Where(p => p.SiteSectionID.ToString().StartsWith(args.Filter))
                .OrderByDescending(p => p.SiteSectionID)
                .Skip(skip).Take(take).ToList();

            return Model;
        }

        public static object GetSiteSectionByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                var Model = DB.SiteSections.Where(t => t.SiteSectionID == Convert.ToByte(args.Value)).ToList();

                return Model.ToList();
            }
            else
            {
                return null;
            }
        }




        public static object GetModuleOptionGroups(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var model = DB.ModuleOptionGroups
                .Where(p => p.ModuleOptionGroupID.ToString().StartsWith(args.Filter) || p.Name.Contains(args.Filter))
                .OrderByDescending(p => p.ModuleOptionGroupID)
                .Skip(skip).Take(take).ToList();

            return model;
        }

        public static object GetModuleOptionGroupByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                var model = DB.ModuleOptionGroups.Where(t => t.ModuleOptionGroupID == (Int16)args.Value).ToList();

                return model;
            }
            else
            {
                return null;
            }
        }


        public static object GetCallToActionWidthModeIDs(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var preModel = new List<CallToActionWidthMode>();

            if (args.Filter != "")
            {
                preModel = DB.CallToActionWidthModes
                 .Where(p => p.CallToActionWidthModeID.ToString().StartsWith(args.Filter) || p.Name.Contains(args.Filter))
                 .Skip(skip).Take(take).ToList();
            }
            else
            {
                preModel = DB.CallToActionWidthModes
                    .Skip(skip).Take(take).ToList();
            }

            var ret = preModel.Select(t => new WidthModeDatum()
            {
                WidthModeID = t.CallToActionWidthModeID,
                Name = t.Name
            }).ToList();

            return ret;
        }

        public static object GetCallToActionWidthModeByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                var preModel = DB.CallToActionWidthModes.Where(t => t.CallToActionWidthModeID == Convert.ToByte(args.Value)).ToList();

                var ret = preModel.Select(t => new WidthModeDatum()
                {
                    WidthModeID = t.CallToActionWidthModeID,
                    Name = t.Name
                }).ToList();

                return ret.ToList();
            }
            else
            {
                return null;
            }
        }

        public static object GetStyleIDs(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var preModel = new List<Style>();

            if (args.Filter != "")
            {
                preModel = DB.Styles
                 .Where(p => p.StyleID.ToString().StartsWith(args.Filter) || p.Name.Contains(args.Filter) && p.DeletedDT == null)
                 .OrderByDescending(p => p.StyleID)
                 .Skip(skip).Take(take).ToList();
            }
            else
            {
                preModel = DB.Styles
                    .Where(p => p.DeletedDT == null)
                    .OrderByDescending(p => p.StyleID)
                    .Skip(skip).Take(take).ToList();
            }
            
            var ret = preModel.Select(t => new StyleDatum()
            {
                StyleID = t.StyleID,
                Name = t.Name
            }).ToList();

            return ret;
        }

        public static object GetStyleByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                var preModel = DB.Styles.Where(t => t.StyleID == Convert.ToInt16(args.Value) && t.DeletedDT == null).ToList();

                var ret = preModel.Select(t => new StyleDatum()
                {
                    StyleID = t.StyleID,
                    Name = t.Name
                }).ToList();

                return ret.ToList();
            }
            else
            {
                return null;
            }
        }

        public static object GetStyleByValue(object value)
        {
            return value != null ? DB.Styles.Where(c => c.StyleID == (short)value).Take(1).ToList() : null;
        }

        public static IEnumerable<PageDatum> GetPageIDs(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var preModel = new List<Page>();

            if (args.Filter != "")
            {
                int id = -1;

                if (Int32.TryParse(args.Filter, out id))
                {

                }

                preModel = DB.Pages
                    .Where(p => p.PageID.ToString().StartsWith(args.Filter) || p.Name.Contains(args.Filter) || p.PageID == id && p.DeletedDT == null)
                    .OrderByDescending(p => p.PageID)
                    .Skip(skip).Take(take).ToList();
            }
            else
            {
                preModel = DB.Pages
                    .Where(p => p.DeletedDT == null)
                    .OrderByDescending(p => p.PageID)
                    .Skip(skip).Take(take).ToList();
            }


            var ret = preModel.Select(t => new PageDatum()
            {
                PageID = t.PageID,
                Name = t.Name
            }).ToList();

            return ret;
        }

        public static IEnumerable<PageDatum> GetPageByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                var preModel = DB.Pages.Where(t => t.PageID == Convert.ToInt32(args.Value) && t.DeletedDT == null).ToList();

                var ret = preModel.Select(t => new PageDatum()
                {
                    PageID = t.PageID,
                    Name = t.Name
                }).ToList();

                return ret.ToList();
            }
            else
            {
                return null;
            }
        }


        public static IEnumerable<Member> GetMembers(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var Skip = args.BeginIndex;
            var Take = args.EndIndex - args.BeginIndex + 1;

            var Model = new List<Member>();

            if (args.Filter != "")
            {
                Model = DB.Members
                    .Where(p => 
                    p.Deleted == false && 
                    (
                        p.MemberID.ToString() == args.Filter || 
                        p.Email.StartsWith(args.Filter) || 
                        p.FullName.Contains(args.Filter))
                    )
                    .OrderByDescending(p => p.MemberID)
                    .Skip(Skip).Take(Take).ToList();
            }
            else
            {
                Model = DB.Members
                    .Where(p => p.DeletedDT == null)
                    .OrderByDescending(p => p.MemberID)
                    .Skip(Skip).Take(Take).ToList();
            }

            return Model;
        }

        public static IEnumerable<Member> GetMemberByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                var Model = DB.Members.Where(t => t.MemberID == Convert.ToInt32(args.Value) && t.DeletedDT == null).ToList();
                
                return Model;
            }
            else
            {
                return null;
            }
        }


        public static object GetArticleIDs(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var preModel = new List<Article>();

            if(args.Filter != "")
            {
                preModel = DB.Articles
                    .Where(p => p.ArticleID.ToString().StartsWith(args.Filter) || p.Name.Contains(args.Filter) && p.DeletedDT == null)
                    .OrderByDescending(p => p.ArticleID)
                    .Skip(skip).Take(take).ToList();
            }
            else
            {
                preModel = DB.Articles
                    .Where(p => p.DeletedDT == null)
                    .OrderByDescending(p => p.ArticleID)
                    .Skip(skip).Take(take).ToList();
            }

            var ret = preModel.Select(t => new ArticleDatum()
            {
                ArticleID = t.ArticleID,
                Name = t.Name
            }).ToList();

            return ret;
        }

        public static object GetArticleByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                var preModel = DB.Articles.Where(t => t.ArticleID == Convert.ToInt32(args.Value) && t.DeletedDT == null).ToList();

                var ret = preModel.Select(t => new ArticleDatum()
                {
                    ArticleID = t.ArticleID,
                    Name = t.Name
                }).ToList();

                return ret.ToList();
            }
            else
            {
                return null;
            }
        }


        public static object GetCollectionIDs(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var preModel = new List<Flock.Models.Collection>();

            if (args.Filter != "")
            {
                preModel = DB.Collections
                    .Where(p => p.CollectionID.ToString().StartsWith(args.Filter) || p.Name.Contains(args.Filter) && p.DeletedDT == null)
                    .OrderByDescending(p => p.CollectionID)
                    .Skip(skip).Take(take).ToList();

            }
            else
            {
                preModel = DB.Collections
                    .Where(p => p.DeletedDT == null)
                    .OrderByDescending(p => p.CollectionID)
                    .Skip(skip).Take(take).ToList();
            }

            var ret = preModel.Select(t => new CollectionDatum()
            {
                CollectionID = t.CollectionID,
                Name = t.Name
            }).ToList();

            return ret;
        }

        public static object GetCollectionByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                var preModel = DB.Collections.Where(t => t.CollectionID == Convert.ToInt16(args.Value) && t.DeletedDT == null).ToList();

                var ret = preModel.Select(t => new CollectionDatum()
                {
                    CollectionID = t.CollectionID,
                    Name = t.Name
                }).ToList();

                return ret.ToList();
            }
            else
            {
                return null;
            }
        }



        public static object GetProductPageIDs(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var preModel = new List<ProductPage>();

            if(args.Filter != "")
            {
                preModel = DB.ProductPages
                    .Where(p => p.ProductPageID.ToString().StartsWith(args.Filter) || p.Name.Contains(args.Filter) && p.DeletedDT == null)
                    .OrderByDescending(p => p.ProductPageID)
                    .Skip(skip).Take(take).ToList();
            }
            else
            {
                preModel = DB.ProductPages
                    .Where(p => p.DeletedDT == null)
                    .OrderByDescending(p => p.ProductPageID)
                    .Skip(skip).Take(take).ToList();
            }

            var ret = preModel.Select(t => new ProductPageDatum()
            {
                ProductPageID = t.ProductPageID,
                Name = t.Name
            }).ToList();

            return ret;
        }

        public static object GetProductPageByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                var preModel = DB.ProductPages.Where(t => t.ProductPageID == Convert.ToInt32(args.Value) && t.DeletedDT == null).ToList();

                var ret = preModel.Select(t => new ProductPageDatum()
                {
                    ProductPageID = t.ProductPageID,
                    Name = t.Name
                }).ToList();

                return ret.ToList();
            }
            else
            {
                return null;
            }
        }

        public static object GetColours(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var ret = DB.Colours.Where(p => p.DeletedDT == null).ToList();
            
            return ret;
        }

        public static object GetColourByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                return DB.Colours.Where(p => p.DeletedDT == null && p.ColourID == Convert.ToByte(args.Value)).ToList();
            }
            else
            {
                return null;
            }
        }

        public static object GetProductIDs(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var preModel = DB.Products
                .Where(p => p.ProductID.ToString().StartsWith(args.Filter) || p.BasketName.Contains(args.Filter) && p.Enabled == true)
                .OrderByDescending(p => p.ProductID)
                .Skip(skip).Take(take).ToList();

            var ret = preModel.Select(t => new ProductDatum()
            {
                ProductID = t.ProductID,
                BasketName = t.BasketName,
                CostSterling = t.CostSterling.HasValue ? t.CostSterling.Value.ToString("F2") : "",
                ProductPageID = t.ProductPageID
            }).ToList();

            return ret;
        }

        public static object GetProductByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                var preModel = DB.Products.Where(t => t.ProductID == Convert.ToInt32(args.Value) && t.Enabled == true).ToList();

                var ret = preModel.Select(t => new ProductDatum()
                {
                    ProductID = t.ProductID,
                    BasketName = t.BasketName,
                    CostSterling = t.CostSterling.HasValue ? t.CostSterling.Value.ToString("F2") : "",
                    ProductPageID = t.ProductPageID
                }).ToList();

                return ret.ToList();
            }
            else
            {
                return null;
            }
        }



        public static object GetProductPageFilterOptions(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var preModel = DB.Products
                .Where(p => p.ProductID.ToString().StartsWith(args.Filter) || p.BasketName.Contains(args.Filter) && p.Enabled == true)
                .OrderByDescending(p => p.ProductID)
                .Skip(skip).Take(take).ToList();

            var ret = preModel.Select(t => new ProductDatum()
            {
                ProductID = t.ProductID,
                BasketName = t.BasketName,
                CostSterling = t.CostSterling.HasValue ? t.CostSterling.Value.ToString("F2") : "",
                ProductPageID = t.ProductPageID
            }).ToList();

            return ret;
        }

        //public static object GetProductByID(ListEditItemRequestedByValueEventArgs args)
        //{
        //    if (args.Value != null)
        //    {
        //        var preModel = DB.Products.Where(t => t.ProductID == Convert.ToInt32(args.Value) && t.Enabled == true).ToList();

        //        var ret = preModel.Select(t => new ProductDatum()
        //        {
        //            ProductID = t.ProductID,
        //            BasketName = t.BasketName,
        //            CostSterling = t.CostSterling.HasValue ? t.CostSterling.Value.ToString("F2") : "",
        //            ProductPageID = t.ProductPageID
        //        }).ToList();

        //        return ret.ToList();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}


        public static object GetTransactionIDs(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var preModel = DB.Transactions
                .Where(p => p.TransactionID.ToString().StartsWith(args.Filter) && p.TransactionID > 9190057 && p.TotalItems != null && p.TotalPandP != null && p.TotalPayable != null && p.TotalPromotionDiscount != null)
                .OrderByDescending(p => p.TransactionID)
                .Skip(skip).Take(take).ToList();

            var preCrd = DB.GiftVouchers.Where(p => p.CreditTransactionID != null); 
            var crd = preCrd.Where(p => preModel.Select(q => q.TransactionID).ToList().Contains(p.CreditTransactionID.Value)).ToList();

            var ret = preModel.Select(t => new TransactionDatum() 
            {
                TransactionID = t.TransactionID,
                TransactionItems = (t.TotalItems.Value - t.TotalPromotionDiscount.Value).ToString("C"),
                TransactionPandP = t.TotalPandP.Value.ToString("C"),
                TransactionTotal = t.TotalPayable.Value.ToString("C"),
                CreditTotal = crd.Where(p => p.CreditTransactionID == t.TransactionID).Select(p => p.ValueInc).Sum().ToString("C")
            }).ToList();

            return ret;
        }

        public static object GetTransactionByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                var crd = DB.GiftVouchers.Where(p => p.CreditTransactionID == Convert.ToInt32(args.Value)).ToList();

                var preModel = DB.Transactions.Where(t => t.TransactionID == Convert.ToInt32(args.Value) && t.TransactionID > 9190057).ToList();

                var ret = preModel
                    .Select(t => new TransactionDatum()
                    {
                        TransactionID = t.TransactionID,
                        TransactionItems = (t.TotalItems.Value - t.TotalPromotionDiscount.Value).ToString("C"),
                        TransactionPandP = t.TotalPandP.Value.ToString("C"),
                        TransactionTotal = t.TotalPayable.Value.ToString("C"),
                        CreditTotal = crd.Where(p => p.CreditTransactionID == t.TransactionID).Select(p => p.ValueInc).Sum().ToString("C")
                    }).ToList();

                return ret.ToList();
            }
            else
            {
                return null;
            }
        }


        public static object PageSectionCloneGetPageIDs(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var preModel = DB.Pages
                .Where(p => p.PageID.ToString().StartsWith(args.Filter) || p.Name.StartsWith(args.Filter))
                .OrderByDescending(p => p.PageID)
                .Skip(skip).Take(take).ToList();

            var ret = preModel.Select(t => new PageDatum()
            {
                PageID = t.PageID,
                Name = t.Name
            }).ToList();

            return ret;
        }

        public static object PageSectionCloneGetPageByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                var preModel = DB.Pages.Where(t => t.PageID == Convert.ToInt32(args.Value) || t.Name == args.Value.ToString()).ToList();

                var ret = preModel
                    .Select(t => new PageDatum()
                    {
                        PageID = t.PageID,
                        Name = t.Name
                    }).ToList();

                return ret.ToList();
            }
            else
            {
                return null;
            }
        }

        public static object GetProductTypesForComboBox(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var preModel = DB.ProductTypes
                .Where(p => p.ProductTypeID.ToString().StartsWith(args.Filter) || p.Name.StartsWith(args.Filter))
                .OrderBy(p => p.Name)
                .Skip(skip).Take(take).ToList();

            var ret = preModel.Select(t => new ProductTypeDatum()
            {
                ProductTypeID = t.ProductTypeID,
                Name = t.Name,
                URLKeywords = t.URLKeywords
            }).ToList();

            return ret;
        }

        public static object GetProductTypeByIDForComboBox(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                var ptid = (byte?)args.Value;

                var preModel = DB.ProductTypes.Where(t => t.ProductTypeID == ptid).ToList();

                var ret = preModel
                    .Select(t => new ProductTypeDatum()
                    {
                        ProductTypeID = t.ProductTypeID,
                        Name = t.Name,
                        URLKeywords = t.URLKeywords
                    }).ToList();

                return ret.ToList();
            }
            else
            {
                return null;
            }
        }

        public static object GetFontClasses(string purpose)
        {
            switch(purpose)
            {
                case "menu":
                    return DB.FontClasses.Where(p => p.Deleted == false && p.MenuStyle == true).ToList();
                case "banner":
                    return DB.FontClasses.Where(p => p.Deleted == false && p.BannerStyle == true).ToList();
                default:
                    return DB.FontClasses.Where(p => p.Deleted == false).ToList();
            }
        }

        public static object GetBannerLocationsForMenuBuilder()
        {
            return DB.BannerLocations.Where(p => p.BannerLocationGroupID == 8).ToList();
        }

        public static object GetNullableLanguages()
        {
            var preModel = DB.Languages.Where(p => p.DeletedDT == null).ToList();
            var Model = new List<NullableLanguage>();
            Model.Add(new NullableLanguage() { LanguageID = null, Name = "Not Set..." });

            foreach (var i in preModel)
                Model.Add(new NullableLanguage() { LanguageID = i.LanguageID, Name = i.Name });

            return Model;
        }

        public class NullableLanguage
        {
            public short? LanguageID { get; set; }
            public string Name { get; set; }
        }

        public static object GetStockLocations()
        {
            return DB.StockLocations.ToList();
        }

        public static object GetNullableCompetitions()
        {
            var preModel = DB.Competitions.Where(p => p.DeletedDT == null && (p.OffDate == null || p.OffDate > DateTime.Now)).OrderByDescending(p => p.CompetitionID).ToList();
            var Model = new List<NullableCompetition>();
            Model.Add(new NullableCompetition() { CompetitionID = null, Name = "Not Set..." });

            foreach (var i in preModel)
                Model.Add(new NullableCompetition() { CompetitionID = i.CompetitionID, Name = "ID: " + i.CompetitionID.ToString() + " - " + i.Name });

            return Model;
        }

        public class NullableCompetition
        {
            public short? CompetitionID { get; set; }
            public string Name { get; set; }
        }


        public static object GetNullableThirdParties()
        {
            var preModel = DB.ThirdParties.OrderByDescending(p => p.ThirdPartyID).ToList();
            var Model = new List<NullableThirdParty>();
            Model.Add(new NullableThirdParty() { ThirdPartyID = null, Name = "Not Set..." });

            foreach (var i in preModel)
                Model.Add(new NullableThirdParty() { ThirdPartyID = i.ThirdPartyID, Name = "ID: " + i.ThirdPartyID.ToString() + " - " + i.Name });

            return Model;
        }

        public class NullableThirdParty
        {
            public short? ThirdPartyID { get; set; }
            public string Name { get; set; }
        }

        public static object GetNullableBrochures()
        {
            var preModel = DB.Brochures.Where(p => p.DeletedDT == null).ToList();
            var Model = new List<NullableBrochure>();
            Model.Add(new NullableBrochure() { BrochureID = null, Name = "Not Set" });

            foreach (var i in preModel)
                Model.Add(new NullableBrochure() { BrochureID = i.BrochureID, Name = i.Name });

            return Model;
        }

        public class NullableBrochure
        {
            public short? BrochureID { get; set; }
            public string Name { get; set; }
        }

        public static object GetNullableStoreEvents()
        {
            var preModel = DB.StoreEvents.Where(p => p.DeletedDT == null).ToList();
            var Model = new List<NullableStoreEvent>();
            Model.Add(new NullableStoreEvent() { StoreEventID = null, Name = "Not Set" });

            foreach (var i in preModel)
                Model.Add(new NullableStoreEvent() { StoreEventID = i.StoreEventID, Name = i.Name });

            return Model;
        }

        public class NullableStoreEvent
        {
            public short? StoreEventID { get; set; }
            public string Name { get; set; }
        }
 
        public static object GetNullableSources()
        {
            var preModel = DB.Sources.Where(p => p.Deleted == false).OrderBy(p => p.SourceID).ToList();

            var Model = new List<NullableSource>();

            Model.Add(new NullableSource() { SourceID = null, Name = "Please Select" });

            foreach (var s in preModel)
                Model.Add(new NullableSource() { SourceID = s.SourceID, Name = s.Name });

            return Model;
        }

        public class NullableSource
        {
            public byte? SourceID { get; set; }
            public string Name { get; set; }
        }

        public static IEnumerable<Source> GetSources(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return DB.Sources.Where(p => p.Deleted == false).OrderBy(p => p.SourceID).ToList();
        }

        public static IEnumerable<Source> GetSourceByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
                return DB.Sources.Where(p => p.SourceID == (byte)args.Value).ToList();
            else
                return DB.Sources.Where(p => p.Deleted == false).OrderBy(p => p.SourceID).ToList();
        }
        
        public static object GetPromotionGroups(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return DB.PromotionGroups.ToList();
        }

        public static object GetPromotionGroupByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
                return DB.PromotionGroups.Where(p => p.PromotionGroupID == (byte)args.Value).ToList();
            else
                return DB.PromotionGroups.ToList();
        }

        public static IEnumerable<PromotionCode> GetPromotionCodes(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var preModel = DB.PromotionCodes.Where(p => p.DeletedDT == null).ToList();

            var model = preModel.Where(p => p.Code.StartsWith(args.Filter) || p.Description.Contains(args.Filter)).OrderByDescending(p => p.CreatedDT).ToList();

            return model;
        }

        public static IEnumerable<PromotionCode> GetPromotionCodeByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
                return DB.PromotionCodes.Where(p => p.PromotionCodeID == (short)args.Value).ToList();
            else
                return DB.PromotionCodes.ToList();
        }


        public static object GetBrands(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var Model = DB.Brands.ToList();
            return Model;
        }

        public static object GetBrandByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
                return DB.Brands.Where(p => p.BrandID == (byte)args.Value).ToList();
            else
                return DB.Brands.ToList();
        }

        public static object GetLaunches(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var Model = DB.Launches.ToList();
            return Model;
        }

        public static object GetLaunchByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
                return DB.Launches.Where(p => p.LaunchID == (short)args.Value).ToList();
            else
                return DB.Launches.ToList();
        }

        public static object GetStockistCountries(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.StockistCountries
                .Skip(skip).Take(take).ToList();

            return Model;
        }

        public static object GetStockistCountryByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                string cc = args.Value.ToString();
                return (from p in DB.StockistCountries
                        where p.CountryCode == cc
                        select p).Take(1);
            }
            return null;
        }

        public static List<NullableSlideshowOverlayPosition> GetSlideshowOverlayPositions()
        {
            var preModel = DB.SlideshowOverlayPositions.ToList();

            var Model = new List<NullableSlideshowOverlayPosition>();
            Model.Add(new NullableSlideshowOverlayPosition() { 
                SlideshowOverlayPositionID = null, 
                Name = "Select...",
                OverlayTop = null,
                OverlayLeft = null,
                OverlayWidth = null,
                OverlayHeight = null
            });

            foreach(var s in preModel)
            {
                Model.Add(new NullableSlideshowOverlayPosition() { 
                    SlideshowOverlayPositionID = s.SlideshowOverlayPositionID, 
                    Name = s.Name,
                    OverlayTop = s.OverlayTop,
                    OverlayLeft = s.OverlayLeft,
                    OverlayWidth = s.OverlayWidth,
                    OverlayHeight = s.OverlayHeight 
                });
            }

            return Model;
        }

        public class NullableSlideshowOverlayPosition
        {
            public byte? SlideshowOverlayPositionID { get; set; }

            public string Name { get; set; }

            public decimal? OverlayTop { get; set; }
            public decimal? OverlayLeft { get; set; }
            public decimal? OverlayWidth { get; set; }
            public decimal? OverlayHeight { get; set; }
        }



        public static object GetSnippetLocations(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var PreModel = DB.SnippetLocations.ToList();

            if(args != null)
            {
                var skip = args.BeginIndex;
                var take = args.EndIndex - args.BeginIndex + 1;
                PreModel = DB.SnippetLocations.Skip(skip).Take(take).ToList();
            }

            var Model = new List<SnippetLocationExt>();

            foreach (var m in PreModel)
                Model.Add(new SnippetLocationExt() { SnippetLocationID = m.SnippetLocationID, Name = m.SnippetLocationID.ToString() + ": " + m.Name });

            return Model;
        }

        public class SnippetLocationExt
        {
            public short SnippetLocationID { get; set; }
            public string Name { get; set; }
        }

        public static object GetSnippetLocationByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                byte id = (byte)args.Value;
                return (from p in DB.SnippetLocations
                        where p.SnippetLocationID == id
                        select p).Take(1);
            }
            return null;
        }

        public static object GetPriceLists(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.PriceLists
                .Skip(skip).Take(take).ToList();

            return Model;
        }

        public static object GetPriceListByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                int id = (int)args.Value;
                return (from p in DB.PriceLists
                        where p.PriceListID == id
                        select p).Take(1);
            }
            return null;
        }

        public static object GetProductTypes(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            if(args != null)
            { 
                var skip = args.BeginIndex;
                var take = args.EndIndex - args.BeginIndex + 1;

                var Model = DB.ProductTypes
                    .Skip(skip).Take(take).ToList();

                return Model;
            }
            else
            {
                return DB.ProductTypes.ToList();
            }
        }

        public static object GetProductTypeByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                int id = (int)args.Value;
                return (from p in DB.ProductTypes
                        where p.ProductTypeID == id
                        select p).Take(1);
            }
            return null;
        }

        public static object GetLanguagesWithCountry()
        {
            var PreModel = DB.Country_Languages.Select(p => p.LanguageID).ToList();
            var Model = DB.Languages
                .Where(p => PreModel.Contains(p.LanguageID)).ToList();
            
            return Model;
        }

        public static object GetLanguages(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.Languages
                .Skip(skip).Take(take).ToList();

            return Model;
        }

        public static object GetLanguageByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                int id = (int)args.Value;
                return (from p in DB.Languages
                        where p.LanguageID == id
                        select p).Take(1);
            }
            return null;
        }

        public static object GetDeliveryBands()
        {
            return DB.DeliveryBands.ToList();
        }

        public static object GetDeliveryBandsForPriceList(byte priceListID)
        {
            var t = DB.DeliveryBands.Where(p => p.PriceListID == priceListID && p.DeletedDT == null).ToList();
            var y = new List<DeliveryBandExt>();
            foreach(var db in t)
            {
                string name = $"{db.DeliveryBandID} - {db.Name} - {db.DisplayName} [Courier: {db.Courier.DisplayName}]";
                y.Add(new DeliveryBandExt() { DeliveryBandID = db.DeliveryBandID, Name = name });
            }

            return y;//DB.DeliveryBands.Where(p => p.PriceListID == priceListID).ToList();
        }

        public class DeliveryBandExt
        {
            public short DeliveryBandID { get; set; }

            public string Name { get; set; }
        }


        public static object GetMemberSegments()
        {
            return DB.MemberSegments.ToList();
        }

        public static object GetDeliveryServices(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.DeliveryServices
                .Skip(skip).Take(take).ToList();

            return Model;
        }

        public static object GetDeliveryServiceByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                int id = (int)args.Value;
                return (from p in DB.DeliveryServices
                        where p.DeliveryServiceID == id
                        select p).Take(1);
            }
            return null;
        }

        public static object GetDeliveryTypes(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.DeliveryTypes
                .Skip(skip).Take(take).ToList();

            return Model;
        }

        public static object GetDeliveryTypeByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                int id = (int)args.Value;
                return (from p in DB.DeliveryTypes
                        where p.DeliveryTypeID == id
                        select p).Take(1);
            }
            return null;
        }


        public static object GetCouriers(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.Couriers
                .OrderBy(p => p.Name)
                .Skip(skip).Take(take).ToList();

            return Model;
        }

        public static object GetCourierByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                int id = (int)args.Value;
                return (from p in DB.Couriers
                        where p.CourierID == id
                        select p).Take(1);
            }
            return null;
        }

        public static object GetCountryWithPriceList(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.Countries
                .Where(p => p.PriceListID != null)
                .OrderBy(p => p.Name)
                .Skip(skip).Take(take).ToList();

            return Model;
        }

        public static object GetCountryWithPriceListByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                short id = (short)args.Value;

                return (from p in DB.Countries
                        where p.CountryID == id
                        select p).Take(1);
            }
            return null;
        }

        public static object GetDeliveryOptions(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.DeliveryOptions
                .Where(p => p.DeletedDT == null)
                .OrderBy(p => p.SortOrder)
                .Skip(skip).Take(take).ToList();

            return Model;
        }

        public static object GetDeliveryOptionsByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                byte id = (byte)args.Value;

                return (from p in DB.DeliveryOptions
                        where p.DeliveryOptionID == id
                        select p).Take(1);
            }
            return null;
        }

        public static object GetCountries(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.Countries
                .Join(DB.Country_Languages,
                  sc => sc.CountryID,
                  soc => soc.CountryID,
                  (sc, soc) => new { sc, soc })
                .Where(p => p.sc.DeletedDT == null)
                .Select(p => p.sc)
                .OrderBy(p => p.Name)
                .Skip(skip).Take(take).ToList();

            return Model;
        }

        public static object GetCountryByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                int id = (int)args.Value;
                return (from p in DB.Countries
                        where p.CountryID == id
                        select p).Take(1);
            }
            return null;
        }

        public static object GetMailingListThirdParties()
        {
            return DB.ThirdParties.ToList();
        }

        public static object GetCountriesForMailingList()
        {
            var Model = DB.Countries
                .Join(DB.Country_Languages,
                  sc => sc.CountryID,
                  soc => soc.CountryID,
                  (sc, soc) => new { sc, soc })
                .Where(p => p.sc.DeletedDT == null)
                .Select(p => p.sc)
                .OrderBy(p => p.Name).ToList();
            Model.Add(new Country(){ CountryID = 0, Name = "Global" });
            return Model;
        }

        public static object GetDotDigitalAccounts()
        {
            var Model = DB.Accounts
                .Where(p => p.AccountID > 0)
                .OrderBy(p => p.AccountID).ToList();
            return Model;
        }

        public static object BrochureHelper(ListEditItemsRequestedByFilterConditionEventArgs args, object value)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.Brochures
                .Where(p => p.DeletedDT == null)
                .OrderBy(p => p.Name)
                .Skip(skip).Take(take).ToList();

            if (value != null)
            {
                if (Model.Where(p => p.BrochureID == Convert.ToInt16(value)).SingleOrDefault() == null)
                    Model.Add(DB.Brochures.Where(p => p.BrochureID == Convert.ToInt16(value)).Single());
            }

            return Model;
        }

        public static object VideoHelper(ListEditItemsRequestedByFilterConditionEventArgs args, object value)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.Videos
                .Where(p => p.DeletedDT == null)
                .OrderBy(p => p.Name)
                .Skip(skip).Take(take).ToList();

            if (value != null)
            {
                if (Model.Where(p => p.VideoID == Convert.ToInt16(value)).SingleOrDefault() == null)
                    Model.Add(DB.Videos.Where(p => p.VideoID == Convert.ToInt16(value)).Single());
            }

            return Model;
        }

        public static object GetVideoByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                int id = (int)args.Value;
                return (from p in DB.Videos
                        where p.VideoID == id
                        select p).Take(1);
            }
            return null;
        }


        public static object GetRequestedBrochures()
        {
            var Linker = DB.Member_Brochures.Where(p => p.CreatedDT > DateTime.Now.AddMonths(-6) && p.DeletedDT == null).Select(p => p.BrochureID).ToList();

            return DB.Brochures.InRange(p => p.BrochureID, 2000, Linker).ToList();
        }

        public static object GetBrochureList(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.Brochures
                .Where(p => p.DeletedDT == null)
                .OrderBy(p => p.Name)
                .Skip(skip).Take(take).ToList();
            return Model;
        }


        public static object GetBrochureByID(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                int id = (int)args.Value;
                return (from p in DB.Brochures
                        where p.BrochureID == id
                        select p).Take(1);
            }
            return null;
        }


        public static object GetProductPageListTextFiltered(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.vwProductPages.Where(p => p.Name.StartsWith(args.Filter) && p.DeletedDT == null)
                .Where(p => HttpContext.Current.Session["ProductPageAttributeProductTypeID"] == null || p.ProductTypeID == Convert.ToByte(HttpContext.Current.Session["ProductPageAttributeProductTypeID"]))
                .Where(p => HttpContext.Current.Session["ProductPageAttributeBrandID"] == null || p.BrandID == Convert.ToByte(HttpContext.Current.Session["ProductPageAttributeBrandID"]))
                .Where(p => HttpContext.Current.Session["ProductPageAttributeLaunchID"] == null || p.LaunchID == Convert.ToInt16(HttpContext.Current.Session["ProductPageAttributeLaunchID"]))
                .Where(p => HttpContext.Current.Session["ProductPageAttributeWebStatusID"] == null || p.StatusID == Convert.ToByte(HttpContext.Current.Session["ProductPageAttributeWebStatusID"]))
                .Where(p => HttpContext.Current.Session["ProductPageAttributeTemplateID"] == null || p.TemplateID == Convert.ToByte(HttpContext.Current.Session["ProductPageAttributeTemplateID"]))
                .Skip(skip).Take(take).ToList();

            return Model;
        }


        public static object GetProductPageList(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var Model = DB.vwProductPages
                .Where(p => p.DeletedDT == null && p.Name.StartsWith(args.Filter))
                .Where(p => HttpContext.Current.Session["ComposeProductPageProductTypeID"] == null || p.ProductTypeID == Convert.ToByte(HttpContext.Current.Session["ComposeProductPageProductTypeID"]))
                .Where(p => HttpContext.Current.Session["ComposeProductPageBrandID"] == null || p.BrandID == Convert.ToByte(HttpContext.Current.Session["ComposeProductPageBrandID"]))
                .Where(p => HttpContext.Current.Session["ComposeProductPageLaunchID"] == null || p.LaunchID == Convert.ToInt16(HttpContext.Current.Session["ComposeProductPageLaunchID"]))
                .Where(p => HttpContext.Current.Session["ComposeProductPageWebStatusID"] == null || p.StatusID == Convert.ToByte(HttpContext.Current.Session["ComposeProductPageWebStatusID"]))
                .Where(p => HttpContext.Current.Session["ComposeProductPageTemplateID"] == null || p.TemplateID == Convert.ToByte(HttpContext.Current.Session["ComposeProductPageTemplateID"]))
                .Skip(skip).Take(take).ToList();

            return Model;
        }




        public static object GetProductPageRange(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var PreModel = DB.vwProductPageTranslates//.Where(p => p.Name.StartsWith(args.Filter))
                .Where(p => HttpContext.Current.Session["TranslateProductPageProductTypeID"] == null || p.ProductTypeID == Convert.ToByte(HttpContext.Current.Session["TranslateProductPageProductTypeID"]))
                .Where(p => HttpContext.Current.Session["TranslateProductPageBrandID"] == null || p.BrandID == Convert.ToByte(HttpContext.Current.Session["TranslateProductPageBrandID"]))
                .Where(p => HttpContext.Current.Session["TranslateProductPageLaunchID"] == null || p.LaunchID == Convert.ToInt16(HttpContext.Current.Session["TranslateProductPageLaunchID"]))
                .Where(p => HttpContext.Current.Session["TranslateProductPageTemplateID"] == null || p.TemplateID == Convert.ToByte(HttpContext.Current.Session["TranslateProductPageTemplateID"]))
                .Where(p => HttpContext.Current.Session["TranslateProductPageWebStatusID"] == null || p.WebStatus == Convert.ToByte(HttpContext.Current.Session["TranslateProductPageWebStatusID"]))
                //.Where(p => HttpContext.Current.Session["TranslateProductPageTranslationStatusID"] == null || p.TranslationStatus == Convert.ToByte(HttpContext.Current.Session["TranslateProductPageTranslationStatusID"]))
                .Where(p => HttpContext.Current.Session["TranslateProductPageTargetLanguageID"] == null || p.LanguageID == Convert.ToByte(HttpContext.Current.Session["TranslateProductPageTargetLanguageID"]))
                .GroupBy(p => p.ProductPageID, (key, productpages) => new { ProductPageID = key, Values = productpages.ToList() })
                .Skip(skip).Take(take).ToList();

            byte index = 1; // default to English

            if (HttpContext.Current.Session["TranslateProductPageTargetLanguageID"] != null)
                index = Convert.ToByte(HttpContext.Current.Session["TranslateProductPageTargetLanguageID"]); // unless there's a target language set

            var Model = new List<vwProductPageTranslate>();

            foreach (var i in PreModel)
            {
                var vwPPT = i.Values.Where(p => p.LanguageID == index).SingleOrDefault();
                if (vwPPT != null)
                    Model.Add(vwPPT);
            }

            return Model;
        }

        public static object GetPageRange(ListEditItemsRequestedByFilterConditionEventArgs args, object value)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;

            var PreModel = DB.vwProductPageTranslates//.Where(p => p.Name.StartsWith(args.Filter))
                .Where(p => HttpContext.Current.Session["TranslateProductPageProductTypeID"] == null || p.ProductTypeID == Convert.ToByte(HttpContext.Current.Session["TranslateProductPageProductTypeID"]))
                .Where(p => HttpContext.Current.Session["TranslateProductPageBrandID"] == null || p.BrandID == Convert.ToByte(HttpContext.Current.Session["TranslateProductPageBrandID"]))
                .Where(p => HttpContext.Current.Session["TranslateProductPageLaunchID"] == null || p.LaunchID == Convert.ToInt16(HttpContext.Current.Session["TranslateProductPageLaunchID"]))
                .Where(p => HttpContext.Current.Session["TranslateProductPageTemplateID"] == null || p.TemplateID == Convert.ToByte(HttpContext.Current.Session["TranslateProductPageTemplateID"]))
                .Where(p => HttpContext.Current.Session["TranslateProductPageWebStatusID"] == null || p.WebStatus == Convert.ToByte(HttpContext.Current.Session["TranslateProductPageWebStatusID"]))
                //.Where(p => HttpContext.Current.Session["TranslateProductPageTranslationStatusID"] == null || p.TranslationStatus == Convert.ToByte(HttpContext.Current.Session["TranslateProductPageTranslationStatusID"]))
                .Where(p => HttpContext.Current.Session["TranslateProductPageTargetLanguageID"] == null || p.LanguageID == Convert.ToByte(HttpContext.Current.Session["TranslateProductPageTargetLanguageID"]))
                .GroupBy(p => p.ProductPageID, (key, productpages) => new { ProductPageID = key, Values = productpages.ToList() })
                .Skip(skip).Take(take).ToList();


            //if (value != null)
            //{
            //    if (PreModel.Where(p => p.ProductPageID == Convert.ToInt32(value)).SingleOrDefault() == null)
            //        PreModel.Add(DB.vwProductPageTranslates.Where(p => p.ProductPageID == Convert.ToInt32(value)).Single());
            //}


            byte index = 1; // default to English

            if (HttpContext.Current.Session["TranslateProductPageTargetLanguageID"] != null)
                index = Convert.ToByte(HttpContext.Current.Session["TranslateProductPageTargetLanguageID"]); // unless there's a target language set

            var Model = new List<vwProductPageTranslate>();

            foreach (var i in PreModel)
            {
                var vwPPT = i.Values.Where(p => p.LanguageID == index).SingleOrDefault();
                if (vwPPT != null)
                    Model.Add(vwPPT);
            }



            return Model;
        }

        public static object GetProductPageByIDForAttributes(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value != null)
            {
                int id = (int)args.Value;
                return (from p in DB.vwProductPageTranslates
                        where p.ProductPageID == id
                        select p).Take(1);
            }
            return null;
        }
    }
}

namespace Flock.Models
{
    public partial class Country_Language
    {
        public string LanguageText { get; set; }
    }

    public class PassCountryLanguage
    {
        public short CountryID { get; set; }
        public byte LanguageID { get; set; }
        public string Name { get; set; }
    }
}








#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion




#if NO_HASHSET
using System.Linq;
#endif


namespace MoreLinq
{
    using System;
    using System.Collections.Generic;


    static class MoreEnumerable
    {
        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the default eqaulity comparer for the projected type.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.</returns>

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }


        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the specified comparer for the projected type.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.</returns>

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            return DistinctByImpl(source, keySelector, comparer);
        }


        private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>(IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
#if !NO_HASHSET
            var knownKeys = new HashSet<TKey>(comparer);
            foreach (var element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
#else
            //
            // On platforms where LINQ is available but no HashSet<T>
            // (like on Silverlight), implement this operator using 
            // existing LINQ operators. Using GroupBy is slightly less
            // efficient since it has do all the grouping work before
            // it can start to yield any one element from the source.
            //


            return source.GroupBy(keySelector, comparer).Select(g => g.First());
#endif
        }
    }
}