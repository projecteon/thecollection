namespace TheCollection.Web.Commands.Tea {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Business;
    using TheCollection.Business.Extensions;
    using TheCollection.Business.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Data.DocumentDB.Repositories;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Models;

    public class CreateTotalBagsCountByPeriodsCommand {
        public CreateTotalBagsCountByPeriodsCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
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

            var firstCountByPeriod = bagsCountByPeriods.First();
            var firstPeriod = (new DateTime(firstCountByPeriod.Value.Year, firstCountByPeriod.Value.Month, 1)).AddMonths(-1);
            var totalBagsCountByPeriods = bagsCountByPeriods.ToList().Scan((state, item) => {
                return new CountBy<Period>(item.Value, item.Count + state.Count);
            }, new CountBy<Period>(new Period(firstPeriod.Year, firstPeriod.Month), 0));

            var dashboard = new Dashboard<IEnumerable<CountBy<Period>>>() { Id = DashBoardTypes.TotalBagsCountByPeriod.Key.ToString(), UserId = ApplicationUser.Id, DashboardType = DashBoardTypes.TotalBagsCountByPeriod, Data=totalBagsCountByPeriods };
            var dashboardRepository = new UpsertRepository<Dashboard<IEnumerable<CountBy<Period>>>>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsStatisticsCollectionId);
            await dashboardRepository.UpsertItemAsync(dashboard.Id, dashboard);

            return new OkObjectResult(totalBagsCountByPeriods);
        }
    }
}
