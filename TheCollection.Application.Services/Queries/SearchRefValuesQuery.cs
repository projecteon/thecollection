namespace TheCollection.Application.Services.Queries {
    using TheCollection.Domain.Core.Contracts;

    public class SearchRefValuesQuery<T> : IQuery where T: class, IRef {
        public SearchRefValuesQuery(string searchTerm) {
            SearchTerm = searchTerm;
        }

        public string SearchTerm { get; }
    }
}
