namespace TheCollection.Web.Commands.Tea {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain;
    using TheCollection.Domain.Tea;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;

    public class GetBagsCountByBagTypesCommand : IAsyncCommand {
        public GetBagsCountByBagTypesCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
        }

        public IDocumentClient DocumentDbClient { get; }
        public IApplicationUser ApplicationUser { get; }

        public async Task<IActionResult> ExecuteAsync() {
            var dashboardRepository = new GetRepository<Dashboard<IEnumerable<CountBy<Domain.RefValue>>>>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.Collections.Statistics);
            var bagsCountByBagType = await dashboardRepository.GetItemAsync(DashBoardTypes.BagsCountByBagTypes.Key.ToString());
            if (bagsCountByBagType == null) {
                return new NotFoundResult();
            }

            return new OkObjectResult(bagsCountByBagType.Data);
        }
    }
}
