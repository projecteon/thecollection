using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TheCollection.Presentation.Web.Repositories;
using TheCollection.Import.Console.Repositories;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace TheCollection.Import.Console
{
    class Program
    {
        // TheCollection.Import.Console.exe /DocumentDbClient:EndpointUri=uri /DocumentDbClient:AuthorizationKey=key /StorageAccount:Name=name /StorageAccount:Key=key
        static void Main(string[] args)
        {
            System.Console.WriteLine("Conversions start");

            var uri = new Uri(ConfigurationManager.AppSettings["DocumentDbClient:EndpointUri"]);
            var authkey = ConfigurationManager.AppSettings["DocumentDbClient:AuthorizationKey"];
            var name = ConfigurationManager.AppSettings["StorageAccount:Name"];
            var key = ConfigurationManager.AppSettings["StorageAccount:Key"];
            var scheme = ConfigurationManager.AppSettings["StorageAccount:Scheme"];
            var endPoints = $"BlobEndpoint=http://127.0.0.1:10000/{name};TableEndpoint=http://127.0.0.1:10000/{name};QueueEndpoint=http://127.0.0.1:10000/{name};";
            if (args.Length > 1)
            {
                if (args[0].Contains("DocumentDbClient:EndpointUri") && args[1].Contains("DocumentDbClient:AuthorizationKey"))
                {
                    uri = new Uri(args[0].Split('=')[1]);
                    authkey = args[1].Split('=')[1];
                }

                if (args.Length == 4)
                {
                    if (args[2].Contains("StorageAccount:Name") && args[3].Contains("StorageAccount:Key"))
                    {
                        name = args[2].Split('=')[1];
                        key = args[3].Split('=')[1];
                        scheme = "https";
                        endPoints = "";
                    }
                }
            }

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            var documentDbClient = new DocumentClient(
                uri,
                authkey,
                serializerSettings,
                new ConnectionPolicy { EnableEndpointDiscovery = false },
                null
            );

            var imageUploadConnectionString = $"DefaultEndpointsProtocol={scheme};AccountName={name};AccountKey={key};{endPoints}";
            //var imageUploadConnectionString2 = "UseDevelopmentStorage=true";

            //var imageUploadService = new ImageAzureBlobService(imageUploadConnectionString);
            //var azureStorageClient = new AzureStorageClient(imageUploadConnectionString);

            ImportTeabags(documentDbClient);
            //ImportImagesAndUpdateTeabags(documentDbClient, imageUploadService);
            //CopyMissingFilesToTempUploadDir(azureStorageClient);

            System.Console.WriteLine("Conversions end");
            System.Console.ReadLine();
        }

        static void CopyMissingFilesToTempUploadDir(AzureStorageClient azureClient)
        {
            var (meerkens, thees) = ImportFromAccess();
            var list = azureClient.GetList();
            var missingImageImports = thees.Where(thee => !list.Contains($"{thee.MainID}.jpg"));
            var missingImageImportsNames = missingImageImports.Select(thee => $"{thee.MainID}.jpg").ToList();
            var missingFiles = missingImageImportsNames.Where(missing => !System.IO.File.Exists($"{ImageFilesystemRepository.Path}{missing}"));
            var troubleTeaBags = thees.Where(thee => !missingFiles.Contains($"{thee.MainID}.jpg"))
                .ToList()
                .Select(thee => $"{thee.TheeMerk} - {thee.TheeSmaak} - {thee.TheeSerienummer}");
            var uploadDir = @"C:\src\projecteon\missingthees\";
            missingImageImportsNames.ForEach(missingFile =>
            {
                if (System.IO.File.Exists($"{ImageFilesystemRepository.Path}{missingFile}"))
                    System.IO.File.Copy($"{ImageFilesystemRepository.Path}{missingFile}", $"{uploadDir}{missingFile}", true);
            });
        }

        static (IEnumerable<Models.Merk> meerkens, IEnumerable<Models.Thee> thees) ImportFromAccess()
        {
            var accessDbPath = "importfiles/Thee Database.mdb";
            var meerkens = (new MerkRepository(accessDbPath)).SearchItemsAsync().Result;
            var thees = (new TheeRepository(accessDbPath)).SearchItemsAsync().Result;

            return (meerkens, thees);
        }


        private static void ImportTeabags(DocumentClient documentDbClient)
        {
            var (meerkens, thees) = ImportFromAccess();
            var theesToImport = thees.OrderBy(thee => thee.MainID).ToList();
            var meerkensToImport = meerkens.ToList();

            var brands = DocumentDbImport.ImportBrandsAsync(documentDbClient, meerkensToImport).Result;
            var bags = DocumentDbImport.ImportBagsAsync(documentDbClient, theesToImport, brands).Result;
        }

        private static void ImportImagesAndUpdateTeabags(DocumentClient documentDbClient, ImageAzureBlobRepository imageUploadService)
        {
            var updateBags = DocumentDbImport.UpdateBagsAsync(documentDbClient, imageUploadService).Result;
        }
    }
}
