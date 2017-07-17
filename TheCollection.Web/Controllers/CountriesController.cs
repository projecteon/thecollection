using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCollection.Business.Tea;
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
        public async Task<IEnumerable<Country>> Countries([FromQuery] string searchterm = "")
        {
            var brandsRepository = new DocumentDBRepository<Country>(documentDbClient, "TheCollection", "Countries");
            IEnumerable<Country> countries;
            if (searchterm != "")
            {
                countries = await brandsRepository.GetItemsAsync(brand => brand.Name.Contains(searchterm));
            }
            else
            {
                countries = await brandsRepository.GetItemsAsync();
            }
            return countries.OrderBy(country => country.Name);
        }
    }
}
