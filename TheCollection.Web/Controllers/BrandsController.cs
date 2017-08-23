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
    public class BrandsController : Controller
    {
        private readonly IDocumentClient documentDbClient;

        public BrandsController(IDocumentClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
        }

        [HttpGet()]
        public async Task<IActionResult> Brands([FromQuery] string searchterm = "")
        {
            var command = new SearchBrandsCommand(documentDbClient);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm });
        }

        [HttpPost()]
        public Brand Create([FromBody] Brand brand)
        {
            brand.Id = System.Guid.NewGuid().ToString();
            return brand;
        }

        [HttpPut()]
        public Brand Update([FromBody] Brand brand)
        {
            return brand;
        }
    }
}
