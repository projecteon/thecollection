namespace TheCollection.Web.Repositories {
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using TheCollection.Domain.Contracts.Repository;

    // https://blogs.msdn.microsoft.com/premier_developer/2017/03/14/building-a-simple-photo-album-using-azure-blob-storage-with-net-core/
    // https://docs.microsoft.com/en-us/azure/storage/storage-samples-dotnet
    // https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator
    // https://www.janaks.com.np/file-upload-asp-net-core-web-api/
    // http://stackoverflow.com/questions/41421280/serving-images-from-azure-blob-storage-in-dot-net-core
    // http://dotnetthoughts.net/working-with-azure-blob-storage-in-aspnet-core/
    // http://www.dotnetcurry.com/visualstudio/1328/visual-studio-connected-services-aspnet-core-azure-storage

    public class ImageAzureBlobRepository : IImageRepository {
        public CloudBlobContainer Container { get; }

        public ImageAzureBlobRepository(string connectionString) {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            Container = blobClient.GetContainerReference("images");
            CreateContainerIfNotExistsAsync().Wait();
        }

        public ImageAzureBlobRepository(string scheme, string name, string key, string endpoints) {
            var connectionString = $"DefaultEndpointsProtocol={scheme};AccountName={name};AccountKey={key};{endpoints}";
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            Container = blobClient.GetContainerReference("images");
            CreateContainerIfNotExistsAsync().Wait();
        }

        public async Task<bool> Delete(string filename) {
            var blockBlob = Container.GetBlockBlobReference(filename);
            return await blockBlob.DeleteIfExistsAsync();
        }

        public async Task<Bitmap> Get(string filename) {
            var blockBlob = Container.GetBlockBlobReference(filename);
            return await GetBitmap(blockBlob);
        }

        public async Task<string> Upload(Stream stream, string filename) {
            var blockBlob = Container.GetBlockBlobReference(filename);
            await blockBlob.UploadFromStreamAsync(stream);
            return blockBlob?.Uri.ToString();
        }

        async Task<Bitmap> GetBitmap(CloudBlockBlob blockBlob) {
            Bitmap image;
            using (var memoryStream = new MemoryStream()) {
                await blockBlob.DownloadToStreamAsync(memoryStream);
                image = new Bitmap(Image.FromStream(memoryStream));
            }

            return image;
        }

        async Task CreateContainerIfNotExistsAsync() {
            await Container.CreateIfNotExistsAsync();
        }
    }
}
