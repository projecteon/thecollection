namespace TheCollection.Application.Services.Queries {
    using TheCollection.Domain.Core.Contracts;

    public class SearchQuery : IQuery {
        public SearchQuery(string searchTerm, int pageSise) {
            SearchTerm = searchTerm;
            PageSise = pageSise;
        }

        public string SearchTerm { get;  }
        public int PageSise { get; }
        public int PageSize { get; }
    }
}
