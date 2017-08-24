using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TheCollection.Business.Tea;
using TheCollection.Import.Console.Models;
using TheCollection.Web.Constants;
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

            var brandsRepository = new CreateRepository<Brand>(client, DocumentDB.DatabaseId, DocumentDB.BrandsCollectionId);
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

        public static async System.Threading.Tasks.Task<IEnumerable<Bag>> ImportBagsAsync(DocumentClient client, string collection, List<Thee> thees, IList<Brand> brands)
        {
            var countries = await ImportCountriesAsync(client, collection, thees);
            var bagTypes = await ImportBagTypesAsync(client, collection, thees);
            var images = await ImportImagesAsync(client, collection, thees);

            var bagsRepository = new CreateRepository<Bag>(client, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
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
            }).ToList();

            System.Console.WriteLine($"Attempting to insert {bags.Count()} bags out of {thees.Count()} thees");
            var insertCounter = 0;
            await bags.ToList().ForEachAsync(async bag =>
            {
                var bagid = await bagsRepository.CreateItemAsync(bag);
                insertCounter++;
                if (insertCounter > 0 && insertCounter % 100 == 0)
                {
                    System.Console.WriteLine($"Inserted bag#: {insertCounter}");
                }
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} bags");
            return bags;
        }

        private static async System.Threading.Tasks.Task<List<Country>> ImportCountriesAsync(DocumentClient client, string collection, List<Thee> thees)
        {
            var countries = thees.Select(thee => thee.TheeLandvanherkomst.Trim()).Distinct().Where(country => country.Length > 0).Select(country => { return new Country { Name = country }; }).ToList();
            var countryRepository = new CreateRepository<Country>(client, DocumentDB.DatabaseId, DocumentDB.CountriesCollectionId);
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
            var bagTypeRepository = new CreateRepository<BagType>(client, DocumentDB.DatabaseId, DocumentDB.BagTypesCollectionId);
            var insertCounter = 0;
            await bagTypes.ForEachAsync(async bagType =>
            {
                bagType.Id = await bagTypeRepository.CreateItemAsync(bagType);
                insertCounter++;
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} bagtypes");
            return bagTypes;
        }

        private static async System.Threading.Tasks.Task<List<Image>> ImportImagesAsync(DocumentClient client, string collection, List<Thee> thees)
        {
            var images = thees.Where(thee => File.Exists($"{ImageFilesystemService.Path}{thee.MainID}.jpg")).Select(thee => { return new Image { Filename = $"{thee.MainID}.jpg" }; }).ToList();
            var imageRepository = new CreateRepository<Image>(client, DocumentDB.DatabaseId, DocumentDB.ImagesCollectionId);
            var insertCounter = 0;
            await images.ForEachAsync(async image =>
            {
                image.Id = await imageRepository.CreateItemAsync(image);
                insertCounter++;
                if (insertCounter > 0 && insertCounter % 250 == 0)
                {
                    System.Console.WriteLine($"Inserted image#: {insertCounter}");
                }
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} images");
            return images;
        }

        private static async System.Threading.Tasks.Task<List<Image>> ImportImages2Async(DocumentClient client, string collection, IEnumerable<Bag> bags, IImageService imageservice)
        {
            var images = bags.Where(bag => File.Exists($"{ImageFilesystemService.Path}{bag.MainID}.jpg")).Select(thee => { return new Image { Filename = $"{thee.MainID}.jpg" }; }).ToList();
            var imageRepository = new CreateRepository<Image>(client, DocumentDB.DatabaseId, DocumentDB.ImagesCollectionId);
            var insertCounter = 0;
            await images.ForEachAsync(async image =>
            {
                var fileImageService = new ImageFilesystemService();
                using (var bitmap = await fileImageService.Get(image.Filename))
                {
                    using (var imageStream = new MemoryStream())
                    {
                        bitmap.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imageStream.Seek(0, SeekOrigin.Begin);
                        image.Uri = await imageservice.Upload(imageStream, image.Filename);
                    }
                }

                image.Id = imageRepository.CreateItemAsync(image).Result;
                insertCounter++;
                System.Console.WriteLine($"Inserted image#: {insertCounter}");
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} images");
            return images;
        }

        public static async System.Threading.Tasks.Task<IEnumerable<Bag>> UpdateBagsAsync(DocumentClient client, string collection, IImageService imageservice)
        {

            var bagsRepository = new GetRepository<Bag>(client, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            var updateBagsRepository = new UpdateRepository<Bag>(client, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            var bags = await bagsRepository.GetItemsAsync(bag => bag.ImageId == null && bag.MainID != 165 && bag.MainID != 1193, 50);
            System.Console.WriteLine($"Fetched {bags.Count()} bags");
            var images = await ImportImages2Async(client, collection, bags, imageservice);
            bags.ToList().ForEach(bag =>
            {
                bag.ImageId = images.FirstOrDefault(image => image.Filename == $"{bag.MainID}.jpg")?.Id;
            });

            System.Console.WriteLine($"Attempting to update {bags.Count()} bags");
            var insertCounter = 0;
            await bags.ToList().ForEachAsync(async bag =>
            {
                var bagid = await updateBagsRepository.UpdateItemAsync(bag.Id, bag);
                insertCounter++;
                System.Console.WriteLine($"Update bag: {insertCounter} - {bag.MainID}");
            });

            System.Console.WriteLine($"Completed updateing {insertCounter} bags");

            return bags;
        }
    }
}
