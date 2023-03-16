using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models.Pages;

namespace Flock.Models.ContentItems
{
    public class GridSectionItem : ContentItem
    {
        public GridSection Record { get; set; }
        public List<ContentItem> Contents { get; set; }
    }
}