using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCollection.Business.Tea;
using TheCollection.Web.Commands;
using TheCollection.Web.Constants;
using TheCollection.Web.Services;

namespace TheCollection.Web.Controllers
{
    [Route("api/[controller]")]
    public class BagTypesController : Controller
    {
        private readonly IDocumentClient documentDbClient;

        public BagTypesController(IDocumentClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
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
    }
}
