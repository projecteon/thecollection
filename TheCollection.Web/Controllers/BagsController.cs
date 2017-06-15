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
    public class BagsController : Controller
    {
        private readonly DocumentClient documentDbClient;

        public BagsController(DocumentClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
        }

        [HttpGet()]
        public async Task<SearchResult<Bag>> Bags([FromQuery] string searchterm = "")
        {
            var bagsRepository = new DocumentDBRepository<Bag>(documentDbClient, "TheCollection", "Bags");
            IEnumerable<Bag> bags;
            if (searchterm != "")
            {
                bags = await bagsRepository.GetItemsAsync(searchterm, 300);
            }
            else
            {
                bags = await bagsRepository.GetItemsAsync();
            }

            return new SearchResult<Bag>
            {
                count = await bagsRepository.GetRowCountAsync(searchterm),
                data = bags.OrderBy(bag => bag.Brand.Name)
                        .ThenBy(bag => bag.Hallmark)
                        .ThenBy(bag => bag.Serie)
                        .ThenBy(bag => bag.BagType?.Name)
                        .ThenBy(bag => bag.Flavour)
            };
        }

        [HttpGet("{id}")]
        public async Task<Bag> Bag(string id)
        {
            var bagsRepository = new DocumentDBRepository<Bag>(documentDbClient, "TheCollection", "Bags");
            return await bagsRepository.GetItemAsync(id);
        }
    }

    public class SearchResult<T>
    {
        public long count { get; set; }
        public IEnumerable<T> data { get; set; }
    }
}
