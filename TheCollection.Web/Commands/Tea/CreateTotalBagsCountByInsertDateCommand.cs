namespace TheCollection.Web.Commands.Tea {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Data.DocumentDB.Repositories;
    using TheCollection.Domain;
    using TheCollection.Domain.Extensions;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;

    public class CreateTotalBagsCountByInsertDateCommand {
        public CreateTotalBagsCountByInsertDateCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
        }

        public IDocumentClient DocumentDbClient { get; }
        public IApplicationUser ApplicationUser { get; }

        public async Task<IActionResult> ExecuteAsync() {
            var bagsRepository = new SearchRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Bags);
            var bags = await bagsRepository.SearchItemsAsync();
            var queryablebags = bags.AsQueryable();
            var countGroupBy = new CountGroupBy<Bag, NodaTime.LocalDate, LocalDateComparer>(queryablebags);
            var bagsCountByInsertDate = countGroupBy.GroupAndCountBy(x => x.InsertDate.With(NodaTime.DateAdjusters.StartOfMonth))
                                                       .OrderBy(x => x.Value);

            var firstCountBy = bagsCountByInsertDate.First();
            var firstPeriod = firstCountBy.Value.With(NodaTime.DateAdjusters.StartOfMonth).PlusMonths(-1);
            var totalBagsCountByPeriods = bagsCountByInsertDate.ToList().Scan((state, item) => {
                return new CountBy<NodaTime.LocalDate>(item.Value, item.Count + state.Count);
            }, new CountBy<NodaTime.LocalDate>(firstPeriod, 0));

            var dashboard = new Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>() { Id = DashBoardTypes.TotalBagsCountByPeriod.Key.ToString(), UserId = ApplicationUser.Id, DashboardType = DashBoardTypes.TotalBagsCountByPeriod, Data = totalBagsCountByPeriods };
            var dashboardRepository = new UpsertRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Statistics);
            await dashboardRepository.UpsertItemAsync(dashboard.Id, dashboard);

            return new OkObjectResult(totalBagsCountByPeriods);
        }
    }
}
