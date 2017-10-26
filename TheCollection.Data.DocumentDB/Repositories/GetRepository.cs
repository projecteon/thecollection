namespace TheCollection.Data.DocumentDB {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using TheCollection.Data.DocumentDB.Extensions;

    public class GetRepository<T> where T : class {
        private readonly string DatabaseId;
        private readonly string CollectionId;
        private IDocumentClient client;

        public GetRepository(IDocumentClient client, string databaseId, string collectionId) {
            DatabaseId = databaseId;
            CollectionId = collectionId;
            this.client = client;
            client.CreateCollectionIfNotExistsAsync(databaseId, collectionId).Wait();
        }

        public async Task<T> GetItemAsync(string id) {
            try {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e) {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound) {
                    return null;
                }
                else {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate = null, int pageSize = 0, int page = 0) {
            var query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 }).AsQueryable();

            if (predicate != null) {
                query = query.Where(predicate);
            }

            if (pageSize > 0) {
                if (page > 0) {
                    query = query.Skip(pageSize * page);
                }

                query = query.Take(pageSize);
            }

            var documentQuery = query.AsDocumentQuery();
            var results = new List<T>();
            while (documentQuery.HasMoreResults) {
                results.AddRange(await documentQuery.ExecuteNextAsync<T>());
            }

            return results;
        }
    }
}
