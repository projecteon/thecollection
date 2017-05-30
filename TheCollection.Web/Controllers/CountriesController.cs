using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCollection.Web.Models;
using TheCollection.Web.Services;

namespace TheCollection.Web.Controllers
{
    [Route("api/[controller]")]
    public class CountriesController : Controller
    {
        private readonly DocumentClient documentDbClient;

        public CountriesController(DocumentClient documentDbClient)
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
