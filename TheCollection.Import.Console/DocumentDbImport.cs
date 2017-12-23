namespace TheCollection.Import.Console {
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Azure.Documents.Client;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Domain.Contracts.Repository;
    using TheCollection.Domain.Tea;
    using TheCollection.Import.Console.Extensions;
    using TheCollection.Import.Console.Models;
    using TheCollection.Import.Console.Translators;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Repositories;

    public class DocumentDbImport {
        public static async System.Threading.Tasks.Task<IList<Brand>> ImportBrandsAsync(DocumentClient client, List<Merk> meerken)
        {
            var translator = new MerkToBrandTranslator();
            var brands = meerken.Select(merk => {
                var newBrand = new Brand();
                translator.Translate(merk, newBrand);
                return newBrand;
            }).ToList();

            var brandsRepository = new CreateRepository<Brand>(client, DocumentDB.DatabaseId, DocumentDB.BrandsCollectionId);
            var insertCounter = 0;
            await brands.ForEachAsync(async brand => {
                brand.Id = await brandsRepository.CreateItemAsync(brand);
                insertCounter++;
                if (insertCounter > 0 && insertCounter % 100 == 0) {
                    System.Console.WriteLine($"Inserted brand#: {insertCounter}");
                }
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} brands");
            return brands;
        }

        public static async System.Threading.Tasks.Task<IEnumerable<Bag>> ImportBagsAsync(DocumentClient client, List<Thee> thees, IList<Brand> brands)
        {
            var countries = await ImportCountriesAsync(client, thees);
            var bagTypes = await ImportBagTypesAsync(client, thees);
            var images = await ImportImagesAsync(client, thees);
            var translater = new TheeToBagTranslator(countries, brands, bagTypes, images);

            var bagsRepository = new CreateRepository<Bag>(client, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            var bags = thees.Select(thee => {
                var newBag = new Bag();
                translater.Translate(thee, newBag);
                return newBag;
            }).ToList();

            System.Console.WriteLine($"Attempting to insert {bags.Count()} bags out of {thees.Count()} thees");
            var insertCounter = 0;
            await bags.ToList().ForEachAsync(async bag => {
                var bagid = await bagsRepository.CreateItemAsync(bag);
                insertCounter++;
                if (insertCounter > 0 && insertCounter % 100 == 0) {
                    System.Console.WriteLine($"Inserted bag#: {insertCounter}");
                }
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} bags");
            return bags;
        }

        private static async System.Threading.Tasks.Task<List<Country>> ImportCountriesAsync(DocumentClient client, List<Thee> thees)
        {
            var countries = thees.Select(thee => thee.TheeLandvanherkomst.Trim()).Distinct().Where(country => country.Length > 0).Select(country => { return new Country { Name = country }; }).ToList();
            var countryRepository = new CreateRepository<Country>(client, DocumentDB.DatabaseId, DocumentDB.CountriesCollectionId);
            var insertCounter = 0;
            await countries.ForEachAsync(async country => {
                country.Id = await countryRepository.CreateItemAsync(country);
                insertCounter++;
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} countries");
            return countries;
        }

        private static async System.Threading.Tasks.Task<List<BagType>> ImportBagTypesAsync(DocumentClient client, List<Thee> thees)
        {
            var bagTypes = thees.Select(thee => thee.TheeSoortzakje.Trim()).Distinct().Where(bagType => bagType.Length > 0).Select(type => { return new BagType { Name = type }; }).ToList();
            var bagTypeRepository = new CreateRepository<BagType>(client, DocumentDB.DatabaseId, DocumentDB.BagTypesCollectionId);
            var insertCounter = 0;
            await bagTypes.ForEachAsync(async bagType => {
                bagType.Id = await bagTypeRepository.CreateItemAsync(bagType);
                insertCounter++;
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} bagtypes");
            return bagTypes;
        }

        private static async System.Threading.Tasks.Task<List<Image>> ImportImagesAsync(DocumentClient client, List<Thee> thees)
        {
            var images = thees.Where(thee => File.Exists($"{ImageFilesystemRepository.Path}{thee.MainID}.jpg")).Select(thee => { return new Image { Filename = $"{thee.MainID}.jpg" }; }).ToList();
            var imageRepository = new CreateRepository<Image>(client, DocumentDB.DatabaseId, DocumentDB.ImagesCollectionId);
            var insertCounter = 0;
            await images.ForEachAsync(async image => {
                image.Id = await imageRepository.CreateItemAsync(image);
                insertCounter++;
                if (insertCounter > 0 && insertCounter % 250 == 0) {
                    System.Console.WriteLine($"Inserted image#: {insertCounter}");
                }
            });

            System.Console.WriteLine($"Completed inserting {insertCounter} images");
            return images;
        }

        private static async System.Threading.Tasks.Task<List<Image>> ImportImages2Async(DocumentClient client, IEnumerable<Bag> bags, IImageRepository imageservice)
        {
            var images = bags.Where(bag => File.Exists($"{ImageFilesystemRepository.Path}{bag.MainID}.jpg")).Select(thee => { return new Image { Filename = $"{thee.MainID}.jpg" }; }).ToList();
            var imageRepository = new CreateRepository<Image>(client, DocumentDB.DatabaseId, DocumentDB.ImagesCollectionId);
            var insertCounter = 0;
            await images.ForEachAsync(async image => {
                var fileImageService = new ImageFilesystemRepository();
                using (var bitmap = await fileImageService.Get(image.Filename)) {
                    using (var imageStream = new MemoryStream()) {
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

        public static async System.Threading.Tasks.Task<IEnumerable<Bag>> UpdateBagsAsync(DocumentClient client, IImageRepository imageservice)
        {
            var bagsRepository = new SearchRepository<Bag>(client, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            var updateBagsRepository = new UpdateRepository<Bag>(client, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            var bags = await bagsRepository.SearchItemsAsync(bag => bag.ImageId == null && bag.MainID != 165 && bag.MainID != 1193, 50);
            System.Console.WriteLine($"Fetched {bags.Count()} bags");
            var images = await ImportImages2Async(client, bags, imageservice);
            bags.ToList().ForEach(bag => {
                bag.ImageId = images.FirstOrDefault(image => image.Filename == $"{bag.MainID}.jpg")?.Id;
            });

            System.Console.WriteLine($"Attempting to update {bags.Count()} bags");
            var insertCounter = 0;
            await bags.ToList().ForEachAsync(async bag => {
                var bagid = await updateBagsRepository.UpdateItemAsync(bag.Id, bag);
                insertCounter++;
                System.Console.WriteLine($"Update bag: {insertCounter} - {bag.MainID}");
            });

            System.Console.WriteLine($"Completed updateing {insertCounter} bags");

            return bags;
        }
    }
}
