using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Linq;
using System.Threading.Tasks;
using TheCollection.Business.Tea;
using TheCollection.Web.Constants;
using TheCollection.Web.Extensions;
using TheCollection.Web.Models;
using TheCollection.Web.Services;

namespace TheCollection.Web.Commands
{
    public class SearchCountriesCommand
    {
        private readonly IDocumentClient documentDbClient;

        public SearchCountriesCommand(IDocumentClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
        }

        public async Task<IActionResult> ExecuteAsync(Search search)
        {
            if (search.searchterm.IsNullOrWhiteSpace())
            {
                return new BadRequestResult();
            }

            var countriesRepository = new SearchRepository<Country>(documentDbClient, DocumentDB.DatabaseId, DocumentDB.CountriesCollectionId);
            var countries = await countriesRepository.SearchAsync(search.searchterm);

            return new OkObjectResult(countries.OrderBy(country => country.Name));
        }
    }
}
