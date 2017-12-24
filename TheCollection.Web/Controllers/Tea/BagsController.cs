namespace TheCollection.Web.Controllers {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain.Contracts.Repository;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Commands.Tea;
    using TheCollection.Web.Models;

    [Route("api/Tea/[controller]")]
    public class BagsController : Controller {
        private readonly IDocumentClient documentDbClient;
        private readonly IGetRepository<ApplicationUser> applicationUserRepository;

        public BagsController(IDocumentClient documentDbClient, IGetRepository<ApplicationUser> applicationUserRepository) {
            this.documentDbClient = documentDbClient;
            this.applicationUserRepository = applicationUserRepository;
        }

        [HttpGet()]
        public async Task<IActionResult> Bags([FromQuery] string searchterm = "", [FromQuery] int pagesize = 300, [FromQuery] int page = 0) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new SearchBagsCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm, pagesize = pagesize });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Bag(string id) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new GetBagCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(id);
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] Bag bag) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new CreateBagCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(bag);
        }

        [HttpPut()]
        public async Task<Bag> Update([FromBody] Bag bag) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            return bag;
        }
    }
}
