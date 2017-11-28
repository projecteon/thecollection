namespace TheCollection.Web.Commands.Tea {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Business;
    using TheCollection.Business.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Data.DocumentDB.Repositories;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Models;

    public class CreateBagsCountByBagTypesCommand : IAsyncCommand {
        public CreateBagsCountByBagTypesCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
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
            var bagsCountByBrand = countGroupByIRef.GroupAndCountBy(x => x.BagType);
            var dashboard = new Dashboard<IEnumerable<CountBy<Business.RefValue>>>() { Id = DashBoardTypes.BagsCountByBagTypes.Key.ToString(), UserId = ApplicationUser.Id, DashboardType = DashBoardTypes.BagsCountByBagTypes, Data = bagsCountByBrand };
            var dashboardRepository = new UpsertRepository<Dashboard<IEnumerable<CountBy<Business.RefValue>>>>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsStatisticsCollectionId);
            await dashboardRepository.UpsertItemAsync(dashboard.Id, dashboard);
            return new OkObjectResult(bagsCountByBrand);
        }
    }
}
