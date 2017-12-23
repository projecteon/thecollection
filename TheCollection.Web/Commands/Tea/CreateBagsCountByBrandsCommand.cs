namespace TheCollection.Web.Commands.Tea {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain;
    using TheCollection.Domain.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Data.DocumentDB.Repositories;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;

    public class CreateBagsCountByBrandsCommand : IAsyncCommand<int> {
        public CreateBagsCountByBrandsCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
        }

        public IDocumentClient DocumentDbClient { get; }
        public IApplicationUser ApplicationUser { get; }

        public async Task<IActionResult> ExecuteAsync(int top) {
            var bagsRepository = new SearchRepository<Bag>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsCollectionId);
            var bags = await bagsRepository.SearchItemsAsync();
            var queryablebags = bags.AsQueryable();
            var strangeBug = queryablebags.Where(x => x.Brand == null);
            var countGroupByIRef = new CountGroupBy<Bag, Domain.RefValue, RefValueComparer>(queryablebags);
            var bagsCountByBrand = countGroupByIRef.GroupAndCountBy(x => x.Brand)
                                                   .OrderByDescending(x => x.Count);

            var dashboard = new Dashboard<IEnumerable<CountBy<Domain.RefValue>>>() { Id = DashBoardTypes.BagsCountByBrands.Key.ToString(), UserId = ApplicationUser.Id, DashboardType = DashBoardTypes.BagsCountByBrands, Data = bagsCountByBrand };
            var dashboardRepository = new UpsertRepository<Dashboard<IEnumerable<CountBy<Domain.RefValue>>>>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsStatisticsCollectionId);
            await dashboardRepository.UpsertItemAsync(dashboard.Id, dashboard);

            return new OkObjectResult(bagsCountByBrand.Take(top).OrderBy(x => x.Value?.Name));
        }
    }
}

