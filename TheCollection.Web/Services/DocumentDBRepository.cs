using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TheCollection.Web.Services
{
    // consider     https://github.com/Crokus/documentdb-repo/blob/master/src/DocumentDb.Repository/DocumentDbRepository.cs
    // partitioning https://petarivanovblog.wordpress.com/2015/10/13/azure-documentdb-generic-repository-with-partitioning-part-1/
    // search       https://azure.microsoft.com/en-us/blog/searching-for-text-with-documentdb/
    //              https://auth0.com/blog/documentdb-with-aspnetcore/
    //              http://www.dotnetcurry.com/windows-azure/1262/documentdb-nosql-json-introduction
    //              https://www.tutorialspoint.com/documentdb_sql/documentdb_sql_parameterized.htm
    public class DocumentDBRepository<T> where T : class
    {
        private readonly string DatabaseId;
        private readonly string CollectionId;
        private DocumentClient client;

        public DocumentDBRepository(DocumentClient client, string databaseId, string collectionId)
        {
            DatabaseId = databaseId;
            CollectionId = collectionId;
            this.client = client;
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<long> GetRowCountAsync<T>(string searchterm)
        {
            var query = client.CreateDocumentQuery(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                SearchableQuery<T>.Count(CollectionId, searchterm.ToLower().Split(' ')),
                new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();

            long results = 0;
            while (query.HasMoreResults)
            {
                var queryResult = await query.ExecuteNextAsync();
                results += queryResult.First().Count;
            }

            return results;
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(string searchterm, int top = 100)         {
            var query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                SearchableQuery<T>.Create(CollectionId, searchterm.ToLower().Split(' '), top),
                new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate = null, IEnumerable<Expression<Func<T, IEnumerable<T>>>> predicate2 = null)
        {
            var query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 }).AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (predicate2 != null)
            {
                query = query.SelectMany(predicate2.ElementAt(0));
                query = query.SelectMany(predicate2.ElementAt(1));

            }

            //var documentQuery = query.Take(100).AsDocumentQuery();
            var documentQuery = query.AsDocumentQuery();
            var results = new List<T>();
            while (documentQuery.HasMoreResults)
            {
                results.AddRange(await documentQuery.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<string> CreateItemAsync(T item)
        {
            var newItem = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
            return newItem.Resource.Id;
        }

        public async Task<string> UpdateItemAsync(string id, T item)
        {
            var updatedItem = await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
            return updatedItem.Resource.Id;
        }

        public async Task DeleteItemAsync(string id)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
