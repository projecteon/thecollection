namespace TheCollection.Web.Commands.Tea {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Business;
    using TheCollection.Business.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Models;

    public class GetBagsCountByBagTypesCommand : IAsyncCommand {
        public GetBagsCountByBagTypesCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
        }

        public IDocumentClient DocumentDbClient { get; }
        public IApplicationUser ApplicationUser { get; }

        public async Task<IActionResult> ExecuteAsync() {
            var dashboardRepository = new GetRepository<Dashboard<IEnumerable<CountBy<Business.RefValue>>>>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsStatisticsCollectionId);
            var bagsCountByBagType = await dashboardRepository.GetItemAsync(DashBoardTypes.BagsCountByBagTypes.Key.ToString());
            if (bagsCountByBagType == null) {
                return new NotFoundResult();
            }

            return new OkObjectResult(bagsCountByBagType.Data);
        }
    }
}
