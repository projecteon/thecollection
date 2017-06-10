using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TheCollection.Business.Tea;
using TheCollection.Import.Console.Models;
using TheCollection.Web.Services;

namespace TheCollection.Import.Console
{
    public class DocumentDbImport
    {
        public static async System.Threading.Tasks.Task<IList<Brand>> ImportBrandsAsync(DocumentClient client, string collection, List<Merk> meerken)
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
            await brands.ForEachAsync(async brand =>
            {
                brand.Id = await brandsRepository.CreateItemAsync(brand);
                insertCounter++;
                if (insertCounter > 0 && insertCounter % 100 == 0)
                {
                    System.Console.WriteLine($"Inserted brand#: {insertCounter}");
                }
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} brands");
            return brands;
        }

        public static async System.Threading.Tasks.Task<IEnumerable<Bag>> ImportBagsAsync(DocumentClient client, string collection, List<Thee> thees, IList<Brand> brands, IImageService imageservice)
        {
            var countries = await ImportCountriesAsync(client, collection, thees);
            var bagTypes = await ImportBagTypesAsync(client, collection, thees);
            var images = await ImportImagesAsync(client, collection, thees, imageservice);
            //var images = new List<Image>();

            var bagsRepository = new DocumentDBRepository<Bag>(client, collection, "Bags");
            var bags = thees.Select(thee =>
            {
                var teabrand = brands.FirstOrDefault(brand => brand.Name == thee.TheeMerk.Trim());
                var teabagType = bagTypes.FirstOrDefault(bagType => bagType.Name == thee.TheeSoortzakje.Trim());
                var teacountry = countries.FirstOrDefault(country => country.Name == thee.TheeLandvanherkomst.Trim());
                return new Bag
                {
                    MainID = thee.MainID,
                    Brand = teabrand != null ? new Business.RefValue { Id = teabrand.Id, Name = teabrand.Name } : null,
                    Serie = thee.TheeSerie.Trim(),
                    Flavour = thee.TheeSmaak.Trim(),
                    Hallmark = thee.TheeKenmerken.Trim(),
                    BagType = teabagType != null ? new Business.RefValue { Id = teabagType.Id, Name = teabagType.Name } : null,
                    Country = teacountry != null ? new Business.RefValue { Id = teacountry.Id, Name = teacountry.Name } : null,
                    SerialNumber = thee.TheeSerienummer.Trim(),
                    InsertDate = thee.Theeinvoerdatum.Trim(),
                    ImageId = images.FirstOrDefault(image => image.Filename == $"{thee.MainID}.jpg")?.Id
                };
            });

            System.Console.WriteLine($"Attempting to insert {bags.Count()} bags out of {thees.Count()} thees");
            var insertCounter = 0;
            await bags.ToList().ForEachAsync(async bag =>
            {
                var bagid = await bagsRepository.CreateItemAsync(bag);
                insertCounter++;
                if (insertCounter > 0 && insertCounter % 100 == 0)
                {
                    System.Console.WriteLine($"Inserted bag#: {insertCounter}");
                    await System.Threading.Tasks.Task.Delay(3000);
                }
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} bags");
            return bags;
        }

        private static async System.Threading.Tasks.Task<List<Country>> ImportCountriesAsync(DocumentClient client, string collection, List<Thee> thees)
        {
            var countries = thees.Select(thee => thee.TheeLandvanherkomst.Trim()).Distinct().Where(country => country.Length > 0).Select(country => { return new Country { Name = country }; }).ToList();
            var countryRepository = new DocumentDBRepository<Country>(client, collection, "Countries");
            var insertCounter = 0;
            await countries.ForEachAsync(async country =>
            {
                country.Id = await countryRepository.CreateItemAsync(country);
                insertCounter++;
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} countries");
            return countries;
        }

        private static async System.Threading.Tasks.Task<List<BagType>> ImportBagTypesAsync(DocumentClient client, string collection, List<Thee> thees)
        {
            var bagTypes = thees.Select(thee => thee.TheeSoortzakje.Trim()).Distinct().Where(bagType => bagType.Length > 0).Select(type => { return new BagType { Name = type }; }).ToList();
            var bagTypeRepository = new DocumentDBRepository<BagType>(client, collection, "BagTypes");
            var insertCounter = 0;
            await bagTypes.ForEachAsync(async bagType =>
            {
                bagType.Id = await bagTypeRepository.CreateItemAsync(bagType);
                insertCounter++;
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} bagtypes");
            return bagTypes;
        }

        private static async System.Threading.Tasks.Task<List<Image>> ImportImagesAsync(DocumentClient client, string collection, List<Thee> thees, IImageService imageservice)
        {
            var images = thees.Where(thee => File.Exists($"{ImageFilesystemService.Path}{thee.MainID}.jpg")).Select(thee => { return new Image { Filename = $"{thee.MainID}.jpg" }; }).ToList();
            var imageRepository = new DocumentDBRepository<Image>(client, collection, "Images");
            var insertCounter = 0;
            await images.ForEachAsync(async image =>
            {
                var fileImageService = new ImageFilesystemService();
                var bitmap = await fileImageService.Get(image.Filename);
                using (var imageStream = new MemoryStream())
                {
                    bitmap.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    imageStream.Seek(0, SeekOrigin.Begin);
                    image.Uri = await imageservice.Upload(imageStream, image.Filename);
                }

                image.Id = imageRepository.CreateItemAsync(image).Result;
                insertCounter++;
                if (insertCounter > 0 && insertCounter % 25 == 0)
                {
                    System.Console.WriteLine($"Inserted image#: {insertCounter}");
                    await System.Threading.Tasks.Task.Delay(3000);
                }
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} images");
            return images;
        }
    }
}
