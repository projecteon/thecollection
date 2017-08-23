using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;
using TheCollection.Web.Extensions;

namespace TheCollection.Web.Services
{
    public class CreateRepository<T> where T : class
    {
        private readonly string DatabaseId;
        private readonly string CollectionId;
        private IDocumentClient client;

        public CreateRepository(IDocumentClient client, string databaseId, string collectionId)
        {
            DatabaseId = databaseId;
            CollectionId = collectionId;
            this.client = client;
            client.CreateCollectionIfNotExistsAsync(databaseId, collectionId).Wait();
        }

        public async Task<string> CreateItemAsync(T item)
        {
            var newItem = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
            return newItem.Resource.Id;
        }
    }
}
