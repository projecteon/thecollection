using System.Collections.Generic;

namespace TheCollection.Web.Models {

    public class SearchResult<T> {
        public long count { get; set; }
        public IEnumerable<T> data { get; set; }
    }
}
