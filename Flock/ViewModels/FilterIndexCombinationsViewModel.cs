using DevExpress.XtraScheduler.Design;

using Flock.DAL;
using Flock.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class FilterIndexCombinationsViewModel : BaseViewModel
    {
        public byte LanguageID { get; set; }
        public byte? ProductTypeID { get; set; }
        public byte? AttributeGroupID { get; set; }

        public FilterIndexCombinationsViewModel(byte languageId, byte? productTypeId = null, byte? attributeGroupId = null) 
        {
            LanguageID = languageId;
            ProductTypeID = productTypeId;
            AttributeGroupID = attributeGroupId;
        }
    }

    public class FilterIndexCombinationsGridSettings
    {
        public byte? LanguageID { get; set; }

        public string TitleTagSuffix { get; set; }

        public FilterIndexCombinationsGridSettings(byte? languageId)
        {
            LanguageID = languageId;

            var db = new FlockDataContext();

            TitleTagSuffix = db.Country_Languages.Single(cl => cl.Country_Language_ID == 3).TitleTagSuffix;
        }
    }

    public class FilterIndexCombination_LanguageEditModel
    {
        public string Key { get; set; }

        [StringLength(4000, ErrorMessage = "Cannot exceed 4,000 characters")]
        public string FooterText { get; set; }

        public int FilterIndexCombinationID 
        { 
            get 
            {
                return Convert.ToInt32(Key?.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0]);
            }
        }

        [StringLength(100, ErrorMessage = "Cannot exceed 100 characters")]
        public string TitleTag { get; set; }


        [StringLength(155, ErrorMessage = "Cannot exceed 155 characters")]
        public string MetaDescription { get; set; }

        public byte LanguageID
        {
            get
            {
                return Convert.ToByte(Key?.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            }
        }
    }
}