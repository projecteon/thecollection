using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System.Linq;
using TheCollection.Business;

namespace TheCollection.Web.Services
{
    public class SearchableQuery<T> 
    {
        public static SqlQuerySpec Create(string collectionId, IEnumerable<string> searchTerms, int top = 0)
        {
            var topSelect = top > 0 ? $"TOP {top}" : "";
            var query = $"SELECT {topSelect} VALUE o FROM {collectionId} o WHERE 1=1 {CreateSearchTermWhereClause(searchTerms)}";
            return new SqlQuerySpec { QueryText = query, Parameters = CreateParams(searchTerms) };
        }

        public static SqlQuerySpec Count(string collectionId, IEnumerable<string> searchTerms)
        {
            var query = $"SELECT COUNT(1) as Count FROM {collectionId} o WHERE 1=1 {CreateSearchTermWhereClause(searchTerms)}";
            return new SqlQuerySpec { QueryText = query, Parameters = CreateParams(searchTerms) };
        }

        static string CreateSearchTermWhereClause(IEnumerable<string> searchTerms)
        {
            var counter = 0;
            var searchterms = searchTerms.Where(term => term.Length > 0).Select(term => $"CONTAINS(o.{nameof(ISearchable.SearchString).ToLower()}, @param{counter++})").ToArray();
            if (counter > 0) return $"AND {searchterms.Aggregate((current, next) => $"{current} AND {next}")}";

            return "";
        }

        static SqlParameterCollection CreateParams(IEnumerable<string> searchTerms)
        {
            var counter = 0;
            return new SqlParameterCollection(searchTerms.Select(searchTerm => new SqlParameter { Name = $"@param{counter++}", Value = searchTerm }));
        }
    }
}
