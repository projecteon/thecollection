namespace TheCollection.Presentation.Web.Repositories {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Presentation.Web.Constants;
    using TheCollection.Presentation.Web.Models;

    public class WebUserRepository : IGetRepository<IApplicationUser>, IGetAllRepository<IApplicationUser> {
        private static readonly string DatabaseId = DocumentDBConstants.DatabaseId;
        private static readonly string CollectionId = DocumentDBConstants.Collections.AspNetIdentity;

        private readonly UserManager<WebUser> _userManager;
        private readonly IHttpContextAccessor _context;
        private readonly IDocumentClient _client;

        public WebUserRepository(UserManager<WebUser> userManager, IHttpContextAccessor context, IDocumentClient client) {
            _userManager = userManager;
            _context = context;
            _client = client;
        }

        public async Task<IApplicationUser> GetItemAsync(string id = null) {
            return await _userManager.GetUserAsync(_context.HttpContext.User);
        }

        public async Task<IEnumerable<IApplicationUser>> GetAllAsync() {
            var query = _client.CreateDocumentQuery<WebUser>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 }).AsQueryable()
                .Where(r => r.DocumentType == typeof(WebUser).Name);


            var documentQuery = query.AsDocumentQuery();
            var results = new List<IApplicationUser>();
            while (documentQuery.HasMoreResults) {
                results.AddRange(await documentQuery.ExecuteNextAsync<WebUser>());
            }

            return results;
        }
    }
}
