using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCollection.Web.Constants
{
    public class DocumentDB
    {
        public const string DatabaseId = "TheCollection";
        public const string BagsCollectionId = "Bags";
        public const string BagTypesCollectionId = "BagTypes";
        public const string BrandsCollectionId = "Brands";
        public const string CountriesCollectionId = "Countries";
        public const string ImagesCollectionId = "Images";

        public static object Bran { get; internal set; }
    }
}
