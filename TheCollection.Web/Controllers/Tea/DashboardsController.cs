namespace TheCollection.Web.Controllers.Tea {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using TheCollection.Domain.Contracts.Repository;
    using TheCollection.Web.Commands.Tea;
    using TheCollection.Web.Models;

    [Route("api/Tea/[controller]")]
    public class DashboardsController : Controller {
        private readonly IDocumentClient documentDbClient;
        private readonly IGetRepository<ApplicationUser> applicationUserRepository;

        public DashboardsController(IDocumentClient documentDbClient, IGetRepository<ApplicationUser> applicationUserRepository) {
            this.documentDbClient = documentDbClient;
            this.applicationUserRepository = applicationUserRepository;
        }

        [HttpGet("BagTypes/{top:int?}")]
        public async Task<IActionResult> BagTypes(int top = 10) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new GetBagsCountByBagTypesCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync();
        }

        [HttpPut("BagTypes/{top:int?}")]
        public async Task<IActionResult> CreateBagTypes(int top = 10) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new CreateBagsCountByBagTypesCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync();
        }

        [HttpGet("Brands/{top:int?}")]
        public async Task<IActionResult> Brands(int top = 10) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new GetBagsCountByBrandsCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(top);
        }

        [HttpPut("Brands/{top:int?}")]
        public async Task<IActionResult> CreateBrands(int top = 10) {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new CreateBagsCountByBrandsCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync(top);
        }

        [HttpGet("Periods")]
        public async Task<IActionResult> Periods() {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new GetBagsCountByInsertDateCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync();
        }

        [HttpPut("Periods")]
        public async Task<IActionResult> CreatePeriods() {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new CreateBagsCountByInsertDateCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync();
        }

        [HttpGet("TotalCountPeriod")]
        public async Task<IActionResult> TotalCountPeriod() {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new GetTotalBagsCountByInsertDateCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync();
        }

        [HttpPut("TotalCountPeriod")]
        public async Task<IActionResult> CreateTotalCountPeriod() {
            var applicationUser = await applicationUserRepository.GetItemAsync();
            var command = new CreateTotalBagsCountByInsertDateCommand(documentDbClient, applicationUser);
            return await command.ExecuteAsync();
        }
    }
}
