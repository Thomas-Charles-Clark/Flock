using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateAttributeGridViewModel
    {
        public List<Attribute_Language_SelectList_AttributeResult> Attributes { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateAttributeGridViewModel()
        {
            Attributes = new List<Attribute_Language_SelectList_AttributeResult>();
        }

        public TranslateAttributeGridViewModel(
            List<Attribute_Language_SelectList_AttributeResult> attributes,
            byte? targetLanguageId)
        {
            Attributes = attributes;
            TargetLanguageID = targetLanguageId;
        }
    }
}