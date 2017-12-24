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
            return await command.ExecuteAsync(new Search { searchterm = searchterm });
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] BagType bagType) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var entityTranslator = new BagTypeToBagTypeTranslator(applicationUser);
            var dtoTranslator = new BagTypeDtoToBagTypeTranslator(applicationUser);
            var command = new CreateCommand<Domain.Tea.BagType, BagType>(documentDbClient, applicationUser, entityTranslator, dtoTranslator);
            return await command.ExecuteAsync(bagType);
        }

        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] BagType bagType) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var entityTranslator = new BagTypeToBagTypeTranslator(applicationUser);
            var dtoTranslator = new BagTypeDtoToBagTypeTranslator(applicationUser);
            var command = new UpdateCommand<Domain.Tea.BagType, BagType>(documentDbClient, applicationUser, entityTranslator, dtoTranslator);
            return await command.ExecuteAsync(bagType);
        }

        [HttpGet, Route("refvalues/{searchterm:alpha}")]
        public async Task<IActionResult> RefValues([FromQuery] string searchterm = "") {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new SearchRefValuesCommand<Domain.Tea.BagType>(documentDbClient, applicationUser, DocumentDB.Collections.BagTypes);
            return await command.ExecuteAsync(new Search { searchterm = searchterm });
        }
    }
}
