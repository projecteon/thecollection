namespace TheCollection.Web.Commands.Tea {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Data.DocumentDB.Repositories;
    using TheCollection.Domain;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;

    public class CreateBagsCountByInsertDateCommand : IAsyncCommand {
        public CreateBagsCountByInsertDateCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
        }

        public IDocumentClient DocumentDbClient { get; }
        public IApplicationUser ApplicationUser { get; }

        public async Task<IActionResult> ExecuteAsync() {
            var bagsRepository = new SearchRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Bags);
            var bags = await bagsRepository.SearchItemsAsync();
            var queryablebags = bags.AsQueryable();

            var countGroupByLocalDate = new CountGroupBy<Bag, NodaTime.LocalDate, LocalDateComparer>(queryablebags);
            var bagsCountByLocalDate = countGroupByLocalDate.GroupAndCountBy(x => x.InsertDate.With(NodaTime.DateAdjusters.StartOfMonth))
                                                            .OrderByDescending(x => x.Value);

            var dashboard = new Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>() { Id = DashBoardTypes.BagsCountByPeriod.Key.ToString(), UserId = ApplicationUser.Id, DashboardType = DashBoardTypes.BagsCountByPeriod, Data = bagsCountByLocalDate };
            var dashboardRepository = new UpsertRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Statistics);
            await dashboardRepository.UpsertItemAsync(dashboard.Id, dashboard);

            return new OkObjectResult(bagsCountByLocalDate);
        }
    }
}
