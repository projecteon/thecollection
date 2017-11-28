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

    public class CreateBagsCountByBrandsCommand : IAsyncCommand {
        public CreateBagsCountByBrandsCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
        }

        public IDocumentClient DocumentDbClient { get; }
        public IApplicationUser ApplicationUser { get; }

        public async Task<IActionResult> ExecuteAsync() {
            var bagsRepository = new SearchRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            var bags = await bagsRepository.SearchItemsAsync();
            var queryablebags = bags.AsQueryable();
            var strangeBug = queryablebags.Where(x => x.Brand == null);
            var countGroupByIRef = new CountGroupBy<Bag, Business.RefValue, RefValueComparer>(queryablebags);
            var bagsCountByBrand = countGroupByIRef.GroupAndCountBy(x => x.Brand)
                                                   .OrderByDescending(x => x.Count);

            var dashboard = new Dashboard<IEnumerable<CountBy<Business.RefValue>>>() { Id = "00ac062d-68f4-4c6e-9a60-044bf8c9077c", UserId = ApplicationUser.Id, Data = bagsCountByBrand };
            var dashboardRepository = new UpsertRepository<Dashboard<IEnumerable<CountBy<Business.RefValue>>>>(DocumentDbClient, DocumentDB.DatabaseId, "DashboardCountBy");
            await dashboardRepository.UpsertItemAsync(dashboard.Id, dashboard);

            return new OkObjectResult(bagsCountByBrand);
        }
    }
}

