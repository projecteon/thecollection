namespace TheCollection.Web.Controllers {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain.Contracts.Repository;
    using TheCollection.Web.Commands;
    using TheCollection.Web.Commands.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Models;
    using TheCollection.Web.Models.Tea;
    using TheCollection.Web.Translators.Tea;

    [Route("api/Tea/[controller]")]
    public class CountriesController : Controller {
        private readonly IDocumentClient documentDbClient;
        private readonly IGetRepository<ApplicationUser> applicationUserRepository;

        public CountriesController(IDocumentClient documentDbClient, IGetRepository<ApplicationUser> applicationUserRepository) {
            this.documentDbClient = documentDbClient;
            this.applicationUserRepository = applicationUserRepository;
        }

        [HttpGet()]
        public async Task<IActionResult> Countries([FromQuery] string searchterm = "") {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new SearchCountriesCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(new Search { searchterm = searchterm });
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] Country country) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var entityTranslator = new CountryToCountryTranslator(applicationUser);
            var dtoTranslator = new CountryDtoToCountryTranslator(applicationUser);
            var command = new CreateCommand<Domain.Tea.Country, Country>(documentDbClient, applicationUser, entityTranslator, dtoTranslator);
            return await command.ExecuteAsync(country);
        }

        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] Country country) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var entityTranslator = new CountryToCountryTranslator(applicationUser);
            var dtoTranslator = new CountryDtoToCountryTranslator(applicationUser);
            var command = new UpdateCommand<Domain.Tea.Country, Country>(documentDbClient, applicationUser, entityTranslator, dtoTranslator);
            return await command.ExecuteAsync(country);
        }

        [HttpGet, Route("refvalues/{searchterm:alpha}")]
        public async Task<IActionResult> RefValues([FromQuery] string searchterm = "") {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new SearchRefValuesCommand<Domain.Tea.Country>(documentDbClient, applicationUser, DocumentDB.Collections.Countries);
            return await command.ExecuteAsync(new Search { searchterm = searchterm });
        }
    }
}
