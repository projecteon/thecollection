namespace TheCollection.Web.Commands.Tea {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Translators.Tea;

    public class CreateCountryCommand : IAsyncCommand<Country> {
        public CreateCountryCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            CountryTranslator = new CountryToCountryTranslator(applicationUser);
        }

        IDocumentClient DocumentDbClient { get; }
        IApplicationUser ApplicationUser { get; }
        ITranslator<Country, Models.Tea.Country> CountryTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Country country) {
            if (country == null) {
                return new BadRequestObjectResult("Country cannot be null");
            }

            var brandRepository = new CreateRepository<Country>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Countries);
            country.Id = await brandRepository.CreateItemAsync(country);
            return new OkObjectResult(CountryTranslator.Translate(country));
        }
    }
}
