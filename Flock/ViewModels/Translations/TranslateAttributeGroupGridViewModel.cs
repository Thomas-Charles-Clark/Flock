using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateAttributeGroupGridViewModel
    {
        public List<AttributeGroup_Language_SelectList_AttributeGroupResult> AttributeGroups { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateAttributeGroupGridViewModel()
        {
            AttributeGroups = new List<AttributeGroup_Language_SelectList_AttributeGroupResult>();
        }

        public TranslateAttributeGroupGridViewModel(
            List<AttributeGroup_Language_SelectList_AttributeGroupResult> attributeGroups,
            byte? targetCountryLanguageID)
        {
            AttributeGroups = attributeGroups;
            TargetLanguageID = targetCountryLanguageID;
        }
    }
}