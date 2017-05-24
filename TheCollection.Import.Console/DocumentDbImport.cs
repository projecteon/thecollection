using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TheCollection.Import.Console.Models;
using TheCollection.Web.Models;
using TheCollection.Web.Services;

namespace TheCollection.Import.Console
{
    public class DocumentDbImport
    {
        public static IList<Brand> ImportBrands(DocumentClient client, string collection, List<Merk> meerken)
        {
            var brands = meerken.Select(merk =>
            {
                return new Brand
                {
                    Name = merk.TheeMerk.Trim()
                };
            }).ToList();

            var brandsRepository = new DocumentDBRepository<Brand>(client, collection, "Brands");
            var insertCounter = 0;
            brands.ForEach(brand =>
            {
                brand.Id = brandsRepository.CreateItemAsync(brand).Result;
                insertCounter++;
                if (insertCounter > 0 && insertCounter % 100 == 0)
                {
                    System.Console.WriteLine($"Inserted brand#: {insertCounter}");
                }
            });

            System.Console.WriteLine($"Inserted {insertCounter} brands");
            return brands;
        }

        public static IEnumerable<Bag> ImportBags(DocumentClient client, string collection, List<Thee> thees, IList<Brand> brands, IImageService imageservice)
        {
            var importThees = thees.Take(1000).ToList();
            var countries = ImportCountries(client, collection, importThees);
            var bagTypes = ImportBagTypes(client, collection, importThees);
            var images = ImportImages(client, collection, importThees, imageservice);

            var bagsRepository = new DocumentDBRepository<Bag>(client, collection, "Bags");
            var bags = importThees.Select(thee =>
            {
                return new Bag
                {
                    MainID = thee.MainID,
                    Brand = brands.FirstOrDefault(brand => brand.Name == thee.TheeMerk.Trim()),
                    Serie = thee.TheeSerie.Trim(),
                    Flavour = thee.TheeSmaak.Trim(),
                    Hallmark = thee.TheeKenmerken.Trim(),
                    Type = bagTypes.FirstOrDefault(bagType => bagType.Name == thee.TheeSoortzakje.Trim()),
                    Country = countries.FirstOrDefault(country => country.Name == thee.TheeLandvanherkomst.Trim()),
                    SerialNumber = thee.TheeSerienummer.Trim(),
                    InsertDate = thee.Theeinvoerdatum.Trim(),
                    ImageId = images.FirstOrDefault(image => image.Filename == $"{thee.MainID}.jpg").Id
                };
            });

            var insertCounter = 0;
            bags.ToList().ForEach(bag =>
            {
                var bagid = bagsRepository.CreateItemAsync(bag).Result;
                insertCounter++;
                if (insertCounter > 0 && insertCounter % 100 == 0)
                {
                    System.Console.WriteLine($"Inserted bag#: {insertCounter}");
                }
            });

            System.Console.WriteLine($"Inserted {insertCounter} bags");
            return bags;
        }

        private static List<Country> ImportCountries(DocumentClient client, string collection, List<Thee> thees)
        {
            var countries = thees.Select(thee => thee.TheeLandvanherkomst.Trim()).Distinct().Select(country => { return new Country { Name = country }; }).ToList();
            var countryRepository = new DocumentDBRepository<Country>(client, collection, "Countries");
            var insertCounter = 0;
            countries.ForEach(country =>
            {
                country.Id = countryRepository.CreateItemAsync(country).Result;
                insertCounter++;
            });

            System.Console.WriteLine($"Inserted {insertCounter} countries");
            return countries;
        }

        private static List<BagType> ImportBagTypes(DocumentClient client, string collection, List<Thee> thees)
        {
            var bagTypes = thees.Select(thee => thee.TheeSoortzakje.Trim()).Distinct().Where(bagType => bagType.Length > 0).Select(type => { return new BagType { Name = type }; }).ToList();
            var bagTypeRepository = new DocumentDBRepository<BagType>(client, collection, "BagTypes");
            var insertCounter = 0;
            bagTypes.ForEach(bagType =>
            {
                bagType.Id = bagTypeRepository.CreateItemAsync(bagType).Result;
                insertCounter++;
            });

            System.Console.WriteLine($"Inserted {insertCounter} bagtypes");
            return bagTypes;
        }

        private static List<Web.Models.Image> ImportImages(DocumentClient client, string collection, List<Thee> thees, IImageService imageservice)
        {
            var images = thees.Select(thee => { return new Image { Filename = $"{thee.MainID}.jpg" }; }).ToList();
            var imageRepository = new DocumentDBRepository<Image>(client, collection, "Images");
            var insertCounter = 0;
            images.ForEach(async image =>
            {
                var fileImageService = new ImageFilesystemService();
                var bitmap = await fileImageService.Get(image.Filename);
                using (var imageStream = new MemoryStream())
                {
                    bitmap.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    image.Uri = await imageservice.Upload(imageStream, image.Filename);
                }

                image.Id = imageRepository.CreateItemAsync(image).Result;
                insertCounter++;
            });

            System.Console.WriteLine($"Inserted {insertCounter} images");
            return images;
        }
    }
}
