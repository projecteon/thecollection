namespace TheCollection.Api.Controllers.Tea {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheCollection.Application.Services.Commands;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Application.Services.Queries;
    using TheCollection.Application.Services.ViewModels;
    using TheCollection.Application.Services.ViewModels.Tea;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    [Produces("application/json")]
    [Route("api/Tea/[controller]")]
    public class BrandsController : Controller {
        IGetRepository<IApplicationUser> ApplicationUserRepository { get; }

        public BrandsController(SearchQueryHandler<Brand, Domain.Tea.Brand> searchBrandsCommand,
                GetQueryHandler<Brand, Domain.Tea.Brand> getBrand,
                IAsyncCommandHandler<UpdateCommand<Brand>, Domain.Tea.Brand> updateCommand,
                IAsyncCommandHandler<CreateCommand<Brand>, Domain.Tea.Brand> createCommand,
                IAsyncQueryHandler<SearchRefValuesQuery<Domain.Tea.Brand>> searchRefValuesQuery,
                ITranslator<IQueryResult, IActionResult> queryTranslator,
                ITranslator<ICommandResult, IActionResult> commandTranslator) {
            SearchBrandsCommand = searchBrandsCommand ?? throw new System.ArgumentNullException(nameof(searchBrandsCommand));
            GetBrand = getBrand ?? throw new System.ArgumentNullException(nameof(getBrand));
            UpdateCommand = updateCommand ?? throw new System.ArgumentNullException(nameof(updateCommand));
            CreateCommand = createCommand ?? throw new System.ArgumentNullException(nameof(createCommand));
            SearchRefValuesQuery = searchRefValuesQuery ?? throw new System.ArgumentNullException(nameof(searchRefValuesQuery));
            QueryTranslator = queryTranslator ?? throw new System.ArgumentNullException(nameof(queryTranslator));
            CommandTranslator = commandTranslator ?? throw new System.ArgumentNullException(nameof(commandTranslator));
        }

        SearchQueryHandler<Brand, Domain.Tea.Brand> SearchBrandsCommand { get; }
        GetQueryHandler<Brand, Domain.Tea.Brand> GetBrand { get; }
        IAsyncCommandHandler<UpdateCommand<Brand>, Domain.Tea.Brand> UpdateCommand { get; }
        IAsyncCommandHandler<CreateCommand<Brand>, Domain.Tea.Brand> CreateCommand { get; }
        IAsyncQueryHandler<SearchRefValuesQuery<Domain.Tea.Brand>> SearchRefValuesQuery { get; }
        ITranslator<IQueryResult, IActionResult> QueryTranslator { get; }
        ITranslator<ICommandResult, IActionResult> CommandTranslator { get; }

        [HttpGet()]
        [ProducesResponseType(typeof(SearchResult<Brand>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Brands([FromQuery] string searchterm = "") {
            var result = await SearchBrandsCommand.ExecuteAsync(new SearchQuery(searchterm, 1000));
            return QueryTranslator.Translate(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] Brand brand) {
            var result = await CreateCommand.ExecuteAsync(new CreateCommand<Brand>(brand));
            return CommandTranslator.Translate(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Brand brand) {
            var result = await UpdateCommand.ExecuteAsync(new UpdateCommand<Brand>(id, brand));
            return CommandTranslator.Translate(result);
        }

        [HttpGet, Route("refvalues/{searchterm:alpha}")]
        [ProducesResponseType(typeof(IEnumerable<RefValue>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefValues([FromQuery] string searchterm = "") {
            var result = await SearchRefValuesQuery.ExecuteAsync(new SearchRefValuesQuery<Domain.Tea.Brand>(searchterm));
            return QueryTranslator.Translate(result);
        }
    }
}
