using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System.Linq;
using TheCollection.Web.Models;

namespace TheCollection.Web.Services
{
    public class SearchableQuery<T> where T : ISearchable
    {
        public static SqlQuerySpec Create(string collectionId, IEnumerable<string> searchTerms, int top = 0)
        {
            var counter = 0;
            var topSelect = top > 0 ? $"TOP {top}" : "";
            var searchterms = searchTerms.Select(term => $"CONTAINS(o.{nameof(ISearchable.SearchString).ToLower()}, @param{counter++})").ToArray();
            var query = $"SELECT {topSelect} VALUE o FROM {collectionId} o WHERE {searchterms.Aggregate((current, next) => current + " AND " + next)}";
            return new SqlQuerySpec { QueryText = query, Parameters = CreateParams(searchTerms) };
        }

        public static SqlQuerySpec Count(string collectionId, IEnumerable<string> searchTerms)
        {
            var counter = 0;
            var searchterms = searchTerms.Select(term => $"CONTAINS(o.{nameof(ISearchable.SearchString).ToLower()}, @param{counter++})").ToArray();
            var query = $"SELECT COUNT(1) as Count FROM {collectionId} o WHERE {searchterms.Aggregate((current, next) => current + " AND " + next)}";
            return new SqlQuerySpec { QueryText = query, Parameters = CreateParams(searchTerms) };
        }

        static SqlParameterCollection CreateParams(IEnumerable<string> searchTerms)
        {
            var counter = 0;
            return new SqlParameterCollection(searchTerms.Select(searchTerm => new SqlParameter { Name = $"@param{counter++}", Value = searchTerm }));
        }
    }
}
