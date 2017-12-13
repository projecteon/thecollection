namespace TheCollection.Web.Commands.Tea {

    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain.Extensions;
    using TheCollection.Domain.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Models;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Translators.Tea;

    public class SearchCountriesCommand {

        public SearchCountriesCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            CountryTranslator = new CountryToCountryTranslator(applicationUser);
        }

        IDocumentClient DocumentDbClient { get; }
        ITranslator<Country, Models.Tea.Country> CountryTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Search search) {
            if (search.searchterm.IsNullOrWhiteSpace()) {
                return new BadRequestResult();
            }

            var countriesRepository = new SearchRepository<Country>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.CountriesCollectionId);
            var countries = await countriesRepository.SearchAsync(search.searchterm);
            var sortedcountries = countries.OrderBy(country => country.Name);

            return new OkObjectResult(CountryTranslator.Translate(sortedcountries));
        }
    }
}
