using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class MasonryTestViewModel : BaseViewModel
    {
        public List<MasonryItem> Items { get; set; }
    }

    public class MasonryItem
    {
        public string MasonryClass { get; set; }
        public Flock.Models.Image Image { get; set; }
        public int Width { get; set; }
    }
}