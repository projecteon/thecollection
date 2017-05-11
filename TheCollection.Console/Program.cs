using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheCollection.Web.Models;
using TheCollection.Web.Services;
using TheCollection.Console.Models;

namespace TheCollection.Console
{
    internal class Program
    {
        private static IConfigurationRoot Configuration { get; set; }

        // dotnet run /PDocumentDbClient:EndpointUri=uri /DocumentDbClient:AuthorizationKey=key
        static void Main(string[] args)
        {
            System.Console.WriteLine("Conversions start");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            Configuration = builder.Build();

            var documentDbClient = new DocumentClient(
                Configuration.GetValue<Uri>("DocumentDbClient:EndpointUri"),
                Configuration.GetValue<string>("DocumentDbClient:AuthorizationKey")
            );

            var brands = ImportBrands(documentDbClient);
            ImportBagsAndSubObjects(documentDbClient, brands);
            //var temp = CreateUserDefinedFunction(documentDbClient, "containsLikeAll").Result;
            //UpdateBags(documentDbClient);
            System.Console.WriteLine("Conversions end");
            System.Console.ReadLine();
        }

        private static void UpdateBags(DocumentClient client)
        {
            var bagsRepository = new DocumentDBRepository<Bag>(client, "AspNetCoreIdentitySample", "Bags");
            var bags = bagsRepository.GetItemsAsync(bag => bag.Brand.Name == "Meßmer").Result;
            foreach (var bag in bags)
            {
                var result = bagsRepository.UpdateItemAsync(bag.Id, bag).Result;
            }
        }

        private static IList<Brand> ImportBrands(DocumentClient client)
        {
            string jsonContent = ReadFile("teabrands.js");
            var jsonObj = JsonConvert.DeserializeObject<Merkens>(jsonContent);

            var brandsRepository = new DocumentDBRepository<Brand>(client, "AspNetCoreIdentitySample", "Brands");
            var brands = jsonObj.tblTheeMerken.Select(merk =>
            {
                return new Brand
                {
                    Name = merk.TheeMerk.Trim()
                };
            }).ToList();

            brands.ForEach(brand =>
            {
                brand.Id = brandsRepository.CreateItemAsync(brand).Result;
            });

            return brands;
        }

        private static void ImportBagsAndSubObjects(DocumentClient client, IList<Brand> brands)
        {
            string jsonContent2 = ReadFile("jsonFile.js");
            var jsonObj2 = JsonConvert.DeserializeObject<Thees>(jsonContent2);

            var countries = ImportCountries(client, jsonObj2.TheeTotaallijst);
            var bagTypes = ImportBagTypes(client, jsonObj2.TheeTotaallijst);

            var bagsRepository = new DocumentDBRepository<Bag>(client, "AspNetCoreIdentitySample", "Bags");
            var bags = jsonObj2.TheeTotaallijst
                    .Select(thee =>
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
                            Image = $"{thee.MainID}.jpg"
                        };
                    });

            var insertCounter = 0;
            bags.ToList().ForEach(bag =>
            {
                var bagid = bagsRepository.CreateItemAsync(bag).Result;
                if (insertCounter > 0 && insertCounter % 1000 == 0)
                {

                    System.Console.WriteLine($"Inserted: {insertCounter}");
                }
            });
        }

        private static IEnumerable<Country> ImportCountries(DocumentClient client, List<Thee> thees)
        {
            var countryRepository = new DocumentDBRepository<Country>(client, "AspNetCoreIdentitySample", "Countries");
            var countries = thees.Select(thee => thee.TheeLandvanherkomst.Trim()).Distinct().Select(country => { return new Country { Name = country }; }).ToList();
            countries.ForEach(country =>
            {
                country.Id = countryRepository.CreateItemAsync(country).Result;
            });


            return countries;
        }

        private static IEnumerable<BagType> ImportBagTypes(DocumentClient client, List<Thee> thees)
        {
            var bagTypeRepository = new DocumentDBRepository<BagType>(client, "AspNetCoreIdentitySample", "BagTypes");
            var bagTypes = thees.Select(thee => thee.TheeSoortzakje.Trim()).Distinct().Where(bagType => bagType.Length > 0).Select(type => { return new BagType { Name = type }; }).ToList();
            bagTypes.ForEach(bagType =>
            {
                bagType.Id = bagTypeRepository.CreateItemAsync(bagType).Result;
            });


            return bagTypes;
        }

        public static string ReadFile(string FileName)
        {
            try
            {
                using (StreamReader reader = File.OpenText(FileName))
                {
                    string fileContent = reader.ReadToEnd();
                    if (fileContent != null && fileContent != "")
                    {
                        return fileContent;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log
                throw ex;
            }
            return "";
        }

        private async static Task<UserDefinedFunction> CreateUserDefinedFunction(DocumentClient client, string udfId)
        {
            var udfBody = File.ReadAllText($"{udfId}.js");

            var udfDefinition = new UserDefinedFunction
            {
                Id = udfId,
                Body = udfBody
            };

            var result = await client.CreateUserDefinedFunctionAsync(UriFactory.CreateDocumentCollectionUri("AspNetCoreIdentitySample", "Bags"), udfDefinition);
            var udf = result.Resource;

            System.Console.WriteLine("Created user defined function {0}; RID: {1}", udf.Id, udf.ResourceId);

            return udf;
        }
    }
}
