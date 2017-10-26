namespace TheCollection.Data.DocumentDB {

    // consider     https://github.com/Crokus/documentdb-repo/blob/master/src/DocumentDb.Repository/DocumentDbRepository.cs
    // partitioning https://petarivanovblog.wordpress.com/2015/10/13/azure-documentdb-generic-repository-with-partitioning-part-1/
    // search       https://azure.microsoft.com/en-us/blog/searching-for-text-with-documentdb/
    //              https://auth0.com/blog/documentdb-with-aspnetcore/
    //              http://www.dotnetcurry.com/windows-azure/1262/documentdb-nosql-json-introduction
    //              https://www.tutorialspoint.com/documentdb_sql/documentdb_sql_parameterized.htm
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using TheCollection.Business;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Data.DocumentDB.Extensions;

    public class SearchRepository<T> : ISearchRepository<T> where T : class {
        private readonly string DatabaseId;
        private readonly string CollectionId;
        private IDocumentClient client;

        public SearchRepository(IDocumentClient client, string databaseId, string collectionId) {
            DatabaseId = databaseId;
            CollectionId = collectionId;
            this.client = client;
            client.CreateCollectionIfNotExistsAsync(databaseId, collectionId).Wait();
        }

        public async Task<IEnumerable<T>> SearchAsync(string searchterm, int top = 0) {
            var query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                SearchableQuery<T>.Create(CollectionId, searchterm.ToLower().Split(' '), top),
                new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults) {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<long> SearchRowCountAsync(string searchterm) {
            var query = client.CreateDocumentQuery(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                SearchableQuery<T>.Count(CollectionId, searchterm.ToLower().Split(' ')),
                new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();

            long results = 0;
            while (query.HasMoreResults) {
                var queryResult = await query.ExecuteNextAsync();
                results += queryResult.First().Count;
            }

            return results;
        }
    }
}
