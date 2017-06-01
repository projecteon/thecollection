using Microsoft.Azure.Documents.Client;
using System;
using System.Configuration;
using System.Linq;
using TheCollection.Web.Services;

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

            var documentDbClient = new DocumentClient(
                uri,
                authkey
            );

            var imageUploadConnectionString = $"DefaultEndpointsProtocol={scheme};AccountName={name};AccountKey={key};{endPoints}";
            var imageUploadConnectionString2 = "UseDevelopmentStorage=true";

            var imageUploadService = new ImageAzureBlobService(imageUploadConnectionString);

            //var accessDbPath = "importfiles/accessdb.mdb";
            //var meerkens = AccessExport.GetMeerken(accessDbPath);
            //var thees = AccessExport.GetThees(accessDbPath);

            var brandsFile = "importfiles/teabrands.json";
            var teabagsFile = "importfiles/teabags.json";
            var meerkens = JsonFileExport.GetMeerken(brandsFile);
            var thees = JsonFileExport.GetThees(teabagsFile);

            var theesToImport = thees.Take(1).ToList();
            var meerkensToImport = meerkens.Where(meerk => theesToImport.Any(thee => thee.TheeMerk.Trim() == meerk.TheeMerk.Trim())).ToList();

            var collectionName = "TheCollection";
            var brands = DocumentDbImport.ImportBrandsAsync(documentDbClient, collectionName, meerkensToImport).Result;
            var bags = DocumentDbImport.ImportBagsAsync(documentDbClient, collectionName, theesToImport, brands, imageUploadService).Result;

            System.Console.WriteLine("Conversions end");
            System.Console.ReadLine();
        }
    }
}
