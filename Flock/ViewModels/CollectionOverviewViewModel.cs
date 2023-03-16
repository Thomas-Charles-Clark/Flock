using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Flock.ViewModels
{
    public class CollectionOverviewViewModel
    {
        public short CollectionID { get; set; }
        
        [StringLength(20, MinimumLength = 0)]
        public string Code { get; set; }

        [StringLength(20, MinimumLength = 0)]
        public string PatternBookCode { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(70, MinimumLength = 5)]
        public string Heading { get; set; }

        [StringLength(150, MinimumLength = 5)]
        public string SubHeading { get; set; }

        [StringLength(2000, MinimumLength = 10)]
        public string Description { get; set; }

        [StringLength(100, MinimumLength = 10)]
        public string URLKeywords { get; set; }

        [StringLength(70, MinimumLength = 5)]
        public string TitleTag { get; set; }

        [StringLength(155, MinimumLength = 5)]
        public string MetaDescription { get; set; }

        public byte BrandID { get; set; }

        public byte ProductTypeID { get; set; }

        public short LaunchID { get; set; }

        public short? BrochureID { get; set; }

        public short? VideoID { get; set; }

        public short TemplateID { get; set; }

        public DateTime CreatedDT { get; set; }

        public DateTime ModifiedDT { get; set; }

        public DateTime? OnDate { get; set; }

        public DateTime? OffDate { get; set; }

        public long? ImageID { get; set; }

        public byte? StatusID { get; set; }

        public int? AssociatedProducts { get; set; }

        public int? RelatedCollections { get; set; }

        public string BrochureName { get; set; }

        public string VideoName { get; set; }

        public string EmbedCode { get; set; }

        public string FilePath { get; set; }

        public string BrandName { get; set; }

        public string LaunchName { get; set; }

        public string ProductTypeName { get; set; }
    }
}