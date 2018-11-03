namespace TheCollection.Import.Console.Translators {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NodaTime;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Tea;
    using TheCollection.Import.Console.Models;

    public class TheeToBagTranslator : ITranslator<Thee, Bag> {
        public IList<Country> Countries { get; }
        public IList<Brand> Brands { get; }
        public IList<BagType> BagTypes { get; }
        public IList<Image> Images { get; }

        public TheeToBagTranslator(IList<Country> countries, IList<Brand> brands, IList<BagType> bagTypes, IList<Image> images) {
            Countries = countries;
            Brands = brands;
            BagTypes = bagTypes;
            Images = images;
        }

        public Bag Translate(Thee source) {
            var teabrand = Brands.FirstOrDefault(brand => brand.Name == source.TheeMerk.Trim());
            var teabagType = BagTypes.FirstOrDefault(bagType => bagType.Name == source.TheeSoortzakje.Trim());
            var teacountry = Countries.FirstOrDefault(country => country.Name == source.TheeLandvanherkomst.Trim());

            return new Bag(null,
                "36544cc9-36ae-42c0-b614-a5a2010d4258",
                source.MainID,
                new Domain.RefValue(teabrand.Id, teabrand.Name),
                source.TheeSerie.Trim(),
                source.TheeSmaak.Trim(),
                source.TheeKenmerken.Trim(),
                teabagType != null ? new Domain.RefValue(teabagType.Id, teabagType.Name) : null,
                teacountry != null ? new Domain.RefValue(teacountry.Id, teacountry.Name) : null,
                source.TheeSerienummer.Trim(),
                ParseInvoerDatum(source),
                Images.FirstOrDefault(image => image.Filename == $"{source.MainID}.jpg")?.Id);
        }

        static LocalDate ParseInvoerDatum(Thee source) {
            if (DateTime.TryParse(source.Theeinvoerdatum.Trim(), out var dateTime)) {
                return LocalDate.FromDateTime(dateTime);
            }

            return LocalDate.MinIsoValue;
        }
    }
}
