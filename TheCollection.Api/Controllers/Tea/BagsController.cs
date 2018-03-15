namespace TheCollection.Api.Controllers.Tea {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheCollection.Application.Services.Commands.Tea;
    using TheCollection.Application.Services.Queries;
    using TheCollection.Application.Services.ViewModels;
    using TheCollection.Application.Services.ViewModels.Tea;
    using TheCollection.Domain.Core.Contracts;

    [Produces("application/json")]
    [Route("api/Tea/[controller]")]
    public class BagsController : Controller {
        public BagsController(SearchQueryHandler<Bag, Domain.Tea.Bag> searchBagsCommand,
                GetQueryHandler<Bag, Domain.Tea.Bag> getBagType,
                IAsyncCommandHandler<UpdateBagCommand> updateCommand,
                IAsyncCommandHandler<CreateBagCommand> createCommand,
                ITranslator<IQueryResult, IActionResult> queryTranslator,
                ITranslator<ICommandResult, IActionResult> commandTranslator) {
            SearchBagsCommand = searchBagsCommand ?? throw new System.ArgumentNullException(nameof(searchBagsCommand));
            GetBagType = getBagType ?? throw new System.ArgumentNullException(nameof(getBagType));
            UpdateCommand = updateCommand ?? throw new System.ArgumentNullException(nameof(updateCommand));
            CreateCommand = createCommand ?? throw new System.ArgumentNullException(nameof(createCommand));
            QueryTranslator = queryTranslator ?? throw new System.ArgumentNullException(nameof(queryTranslator));
            CommandTranslator = commandTranslator ?? throw new System.ArgumentNullException(nameof(commandTranslator));
        }

        SearchQueryHandler<Bag, Domain.Tea.Bag> SearchBagsCommand { get; }
        GetQueryHandler<Bag, Domain.Tea.Bag> GetBagType { get; }
        IAsyncCommandHandler<UpdateBagCommand> UpdateCommand { get; }
        IAsyncCommandHandler<CreateBagCommand> CreateCommand { get; }
        ITranslator<IQueryResult, IActionResult> QueryTranslator { get; }
        ITranslator<ICommandResult, IActionResult> CommandTranslator { get; }

        [HttpGet()]
        [ProducesResponseType(typeof(SearchResult<Bag>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Bags([FromQuery] string searchterm = "", [FromQuery] int pagesize = 300, [FromQuery] int page = 0) {
            var result = await SearchBagsCommand.ExecuteAsync(new SearchQuery(searchterm, pagesize));
            return QueryTranslator.Translate(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Brand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Bag(string id) {
            var result = await GetBagType.ExecuteAsync(new GetQuery(id));
            return QueryTranslator.Translate(result);
        }

        [HttpPost()]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] Bag bag) {
            var result = await CreateCommand.ExecuteAsync(new CreateBagCommand(bag));
            return CommandTranslator.Translate(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] Bag bag) {
            var result = await UpdateCommand.ExecuteAsync(new UpdateBagCommand(id, bag));
            return CommandTranslator.Translate(result);
        }
    }
}
