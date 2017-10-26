namespace TheCollection.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using System.Threading.Tasks;
    using TheCollection.Business.Tea;
    using TheCollection.Web.Commands;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Services;

    [Route("api/[controller]")]
    public class BagTypesController : Controller
    {
        private readonly IDocumentClient documentDbClient;
        private readonly IApplicationUserAccessor applicationUserAccessor;

        public BagTypesController(IDocumentClient documentDbClient, IApplicationUserAccessor applicationUserAccessor)
        {
            this.documentDbClient = documentDbClient;
            this.applicationUserAccessor = applicationUserAccessor;
        }

        [HttpGet()]
        public async Task<IActionResult> BagTypes([FromQuery] string searchterm = "")
        {
            var command = new SearchBagTypesCommand(documentDbClient);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm });
        }

        [HttpPost()]
        public BagType Create([FromBody] BagType bagType)
        {
            bagType.Id = System.Guid.NewGuid().ToString();
            return bagType;
        }

        [HttpPut()]
        public BagType Update([FromBody] BagType bagType)
        {
            return bagType;
        }

        [HttpGet, Route("refvalues/{searchterm:alpha}")]
        public async Task<IActionResult> RefValues([FromQuery] string searchterm = "")
        {
            var applicationUser = await applicationUserAccessor.GetUser();
            var command = new SearchRefValuesCommand<BagType>(documentDbClient, applicationUser, DocumentDB.BagTypesCollectionId);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm });
        }
    }
}
