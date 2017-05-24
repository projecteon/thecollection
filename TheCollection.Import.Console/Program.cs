using Microsoft.Azure.Documents.Client;
using System;
using System.Configuration;
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
                    }
                }
            }

            var documentDbClient = new DocumentClient(
                uri,
                authkey
            );

            //var accessDbPath = "importfiles/accessdb.mdb";
            //var meerkens = AccessExport.GetMeerken(accessDbPath);
            //var thees = AccessExport.GetThees(accessDbPath);

            var brandsFile = "importfiles/teabrands.json";
            var teabagsFile = "importfiles/teabags.json";
            var meerkens = JsonFileExport.GetMeerken(brandsFile);
            var thees = JsonFileExport.GetThees(teabagsFile);

            var collectionName = "TheCollection";
            var brands = DocumentDbImport.ImportBrands(documentDbClient, collectionName, meerkens);
            var bags = DocumentDbImport.ImportBags(documentDbClient, collectionName, thees, brands, new ImageAzureBlobService($"DefaultEndpointsProtocol={scheme};AccountName={name};AccountKey={key};"));

            System.Console.WriteLine("Conversions end");
            System.Console.ReadLine();
        }
    }
}
