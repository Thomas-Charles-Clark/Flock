using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.ViewModels
{
    public class CollectionProductAssociationViewModel
    {
        public int ProductID { get; set; }

        public int? ProductPageID { get; set; }

        public long? SortOrder { get; set; }

        public long? ImageID { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public char Status { get; set; }
    }

    public class CollectionProductAssociationContentsViewModel
    {
        public List<CollectionProductAssociationViewModel> Products { get; set; }
    }
}