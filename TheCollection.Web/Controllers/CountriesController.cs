namespace TheCollection.Web.Controllers {

    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Business.Tea;
    using TheCollection.Web.Commands;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Services;

    [Route("api/[controller]")]
    public class CountriesController : Controller {
        private readonly IDocumentClient documentDbClient;
        private readonly IApplicationUserAccessor applicationUserAccessor;

        public CountriesController(IDocumentClient documentDbClient, IApplicationUserAccessor applicationUserAccessor) {
            this.documentDbClient = documentDbClient;
            this.applicationUserAccessor = applicationUserAccessor;
        }

        [HttpGet()]
        public async Task<IActionResult> Countries([FromQuery] string searchterm = "") {
            var applicationUser = await applicationUserAccessor.GetUser();
            var command = new SearchCountriesCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm });
        }

        [HttpPost()]
        public Country Create([FromBody] Country country) {
            country.Id = System.Guid.NewGuid().ToString();
            return country;
        }

        [HttpPut()]
        public Country Update([FromBody] Country country) {
            return country;
        }

        [HttpGet, Route("refvalues/{searchterm:alpha}")]
        public async Task<IActionResult> RefValues([FromQuery] string searchterm = "") {
            var applicationUser = await applicationUserAccessor.GetUser();
            var command = new SearchRefValuesCommand<Country>(documentDbClient, applicationUser, DocumentDB.CountriesCollectionId);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm });
        }
    }
}
