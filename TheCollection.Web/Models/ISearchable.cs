using System.Collections.Generic;

namespace TheCollection.Web.Models
{
    public interface ISearchable
    {
        IEnumerable<string> Tags { get; }
        string SearchString { get; }
    }
}
