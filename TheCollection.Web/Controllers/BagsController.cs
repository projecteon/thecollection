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
    public class BagsController : Controller
    {
        private readonly IDocumentClient documentDbClient;

        public BagsController(IDocumentClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
        }

        [HttpGet()]
        public async Task<SearchResult<Bag>> Bags([FromQuery] string searchterm = "", [FromQuery] int pagesize = 300, [FromQuery] int page = 0)
        {
            var bagsRepository = new DocumentDBRepository<Bag>(documentDbClient, "TheCollection", "Bags");
            IEnumerable<Bag> bags;
            if (searchterm != "")
            {
                bags = await bagsRepository.GetItemsAsync(searchterm, pagesize);
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

        [HttpPost()]
        public Bag Create([FromBody] Bag bag)
        {
            bag.Id = System.Guid.NewGuid().ToString();
            return bag;
        }

        [HttpPut()]
        public Bag Update([FromBody] Bag bag)
        {
            return bag;
        }
    }

    public class SearchResult<T>
    {
        public long count { get; set; }
        public IEnumerable<T> data { get; set; }
    }
}
