﻿using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;
using TheCollection.Web.Extensions;

namespace TheCollection.Web.Services
{
    public class UpdateRepository<T> where T : class
    {
        private readonly string DatabaseId;
        private readonly string CollectionId;
        private IDocumentClient client;

        public UpdateRepository(IDocumentClient client, string databaseId, string collectionId)
        {
            DatabaseId = databaseId;
            CollectionId = collectionId;
            this.client = client;
            client.CreateCollectionIfNotExistsAsync(databaseId, collectionId).Wait();
        }

        public async Task<string> UpdateItemAsync(string id, T item)
        {
            var updatedItem = await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
            return updatedItem.Resource.Id;
        }
    }
}