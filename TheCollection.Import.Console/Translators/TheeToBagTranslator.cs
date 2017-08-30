using System.Collections.Generic;
using System.Linq;
using TheCollection.Business.Tea;
using TheCollection.Import.Console.Models;

namespace TheCollection.Import.Console.Translators
{
    public class TheeToBagTranslator : ITranslator<Thee, Bag>
    {
        public IList<Country> Countries { get; }
        public IList<Brand> Brands { get; }
        public IList<BagType> BagTypes { get; }
        public IList<Image> Images { get; }

        public TheeToBagTranslator(IList<Country> countries, IList<Brand> brands, IList<BagType> bagTypes, IList<Image> images)
        {
            Countries = countries;
            Brands = brands;
            BagTypes = bagTypes;
            Images = images;
        }

        public void Translate(Thee source, Bag destination)
        {
            var teabrand = Brands.FirstOrDefault(brand => brand.Name == source.TheeMerk.Trim());
            var teabagType = BagTypes.FirstOrDefault(bagType => bagType.Name == source.TheeSoortzakje.Trim());
            var teacountry = Countries.FirstOrDefault(country => country.Name == source.TheeLandvanherkomst.Trim());

            destination.MainID = source.MainID;
            destination.Brand = teabrand != null ? new Business.RefValue { Id = teabrand.Id, Name = teabrand.Name } : null;
            destination.Serie = source.TheeSerie.Trim();
            destination.Flavour = source.TheeSmaak.Trim();
            destination.Hallmark = source.TheeKenmerken.Trim();
            destination.BagType = teabagType != null ? new Business.RefValue { Id = teabagType.Id, Name = teabagType.Name } : null;
            destination.Country = teacountry != null ? new Business.RefValue { Id = teacountry.Id, Name = teacountry.Name } : null;
            destination.SerialNumber = source.TheeSerienummer.Trim();
            destination.InsertDate = source.Theeinvoerdatum.Trim();
            destination.ImageId = Images.FirstOrDefault(image => image.Filename == $"{source.MainID}.jpg")?.Id;
        }
    }
}
