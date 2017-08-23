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
    public class CountriesController : Controller
    {
        private readonly IDocumentClient documentDbClient;

        public CountriesController(IDocumentClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
        }

        [HttpGet()]
        public async Task<IActionResult> Countries([FromQuery] string searchterm = "")
        {
            var command = new SearchCountriesCommand(documentDbClient);
            return await command.ExecuteAsync(new Models.Search { searchterm = searchterm });
        }

        [HttpPost()]
        public Country Create([FromBody] Country country)
        {
            country.Id = System.Guid.NewGuid().ToString();
            return country;
        }

        [HttpPut()]
        public Country Update([FromBody] Country country)
        {
            return country;
        }
    }
}
