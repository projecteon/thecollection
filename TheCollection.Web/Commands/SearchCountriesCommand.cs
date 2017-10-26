namespace TheCollection.Web.Commands
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Business.Tea;
    using TheCollection.Lib.Extensions;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Models;
    using TheCollection.Web.Services;

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
