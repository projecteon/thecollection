namespace TheCollection.Domain.Contracts {
    using System.Collections.Generic;

    public interface ISearchable {
        IEnumerable<string> Tags { get; }
        string SearchString { get; }
    }
}
