namespace TheCollection.Web.Commands.Tea {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Data.DocumentDB;
    using TheCollection.Domain;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Models;

    public class GetTotalBagsCountByInsertDateCommand : IAsyncCommand {
        public GetTotalBagsCountByInsertDateCommand(IDocumentClient documentDbClient, IApplicationUser applicationUser) {
            DocumentDbClient = documentDbClient;
            ApplicationUser = applicationUser;
        }

        public IDocumentClient DocumentDbClient { get; }
        public IApplicationUser ApplicationUser { get; }

        public async Task<IActionResult> ExecuteAsync() {
            var dashboardRepository = new GetRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>>(DocumentDbClient, DocumentDB.DatabaseId, DocumentDB.BagsStatisticsCollectionId);
            var totalBagsCountByPeriods = await dashboardRepository.GetItemAsync(DashBoardTypes.TotalBagsCountByPeriod.Key.ToString());
            if (totalBagsCountByPeriods == null) {
                return new NotFoundResult();
            }

            return new OkObjectResult(totalBagsCountByPeriods.Data);
        }
    }
}