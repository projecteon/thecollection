using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Linq;
using System.Threading.Tasks;
using TheCollection.Business.Tea;
using TheCollection.Web.Constants;
using TheCollection.Web.Extensions;
using TheCollection.Web.Models;
using TheCollection.Web.Services;

namespace TheCollection.Web.Commands
{
    public class SearchBagsCommand : IAsyncCommand<Search>
    {
        private readonly IDocumentClient documentDbClient;

        public SearchBagsCommand(IDocumentClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
        }

        public async Task<IActionResult> ExecuteAsync(Search search)
        {
            if (search.searchterm.IsNullOrWhiteSpace())
            {
                return new BadRequestResult();
            }

            var bagsRepository = new SearchRepository<Bag>(documentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            var bags = await bagsRepository.SearchAsync(search.searchterm, search.pagesize);
            var result = new SearchResult<Bag>
            {
                count = await bagsRepository.SearchRowCountAsync(search.searchterm),
                data = bags.OrderBy(bag => bag.Brand.Name)
                        .ThenBy(bag => bag.Hallmark)
                        .ThenBy(bag => bag.Serie)
                        .ThenBy(bag => bag.BagType?.Name)
                        .ThenBy(bag => bag.Flavour)
            };

            return new OkObjectResult(result);
        }
    }
}