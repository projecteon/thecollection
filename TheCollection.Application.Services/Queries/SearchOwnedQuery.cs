namespace TheCollection.Application.Services.Queries {
    using TheCollection.Domain.Core.Contracts;

    public class SearchOwnedQuery : IQuery {
        public SearchOwnedQuery(string searchTerm, int pageSize) {
            SearchTerm = searchTerm;
            PageSize = pageSize;
        }

        public string SearchTerm { get; }
        public int PageSize { get; }
    }
}
