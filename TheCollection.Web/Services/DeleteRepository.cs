using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;
using TheCollection.Web.Extensions;

namespace TheCollection.Web.Services
{
    public class DeleteRepository<T> where T : class
    {
        private readonly string DatabaseId;
        private readonly string CollectionId;
        private IDocumentClient client;

        public DeleteRepository(IDocumentClient client, string databaseId, string collectionId)
        {
            DatabaseId = databaseId;
            CollectionId = collectionId;
            this.client = client;
            client.CreateCollectionIfNotExistsAsync(databaseId, collectionId).Wait();
        }

        public async Task DeleteItemAsync(string id)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }
    }
}
