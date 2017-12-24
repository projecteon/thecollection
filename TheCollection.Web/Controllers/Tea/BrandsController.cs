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

    [Route("api/Tea/[controller]")]
    public class BrandsController : Controller {
        private readonly IDocumentClient documentDbClient;
        private readonly IGetRepository<ApplicationUser> applicationUserRepository;

        public BrandsController(IDocumentClient documentDbClient, IGetRepository<ApplicationUser> applicationUserRepository) {
            this.documentDbClient = documentDbClient;
            this.applicationUserRepository = applicationUserRepository;
        }

        [HttpGet()]
        public async Task<IActionResult> Brands([FromQuery] string searchterm = "") {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new SearchBrandsCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(new Search { searchterm = searchterm });
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] Brand brand) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new CreateBrandCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(brand);
        }

        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] Brand brand) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new UpdateBrandCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(brand);
        }

        [HttpGet, Route("refvalues/{searchterm:alpha}")]
        public async Task<IActionResult> RefValues([FromQuery] string searchterm = "") {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new SearchRefValuesCommand<Domain.Tea.Brand>(documentDbClient, applicationUser, DocumentDB.Collections.Brands);
            return await command.ExecuteAsync(new Search { searchterm = searchterm });
        }
    }
}
