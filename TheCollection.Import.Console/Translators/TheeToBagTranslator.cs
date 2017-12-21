namespace TheCollection.Import.Console.Translators {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NodaTime;
    using TheCollection.Domain.Tea;
    using TheCollection.Import.Console.Models;
    using TheCollection.Web.Translators;

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

        public void Translate(Thee source, Bag destination) {
            var teabrand = Brands.FirstOrDefault(brand => brand.Name == source.TheeMerk.Trim());
            var teabagType = BagTypes.FirstOrDefault(bagType => bagType.Name == source.TheeSoortzakje.Trim());
            var teacountry = Countries.FirstOrDefault(country => country.Name == source.TheeLandvanherkomst.Trim());

            destination.MainID = source.MainID;
            destination.Brand = new Domain.RefValue { Id = teabrand.Id, Name = teabrand.Name };
            destination.Serie = source.TheeSerie.Trim();
            destination.Flavour = source.TheeSmaak.Trim();
            destination.Hallmark = source.TheeKenmerken.Trim();
            destination.BagType = teabagType != null ? new Domain.RefValue { Id = teabagType.Id, Name = teabagType.Name } : null;
            destination.Country = teacountry != null ? new Domain.RefValue { Id = teacountry.Id, Name = teacountry.Name } : null;
            destination.SerialNumber = source.TheeSerienummer.Trim();
            destination.InsertDate = ParseInvoerDatum(source);
            destination.ImageId = Images.FirstOrDefault(image => image.Filename == $"{source.MainID}.jpg")?.Id;
            //destination.UserId = "36544cc9-36ae-42c0-b614-a5a2010d4258";
        }

        static LocalDate ParseInvoerDatum(Thee source) {
            if (DateTime.TryParse(source.Theeinvoerdatum.Trim(), out var dateTime)) {
                return LocalDate.FromDateTime(dateTime);
            }

            return LocalDate.MinIsoValue;
        }
    }
}
