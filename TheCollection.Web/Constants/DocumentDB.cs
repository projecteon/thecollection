namespace TheCollection.Web.Constants {
    using System;
    using System.Collections.Generic;

    public static class DocumentDB {
        public const string DatabaseId = "TheCollection";

        public static class Collections {
            public const string Statistics = nameof(Statistics);
            public const string Bags = nameof(Bags);
            public const string BagTypes = nameof(BagTypes);
            public const string Brands = nameof(Brands);
            public const string Countries = nameof(Countries);
            public const string Images = nameof(Images);

            public const string AspNetIdentity = nameof(AspNetIdentity);

            public static IDictionary<Type, string> EntityToCollectionMap { get; } = new Dictionary<Type, string>() {
                { typeof(Domain.Tea.Bag), Bags },
                { typeof(Domain.Tea.Brand), Brands },
                { typeof(Domain.Tea.BagType), BagTypes },
                { typeof(Domain.Tea.Country), Countries},
            };
        }
    }
}
