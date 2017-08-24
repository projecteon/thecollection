using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Threading.Tasks;
using TheCollection.Business.Tea;
using TheCollection.Web.Commands;
using TheCollection.Web.Services;

namespace TheCollection.Web.Controllers
{
    [Route("api/[controller]")]
    public class BagsController : Controller
    {
        private readonly IDocumentClient documentDbClient;
        private readonly IApplicationUserAccessor applicationUserAccessor;

        public BagsController(IDocumentClient documentDbClient, IApplicationUserAccessor applicationUserAccessor)
        {
            this.documentDbClient = documentDbClient;
            this.applicationUserAccessor = applicationUserAccessor;
        }

        [HttpGet()]
        public async Task<IActionResult> Bags([FromQuery] string searchterm = "", [FromQuery] int pagesize = 300, [FromQuery] int page = 0)
        {
            var command = new SearchBagsCommand(documentDbClient);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm, pagesize = pagesize });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Bag(string id)
        {
            var command = new GetTeabagCommand(documentDbClient);
            return await command.ExecuteAsync(id);
        }

        [HttpPost()]
        public Bag Create([FromBody] Bag bag)
        {
            bag.Id = System.Guid.NewGuid().ToString();
            return bag;
        }

        [HttpPut()]
        public Bag Update([FromBody] Bag bag)
        {
            return bag;
        }
    }
}
