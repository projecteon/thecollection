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

    public class GetBagsCountByPeriodsCommand : IAsyncCommand {
        public GetBagsCountByPeriodsCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
        }

        public IDocumentClient DocumentDbClient { get; }
        public IApplicationUser ApplicationUser { get; }

        public async Task<IActionResult> ExecuteAsync() {
            var bagsRepository = new SearchRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            var bags = await bagsRepository.SearchItemsAsync();
            var queryablebags = bags.AsQueryable();
            var countGroupByPeriod = new CountGroupBy<Bag, Period, PeriodComparer>(queryablebags);
            var bagsCountByPeriods = countGroupByPeriod.GroupAndCountBy(x => new Period(x.InsertDate))
                                                       .OrderByDescending(x => x.Value.Year)
                                                       .ThenByDescending(x => x.Value.Month);
            return new OkObjectResult(bagsCountByPeriods);
        }
    }
}
