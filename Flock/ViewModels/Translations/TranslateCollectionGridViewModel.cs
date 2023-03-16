using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateCollectionGridViewModel
    {
        public List<Collection_Language_SelectList_CollectionResult> Collections { get; set; }
        public List<Brand_SelectResult> Brands { get; set; }
        public List<Launch_SelectResult> Launches { get; set; }
        public List<ProductType_SelectResult> ProductTypes { get; set; }
        public List<Status_SelectResult> Statuses { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public int CollectionsToTranslateCount { get; set; }

        public TranslateCollectionGridViewModel()
        {

        }

        public TranslateCollectionGridViewModel(List<Collection_Language_SelectList_CollectionResult> collections,
            List<Brand_SelectResult> brands,
            List<Launch_SelectResult> launches,
            List<ProductType_SelectResult> productTypes,
            List<Status_SelectResult> statuses,
            int collectionsToTranslateCount,
            byte? targetLanguageId)
        {
            Collections = collections;
            Brands = brands;
            Launches = launches;
            ProductTypes = productTypes;
            Statuses = statuses;
            CollectionsToTranslateCount = collectionsToTranslateCount;
            TargetLanguageID = targetLanguageId;
        }
    }
}