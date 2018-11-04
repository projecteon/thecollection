namespace TheCollection.Api.Controllers.Tea {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using TheCollection.Application.Services.Commands.Tea;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Application.Services.Queries.Tea;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    [Produces("application/json")]
    [Route("api/Tea/[controller]")]
    public class DashboardsController : Controller {
        public DashboardsController(IGetRepository<IApplicationUser> applicationUserRepository,
                IAsyncQueryHandler<BagsCountByBagTypesQuery> bagTypesCountQuery,
                IAsyncQueryHandler<BagsCountByBrandsQuery> brandsCountQuery,
                IAsyncQueryHandler<BagsCountByInsertDateQuery> insertDateCountQuery,
                IAsyncQueryHandler<TotalBagsCountByInsertDateQuery> totalInsertDateCountQuery,
                ITranslator<IQueryResult, IActionResult> queryTranslator,
                ITranslator<ICommandResult, IActionResult> commandTranslator) {
            ApplicationUserRepository = applicationUserRepository ?? throw new System.ArgumentNullException(nameof(applicationUserRepository));
            BagTypesCountQuery = bagTypesCountQuery ?? throw new System.ArgumentNullException(nameof(bagTypesCountQuery));
            BrandsCountQuery = brandsCountQuery ?? throw new System.ArgumentNullException(nameof(brandsCountQuery));
            InsertDateCountQuery = insertDateCountQuery ?? throw new System.ArgumentNullException(nameof(insertDateCountQuery));
            TotalInsertDateCountQuery = totalInsertDateCountQuery ?? throw new System.ArgumentNullException(nameof(totalInsertDateCountQuery));
            QueryTranslator = queryTranslator ?? throw new System.ArgumentNullException(nameof(queryTranslator));
            CommandTranslator = commandTranslator ?? throw new System.ArgumentNullException(nameof(commandTranslator));
        }
        IGetRepository<IApplicationUser> ApplicationUserRepository { get; }
        IAsyncQueryHandler<BagsCountByBagTypesQuery> BagTypesCountQuery { get; }
        IAsyncQueryHandler<BagsCountByBrandsQuery> BrandsCountQuery { get; }
        IAsyncQueryHandler<BagsCountByInsertDateQuery> InsertDateCountQuery { get; }
        IAsyncQueryHandler<TotalBagsCountByInsertDateQuery> TotalInsertDateCountQuery { get; }
        ITranslator<IQueryResult, IActionResult> QueryTranslator { get; }
        ITranslator<ICommandResult, IActionResult> CommandTranslator { get; }

        [HttpGet("BagTypes/{top:int?}")]
        public async Task<IActionResult> BagTypes(int top = 10) {
            var applicationUser = await ApplicationUserRepository.GetItemAsync();
            var result = await BagTypesCountQuery.ExecuteAsync(new BagsCountByBagTypesQuery());
            return QueryTranslator.Translate(result);
        }

        [HttpGet("Brands/{top:int?}")]
        public async Task<IActionResult> Brands(int top = 10) {
            var applicationUser = await ApplicationUserRepository.GetItemAsync();
            var result = await BrandsCountQuery.ExecuteAsync(new BagsCountByBrandsQuery(top));
            return QueryTranslator.Translate(result);
        }

        [HttpGet("Periods")]
        public async Task<IActionResult> Periods() {
            var applicationUser = await ApplicationUserRepository.GetItemAsync();
            var result = await InsertDateCountQuery.ExecuteAsync(new BagsCountByInsertDateQuery());
            return QueryTranslator.Translate(result);
        }

        [HttpGet("TotalCountPeriod")]
        public async Task<IActionResult> TotalCountPeriod() {
            var applicationUser = await ApplicationUserRepository.GetItemAsync();
            var result = await TotalInsertDateCountQuery.ExecuteAsync(new TotalBagsCountByInsertDateQuery());
            return QueryTranslator.Translate(result);
        }
    }
}
