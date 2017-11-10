namespace TheCollection.Web.Commands.Tea {
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Business;
    using TheCollection.Business.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Models;

    public class GetBagsCountByBrandsCommand : IAsyncCommand {
        public GetBagsCountByBrandsCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
        }

        public IDocumentClient DocumentDbClient { get; }
        public IApplicationUser ApplicationUser { get; }

        public async Task<IActionResult> ExecuteAsync() {
            var bagsRepository = new SearchRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            var bags = await bagsRepository.SearchItemsAsync();
            var queryablebags = bags.AsQueryable();
            var countGroupByIRef = new CountGroupBy<Bag, Business.RefValue, RefValueComparer>(queryablebags);
            var bagsCountByBrand = countGroupByIRef.GroupAndCountBy(x => x.Brand)
                                                   .OrderByDescending(x => x.Count)
                                                   .Take(10)
                                                   .OrderBy(x => x.Value.Name);
            return new OkObjectResult(bagsCountByBrand);
        }
    }
}
