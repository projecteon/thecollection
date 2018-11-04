namespace TheCollection.Api.Controllers.Tea {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheCollection.Application.Services.Commands;
    using TheCollection.Application.Services.Queries;
    using TheCollection.Application.Services.ViewModels;
    using TheCollection.Application.Services.ViewModels.Tea;
    using TheCollection.Domain.Core.Contracts;

    [Produces("application/json")]
    [Route("api/Tea/[controller]")]
    public class BagTypesController : Controller {
        public BagTypesController(SearchQueryHandler<BagType, Domain.Tea.BagType> searchBagTypesCommand,
                GetQueryHandler<BagType, Domain.Tea.BagType> getBagType,
                IAsyncCommandHandler<UpdateCommand<BagType>, Domain.Tea.BagType> updateCommand,
                IAsyncCommandHandler<CreateCommand<BagType>, Domain.Tea.BagType> createCommand,
                IAsyncQueryHandler<SearchRefValuesQuery<Domain.Tea.BagType>> searchRefValuesQuery,
                ITranslator<IQueryResult, IActionResult> queryTranslator,
                ITranslator<ICommandResult, IActionResult> commandTranslator) {
            SearchBagTypesCommand = searchBagTypesCommand ?? throw new System.ArgumentNullException(nameof(searchBagTypesCommand));
            GetBagType = getBagType ?? throw new System.ArgumentNullException(nameof(getBagType));
            UpdateCommand = updateCommand ?? throw new System.ArgumentNullException(nameof(updateCommand));
            CreateCommand = createCommand ?? throw new System.ArgumentNullException(nameof(createCommand));
            SearchRefQuery = searchRefValuesQuery ?? throw new System.ArgumentNullException(nameof(searchRefValuesQuery));
            QueryTranslator = queryTranslator ?? throw new System.ArgumentNullException(nameof(queryTranslator));
            CommandTranslator = commandTranslator ?? throw new System.ArgumentNullException(nameof(commandTranslator));
        }

        SearchQueryHandler<BagType, Domain.Tea.BagType> SearchBagTypesCommand { get; }
        GetQueryHandler<BagType, Domain.Tea.BagType> GetBagType { get; }
        IAsyncCommandHandler<UpdateCommand<BagType>, Domain.Tea.BagType> UpdateCommand { get; }
        IAsyncCommandHandler<CreateCommand<BagType>, Domain.Tea.BagType> CreateCommand { get; }
        IAsyncQueryHandler<SearchRefValuesQuery<Domain.Tea.BagType>> SearchRefQuery { get; }
        ITranslator<IQueryResult, IActionResult> QueryTranslator { get; }
        ITranslator<ICommandResult, IActionResult> CommandTranslator { get; }

        [HttpGet()]
        [ProducesResponseType(typeof(SearchResult<BagType>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BagTypes([FromQuery] string searchterm = "") {
            var result = await SearchBagTypesCommand.ExecuteAsync(new SearchQuery(searchterm, 1000));
            return QueryTranslator.Translate(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] BagType bagType) {
            var result = await CreateCommand.ExecuteAsync(new CreateCommand<BagType>(bagType));
            return CommandTranslator.Translate(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] BagType bagType) {
            var result = await UpdateCommand.ExecuteAsync(new UpdateCommand<BagType>(id, bagType));
            return CommandTranslator.Translate(result);
        }

        [HttpGet, Route("refvalues/{searchterm:alpha}")]
        [ProducesResponseType(typeof(IEnumerable<RefValue>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefValues([FromQuery] string searchterm = "") {
            var result = await SearchRefQuery.ExecuteAsync(new SearchRefValuesQuery<Domain.Tea.BagType>(searchterm));
            return QueryTranslator.Translate(result);
        }
    }
}
