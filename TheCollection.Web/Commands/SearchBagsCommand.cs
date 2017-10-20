namespace TheCollection.Web.Commands
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Business.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Models;
    using TheCollection.Web.Services;
    using TheCollection.Web.Translators;
    using TheCollection.Web.Translators.Tea;

    public class SearchBagsCommand : IAsyncCommand<Search>
    {
        public SearchBagsCommand(IDocumentClient documentDbClient)
        {
            this.DocumentDbClient = documentDbClient;
            BagTranslator = new BagToBagTranslator();
        }

        public IDocumentClient DocumentDbClient { get; }
        public ITranslator<Bag, Models.Tea.Bag> BagTranslator { get; }

        public async Task<IActionResult> ExecuteAsync(Search search)
        {
            if (search.searchterm.IsNullOrWhiteSpace())
            {
                return new BadRequestResult();
            }

            var bagsRepository = new SearchRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            var bags = await bagsRepository.SearchAsync(search.searchterm, search.pagesize);
            var sortedbags = bags.OrderBy(bag => bag.Brand.Name)
                                 .ThenBy(bag => bag.Serie)
                                 .ThenBy(bag => bag.Hallmark)
                                 .ThenBy(bag => bag.BagType?.Name)
                                 .ThenBy(bag => bag.Flavour);
            var result = new SearchResult<Models.Tea.Bag>
            {
                count = await bagsRepository.SearchRowCountAsync(search.searchterm),
                data = BagTranslator.Translate(sortedbags)
            };

            return new OkObjectResult(result);
        }
    }
}