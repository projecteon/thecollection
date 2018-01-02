namespace TheCollection.Web.Commands.Tea {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain;
    using TheCollection.Domain.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;

    public class GetBagsCountByBrandsCommand : IAsyncCommand<int> {
        public GetBagsCountByBrandsCommand(IDocumentClient documentDbClient, IWebUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
        }

        public IDocumentClient DocumentDbClient { get; }
        public IWebUser ApplicationUser { get; }

        public async Task<IActionResult> ExecuteAsync(int top = 10) {
            var dashboardRepository = new GetRepository<Dashboard<IEnumerable<CountBy<Domain.RefValue>>>>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Statistics);
            var bagsCountByBrand = await dashboardRepository.GetItemAsync(DashBoardTypes.BagsCountByBrands.Key.ToString());
            if (bagsCountByBrand == null) {
                return new NotFoundResult();
            }

            return new OkObjectResult(bagsCountByBrand.Data.Take(top).OrderBy(x => x.Value?.Name));
        }
    }
}
