using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.Models.ContentItems
{
    public class SearchResultItem : ContentItem
    {
        public string ClassName { get; set; }
        public string ArbitraryContents { get; set; }
    }
}