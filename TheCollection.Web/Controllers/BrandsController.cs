using Microsoft.AspNetCore.Mvc;

namespace TheCollection.Web.Controllers {

    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using TheCollection.Business.Tea;
    using TheCollection.Web.Commands;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Services;

    [Route("api/[controller]")]
    public class BrandsController : Controller {
        private readonly IDocumentClient documentDbClient;
        private readonly IApplicationUserAccessor applicationUserAccessor;

        public BrandsController(IDocumentClient documentDbClient, IApplicationUserAccessor applicationUserAccessor) {
            this.documentDbClient = documentDbClient;
            this.applicationUserAccessor = applicationUserAccessor;
        }

        [HttpGet()]
        public async Task<IActionResult> Brands([FromQuery] string searchterm = "") {
            var applicationUser = await applicationUserAccessor.GetUser();
            var command = new SearchBrandsCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm });
        }

        [HttpPost()]
        public Brand Create([FromBody] Brand brand) {
            brand.Id = System.Guid.NewGuid().ToString();
            return brand;
        }

        [HttpPut()]
        public Brand Update([FromBody] Brand brand) {
            return brand;
        }

        [HttpGet, Route("refvalues/{searchterm:alpha}")]
        public async Task<IActionResult> RefValues([FromQuery] string searchterm = "") {
            var applicationUser = await applicationUserAccessor.GetUser();
            var command = new SearchRefValuesCommand<Brand>(documentDbClient, applicationUser, DocumentDB.BrandsCollectionId);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm });
        }
    }
}
