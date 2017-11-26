namespace TheCollection.Web.Controllers.Tea {
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using System.Threading.Tasks;
    using TheCollection.Web.Commands.Tea;
    using TheCollection.Web.Services;

    [Route("api/tea/[controller]")]
    public class DashboardsController : Controller {

        private readonly IDocumentClient documentDbClient;
        private readonly IApplicationUserAccessor applicationUserAccessor;

        public DashboardsController(IDocumentClient documentDbClient, IApplicationUserAccessor applicationUserAccessor) {
            this.documentDbClient = documentDbClient;
            this.applicationUserAccessor = applicationUserAccessor;
        }

        [HttpGet("BagTypes")]
        public async Task<IActionResult> BagTypes() {
            var applicationUser = await applicationUserAccessor.GetUser();
            var command = new GetBagsCountByBagTypesCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync();
        }

        [HttpGet("Brands/{top:int?}")]
        public async Task<IActionResult> Brands(int top = 10) {
            var applicationUser = await applicationUserAccessor.GetUser();
            var command = new GetBagsCountByBrandsCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(top);
        }

        [HttpGet("Periods")]
        public async Task<IActionResult> Periods() {
            var applicationUser = await applicationUserAccessor.GetUser();
            var command = new GetBagsCountByPeriodsCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync();
        }

        [HttpGet("TotalCountPeriod")]
        public async Task<IActionResult> TotalCountPeriod() {
            var applicationUser = await applicationUserAccessor.GetUser();
            var command = new GetTotalBagsCountByPeriodsCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync();
        }
    }
}
