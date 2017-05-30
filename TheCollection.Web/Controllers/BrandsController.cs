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
    public class BrandsController : Controller
    {
        private readonly DocumentClient documentDbClient;

        public BrandsController(DocumentClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
        }

        [HttpGet()]
        public async Task<IEnumerable<Brand>> Brands([FromQuery] string searchterm = "")
        {
            var brandsRepository = new DocumentDBRepository<Brand>(documentDbClient, "TheCollection", "Brands");
            IEnumerable<Brand> brands;
            if (searchterm != "")
            {
                brands = await brandsRepository.GetItemsAsync(brand => brand.Name.Contains(searchterm));
            }
            else
            {
                brands = await brandsRepository.GetItemsAsync();
            }

            return brands.OrderBy(brand => brand.Name);
        }
    }
}
