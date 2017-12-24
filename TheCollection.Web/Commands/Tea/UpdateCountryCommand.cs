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

    public class UpdateCountryCommand : IAsyncCommand<Models.Tea.Country> {
        public UpdateCountryCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            CountryTranslator = new CountryToCountryTranslator(applicationUser);
            CountryDtoTranslator = new CountryDtoToCountryTranslator(applicationUser);
        }

        IDocumentClient DocumentDbClient { get; }
        IApplicationUser ApplicationUser { get; }
        ITranslator<Country, Models.Tea.Country> CountryTranslator { get; }
        ITranslator<Models.Tea.Country, Country> CountryDtoTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Models.Tea.Country country) {
            if (country == null) {
                return new BadRequestObjectResult("Country cannot be null");
            }

            var updateRepository = new UpdateRepository<Country>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Countries);
            var updateCountry = CountryDtoTranslator.Translate(country);
            updateCountry.Id = await updateRepository.UpdateItemAsync(country.id, updateCountry);
            return new OkObjectResult(CountryTranslator.Translate(updateCountry));
        }
    }
}
