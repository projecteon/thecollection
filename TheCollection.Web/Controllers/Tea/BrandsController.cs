namespace TheCollection.Web.Controllers {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain.Contracts.Repository;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Commands;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Models;

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
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm });
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] Brand brand) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new CreateBrandCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(brand);
        }

        [HttpPut()]
        public Brand Update([FromBody] Brand brand) {
            return brand;
        }

        [HttpGet, Route("refvalues/{searchterm:alpha}")]
        public async Task<IActionResult> RefValues([FromQuery] string searchterm = "") {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new SearchRefValuesCommand<Brand>(documentDbClient, applicationUser, DocumentDB.BrandsCollectionId);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm });
        }
    }
}
