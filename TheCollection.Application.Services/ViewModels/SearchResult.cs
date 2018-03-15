namespace TheCollection.Application.Services.ViewModels {
    using System.Collections.Generic;

    public class SearchResult<T> {
        public SearchResult(IEnumerable<T> data, long count) {
            Data = data;
            Count = count;
        }

        public IEnumerable<T> Data { get; }
        public long Count { get; }
    }
}
