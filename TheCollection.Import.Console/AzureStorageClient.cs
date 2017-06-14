using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCollection.Import.Console
{
    public class AzureStorageClient
    {
        public CloudBlobContainer Container { get; }

        public AzureStorageClient(string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            Container = blobClient.GetContainerReference("images");
            CreateContainerIfNotExistsAsync().Wait();
        }

        private async Task CreateContainerIfNotExistsAsync()
        {
            await Container.CreateIfNotExistsAsync();
        }

        public List<string> GetList()
        {
            var list = Container.ListBlobs();
            var blobNames = list.OfType<CloudBlockBlob>().Select(b => b.Name).ToList();
            return blobNames;
        }
    }
}
