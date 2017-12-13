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
        public BagType Create([FromBody] BagType bagType) {
            bagType.Id = System.Guid.NewGuid().ToString();
            return bagType;
        }

        [HttpPut()]
        public BagType Update([FromBody] BagType bagType) {
            return bagType;
        }

        [HttpGet, Route("refvalues/{searchterm:alpha}")]
        public async Task<IActionResult> RefValues([FromQuery] string searchterm = "") {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new SearchRefValuesCommand<BagType>(documentDbClient, applicationUser, DocumentDB.BagTypesCollectionId);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm });
        }
    }
}
