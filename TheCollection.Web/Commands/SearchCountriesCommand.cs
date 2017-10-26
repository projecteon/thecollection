namespace TheCollection.Web.Commands {

    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Business.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Lib.Extensions;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Models;
    using TheCollection.Web.Translators.Tea;

    public class SearchCountriesCommand {

        public SearchCountriesCommand(IDocumentClient documentDbClient, ApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            CountryTranslator = new CountryToCountryTranslator(applicationUser);
        }

        public IDocumentClient DocumentDbClient { get; }
        public CountryToCountryTranslator CountryTranslator { get; }

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
