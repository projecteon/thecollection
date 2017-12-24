namespace TheCollection.Web.Controllers {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain.Contracts.Repository;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Commands;
    using TheCollection.Web.Commands.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Models;

    [Route("api/Tea/[controller]")]
    public class BagTypesController : Controller {
        private readonly IDocumentClient documentDbClient;
        private readonly IGetRepository<ApplicationUser> applicationUserRepository;

        public BagTypesController(IDocumentClient documentDbClient, IGetRepository<ApplicationUser> applicationUserRepository) {
            this.documentDbClient = documentDbClient;
            this.applicationUserRepository = applicationUserRepository;
        }

        [HttpGet()]
        public async Task<IActionResult> BagTypes([FromQuery] string searchterm = "") {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new SearchBagTypesCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm });
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] BagType bagType) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new CreateBagTypeCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(bagType);
        }

        [HttpPut()]
        public BagType Update([FromBody] BagType bagType) {
            return bagType;
        }

        [HttpGet, Route("refvalues/{searchterm:alpha}")]
        public async Task<IActionResult> RefValues([FromQuery] string searchterm = "") {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new SearchRefValuesCommand<BagType>(documentDbClient, applicationUser, DocumentDB.Collections.BagTypes);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm });
        }
    }
}
