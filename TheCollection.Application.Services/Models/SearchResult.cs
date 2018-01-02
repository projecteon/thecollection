namespace TheCollection.Application.Services.Models {
    using System.Collections.Generic;

    public class SearchResult<T> {
        public long Count { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
