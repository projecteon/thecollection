using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCollection.Business.Tea;
using TheCollection.Web.Services;

namespace TheCollection.Web.Controllers
{
    [Route("api/[controller]")]
    public class BagTypesController : Controller
    {
        private readonly DocumentClient documentDbClient;

        public BagTypesController(DocumentClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
        }

        [HttpGet()]
        public async Task<IEnumerable<BagType>> Brands([FromQuery] string searchterm = "")
        {
            var bagtypesRepository = new DocumentDBRepository<BagType>(documentDbClient, "TheCollection", "BagTypes");
            IEnumerable<BagType> bagtypes;
            if (searchterm != "")
            {
                bagtypes = await bagtypesRepository.GetItemsAsync(brand => brand.Name.Contains(searchterm));
            }
            else
            {
                bagtypes = await bagtypesRepository.GetItemsAsync();
            }

            return bagtypes.OrderBy(bagtype => bagtype.Name);
        }
    }
}
