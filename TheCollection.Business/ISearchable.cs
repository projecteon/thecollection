namespace TheCollection.Business {

    using System.Collections.Generic;

    public interface ISearchable {
        IEnumerable<string> Tags { get; }
        string SearchString { get; }
    }
}
