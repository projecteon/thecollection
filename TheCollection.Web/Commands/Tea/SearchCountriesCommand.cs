namespace TheCollection.Web.Commands.Tea {
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Domain.Extensions;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Models;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Translators.Tea;

    public class SearchCountriesCommand {
        public SearchCountriesCommand(IDocumentClient documentDbClient, IWebUser applicationUser) {
            DocumentDbClient = documentDbClient;
            CountryTranslator = new CountryToCountryTranslator(applicationUser);
        }

        IDocumentClient DocumentDbClient { get; }
        ITranslator<Country, Models.Tea.Country> CountryTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Search search) {
            if (search.Searchterm.IsNullOrWhiteSpace()) {
                return new BadRequestResult();
            }

            var countriesRepository = new SearchRepository<Country>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Countries);
            var countries = await countriesRepository.SearchAsync(search.Searchterm);
            var sortedcountries = countries.OrderBy(country => country.Name);

            return new OkObjectResult(CountryTranslator.Translate(sortedcountries));
        }
    }
}
