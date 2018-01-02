namespace TheCollection.Web.Controllers {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Application.Services;
    using TheCollection.Application.Services.Commands;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Application.Services.Contracts.Repository;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Web.Commands.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Models;
    using TheCollection.Web.Models.Tea;

    [Route("api/Tea/[controller]")]
    public class BagsController : Controller {
        private readonly IDocumentClient documentDbClient;
        private readonly IGetRepository<WebUser> applicationUserRepository;

        public BagsController(IDocumentClient documentDbClient, IGetRepository<WebUser> applicationUserRepository) {
            this.documentDbClient = documentDbClient;
            this.applicationUserRepository = applicationUserRepository;
        }

        [HttpGet()]
        public async Task<IActionResult> Bags([FromQuery] string searchterm = "", [FromQuery] int pagesize = 300, [FromQuery] int page = 0) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new SearchBagsCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(new Search { Searchterm = searchterm, Pagesize = pagesize });
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
        public async Task<IActionResult> Update([FromBody] Bag bag) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new UpdateBagCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(bag);
        }
    }
}
