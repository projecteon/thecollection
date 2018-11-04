namespace TheCollection.Import.Console {
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Azure.Documents.Client;
    using TheCollection.Data.DocumentDB.Repositories;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Tea;
    using TheCollection.Import.Console.Extensions;
    using TheCollection.Import.Console.Models;
    using TheCollection.Import.Console.Translators;
    using TheCollection.Presentation.Web.Constants;
    using TheCollection.Presentation.Web.Repositories;

    public class DocumentDbImport {
        static async System.Threading.Tasks.Task<IList<Brand>> ImportBrandsAsync(DocumentClient client, List<Merk> meerken) {
            var translator = new MerkToBrandTranslator();
            var brands = meerken.Select(merk => {
                var newBrand = translator.Translate(merk);
                return newBrand;
            }).ToList();

            var brandsRepository = new CreateRepository<Brand>(client, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Brands);
            var insertCounter = 0;
            var insertedBrands = new List<Brand>();
            foreach (var brand in brands) {
                var id = await brandsRepository.CreateItemAsync(brand);
                insertedBrands.Add(new Brand(id, brand.Name));
                insertCounter++;
                if (insertCounter > 0 && insertCounter % 100 == 0) {
                    System.Console.WriteLine($"Inserted brand#: {insertCounter}");
                }
            }

            System.Console.WriteLine($"Completed inserting {insertCounter} brands");
            return insertedBrands;
        }

        public static async System.Threading.Tasks.Task<IEnumerable<Bag>> ImportBagsAsync(DocumentClient client, IImageRepository imageUploadService, List<Thee> thees, List<Merk> meerkens) {
            var brands = await ImportBrandsAsync(client, meerkens);
            var countries = await ImportCountriesAsync(client, thees);
            var bagTypes = await ImportBagTypesAsync(client, thees);
            var images = await ImportImagesAsync(client, thees, imageUploadService);
            var translater = new TheeToBagTranslator(countries, brands, bagTypes, images);

            var bagsRepository = new CreateRepository<Bag>(client, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Bags);
            var bags = thees.Select(thee => {
                var newBag = translater.Translate(thee);
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

        private static async System.Threading.Tasks.Task<List<Country>> ImportCountriesAsync(DocumentClient client, List<Thee> thees) {
            var countries = thees.Select(thee => thee.TheeLandvanherkomst.Trim()).Distinct().Where(country => country.Length > 0).Select(country => { return new Country(null, country); }).ToList();
            var countryRepository = new CreateRepository<Country>(client, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Countries);
            var insertCounter = 0;
            var insertedCountries = new List<Country>();
            foreach (var country in countries) {
                var id = await countryRepository.CreateItemAsync(country);
                insertedCountries.Add(new Country(id, country.Name));
                insertCounter++;
            }

            System.Console.WriteLine($"Completed inserting {insertCounter} countries");
            return insertedCountries;
        }

        private static async System.Threading.Tasks.Task<List<BagType>> ImportBagTypesAsync(DocumentClient client, List<Thee> thees) {
            var bagTypes = thees.Select(thee => thee.TheeSoortzakje.Trim()).Distinct().Where(bagType => bagType.Length > 0).Select(type => { return new BagType(null, type); }).ToList();
            var bagTypeRepository = new CreateRepository<BagType>(client, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.BagTypes);
            var insertCounter = 0;
            var insertedBagTypes = new List<BagType>();
            foreach (var bagType in bagTypes) {
                var id = await bagTypeRepository.CreateItemAsync(bagType);
                insertedBagTypes.Add(new BagType(id, bagType.Name));
                insertCounter++;
            }

            System.Console.WriteLine($"Completed inserting {insertCounter} bagtypes");
            return insertedBagTypes;
        }

        private static async System.Threading.Tasks.Task<string> ImportImageAsync(DocumentClient client, Thee thee, IImageRepository imageservice) {
            if (File.Exists($"{ImageFilesystemRepository.Path}{thee.MainID}.jpg") == false) {
                return null;
            }

            var image = new Image(null, $"{thee.MainID}.jpg", null);
            var fileImageService = new ImageFilesystemRepository();
            var uri = "";
            using (var bitmap = await fileImageService.Get(image.Filename)) {
                using (var imageStream = new MemoryStream()) {
                    bitmap.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    imageStream.Seek(0, SeekOrigin.Begin);
                    uri = await imageservice.Upload(imageStream, image.Filename);
                }
            }

            var imageRepository = new CreateRepository<Image>(client, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Images);
            return await imageRepository.CreateItemAsync(image);
        }

        private static async System.Threading.Tasks.Task<List<Image>> ImportImagesAsync(DocumentClient client, List<Thee> thees, IImageRepository imageservice) {
            var images = thees.Where(bag => File.Exists($"{ImageFilesystemRepository.Path}{bag.MainID}.jpg")).Select(thee => { return new Image(null, $"{thee.MainID}.jpg", null); }).ToList();
            var imageRepository = new CreateRepository<Image>(client, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Images);
            var insertCounter = 0;
            var insertedImages = new List<Image>();
            foreach (var image in images) {
                var fileImageService = new ImageFilesystemRepository();
                var uri = "";
                using (var bitmap = await fileImageService.Get(image.Filename)) {
                    using (var imageStream = new MemoryStream()) {
                        bitmap.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imageStream.Seek(0, SeekOrigin.Begin);
                        uri = await imageservice.Upload(imageStream, image.Filename);
                    }
                }

                var id = await imageRepository.CreateItemAsync(image);
                insertedImages.Add(new Image(id, image.Filename, uri));
                insertCounter++;
                if (insertCounter > 0 && insertCounter % 250 == 0) {
                    System.Console.WriteLine($"Inserted image#: {insertCounter}");
                }
            }

            System.Console.WriteLine($"Completed inserting {insertCounter} images");
            return insertedImages;
        }

        private static async System.Threading.Tasks.Task<List<Image>> ImportImages2Async(DocumentClient client, IEnumerable<Bag> bags, IImageRepository imageservice) {
            var images = bags.Where(bag => File.Exists($"{ImageFilesystemRepository.Path}{bag.MainID}.jpg")).Select(thee => { return new Image(null, $"{thee.MainID}.jpg", null); }).ToList();
            var imageRepository = new CreateRepository<Image>(client, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Images);
            var insertCounter = 0;
            var insertedImages = new List<Image>();
            foreach (var image in images) {
                var fileImageService = new ImageFilesystemRepository();
                var uri = "";
                using (var bitmap = await fileImageService.Get(image.Filename)) {
                    using (var imageStream = new MemoryStream()) {
                        bitmap.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imageStream.Seek(0, SeekOrigin.Begin);
                        uri = await imageservice.Upload(imageStream, image.Filename);
                    }
                }

                var id = await imageRepository.CreateItemAsync(image);
                insertedImages.Add(new Image(id, image.Filename, uri));
                insertCounter++;
                if (insertCounter > 0 && insertCounter % 250 == 0) {
                    System.Console.WriteLine($"Inserted image#: {insertCounter}");
                }
            }

            System.Console.WriteLine($"Completed inserting {insertCounter} images");
            return insertedImages;
        }

        public static async System.Threading.Tasks.Task<IEnumerable<Bag>> UpdateBagsAsync(DocumentClient client, IImageRepository imageservice) {
            var bagsRepository = new SearchRepository<Bag>(client, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Bags);
            var updateBagsRepository = new UpdateRepository<Bag>(client, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Bags);
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
