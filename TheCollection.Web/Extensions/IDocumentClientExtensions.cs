using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace TheCollection.Web.Extensions {

    public static class IDocumentClientExtensions {

        private static async Task CreateDatabaseIfNotExistsAsync(this IDocumentClient client, string databaseId) {
            try {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseId));
            }
            catch (DocumentClientException e) {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound) {
                    await client.CreateDatabaseAsync(new Database { Id = databaseId });
                }
                else {
                    throw;
                }
            }
        }

        public static async Task CreateCollectionIfNotExistsAsync(this IDocumentClient client, string databaseId, string collectionId) {
            try {
                await CreateDatabaseIfNotExistsAsync(client, databaseId);
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId));
            }
            catch (DocumentClientException e) {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound) {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(databaseId),
                        new DocumentCollection { Id = collectionId },
                        new RequestOptions { OfferThroughput = 7000 });
                }
                else {
                    throw;
                }
            }
        }
    }
}
