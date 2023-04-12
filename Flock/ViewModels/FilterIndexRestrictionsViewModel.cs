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
    public class FilterIndexRestrictionsViewModel : BaseViewModel
    {
        public byte LanguageID { get; set; }
        public byte? ProductTypeID { get; set; }
        public byte? AttributeGroupID { get; set; }

        public FilterIndexRestrictionsViewModel(byte languageId, byte? productTypeId = null, byte? attributeGroupId = null) 
        {
            LanguageID = languageId;
            ProductTypeID = productTypeId;
            AttributeGroupID = attributeGroupId;
        }
    }

    public class FilterIndexRestrictionsGridSettings
    {
        public byte? LanguageID { get; set; }

        public FilterIndexRestrictionsGridSettings(byte? languageId)
        {
            LanguageID = languageId;
        }
    }

    public class FilterIndexRestriction_LanguageEditModel
    {
        public string Key { get; set; }

        [StringLength(4000, ErrorMessage = "Cannot exceed 4,000 characters")]
        public string FooterText { get; set; }

        public int FilterIndexRestrictionID 
        { 
            get 
            {
                return Convert.ToInt32(Key?.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0]);
            } 
        }

        public byte LanguageID
        {
            get
            {
                return Convert.ToByte(Key?.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            }
        }
    }
}