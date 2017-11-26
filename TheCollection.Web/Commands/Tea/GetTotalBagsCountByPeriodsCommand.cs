namespace TheCollection.Web.Commands.Tea {
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Business;
    using TheCollection.Business.Extensions;
    using TheCollection.Business.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Models;

    public class GetTotalBagsCountByPeriodsCommand : IAsyncCommand {
        public GetTotalBagsCountByPeriodsCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
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
                                                       .OrderBy(x => x.Value.Year)
                                                       .ThenBy(x => x.Value.Month);

            var totalBagsCountByPeriods = bagsCountByPeriods.ToList().Scan((state, item) => {
                return new CountGroupBy<Bag, Period, PeriodComparer>.CountBy(item.Value, item.Count + state.Count);
            }, new CountGroupBy<Bag, Period, PeriodComparer>.CountBy(null, 0)).Skip(1);

            return new OkObjectResult(totalBagsCountByPeriods);
        }
    }
}
