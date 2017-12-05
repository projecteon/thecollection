namespace TheCollection.Data.DocumentDB.Repositories {
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using TheCollection.Business;
    using TheCollection.Data.DocumentDB.Extensions;

    public class UpsertRepository<T> : IUpsertRepository<T> where T : class {
        private readonly string DatabaseId;
        private readonly string CollectionId;
        private IDocumentClient client;

        public UpsertRepository(IDocumentClient client, string databaseId, string collectionId) {
            DatabaseId = databaseId;
            CollectionId = collectionId;
            this.client = client;
            client.CreateCollectionIfNotExistsAsync(databaseId, collectionId).Wait();
        }

        public async Task<string> UpsertItemAsync(string id, T item) {
            var newItem = await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
            return newItem.Resource.Id;
        }
    }
}
