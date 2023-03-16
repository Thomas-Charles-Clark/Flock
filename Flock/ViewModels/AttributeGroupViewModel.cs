using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class AttributeGroupViewModel
    {
        public byte AttributeGroupID { get; set; }
        public string Name { get; set; }
        public bool Filter { get; set; }
        public bool Info { get; set; }
        public bool IsMultipleSelect { get; set; }
        public List<Flock.Models.Attribute_SelectResult> Options { get; set; }
        public List<short> SelectedValues { get; set; }
        public byte SortOrder { get; set; }
        public bool AutoSync { get; set; }
    }
}