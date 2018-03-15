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
    public class CountriesController : Controller {
        public CountriesController(SearchQueryHandler<Country, Domain.Tea.Country> searchCountriesCommand,
                GetQueryHandler<Country, Domain.Tea.Country> getCountry,
                IAsyncCommandHandler<UpdateCommand<Country>, Domain.Tea.Country> updateCommand,
                IAsyncCommandHandler<CreateCommand<Country>, Domain.Tea.Country> createCommand,
                IAsyncQueryHandler<SearchRefValuesQuery<Domain.Tea.Country>> searchRefValuesQuery,
                ITranslator<IQueryResult, IActionResult> queryTranslator,
                ITranslator<ICommandResult, IActionResult> commandTranslator) {
            SearchCountriesCommand = searchCountriesCommand ?? throw new System.ArgumentNullException(nameof(searchCountriesCommand));
            GetCountry = getCountry ?? throw new System.ArgumentNullException(nameof(getCountry));
            UpdateCommand = updateCommand ?? throw new System.ArgumentNullException(nameof(updateCommand));
            CreateCommand = createCommand ?? throw new System.ArgumentNullException(nameof(createCommand));
            SearchRefValuesQuery = searchRefValuesQuery ?? throw new System.ArgumentNullException(nameof(searchRefValuesQuery));
            QueryTranslator = queryTranslator ?? throw new System.ArgumentNullException(nameof(queryTranslator));
            CommandTranslator = commandTranslator ?? throw new System.ArgumentNullException(nameof(commandTranslator));
        }

        SearchQueryHandler<Country, Domain.Tea.Country> SearchCountriesCommand { get; }
        GetQueryHandler<Country, Domain.Tea.Country> GetCountry { get; }
        IAsyncCommandHandler<UpdateCommand<Country>, Domain.Tea.Country> UpdateCommand { get; }
        IAsyncCommandHandler<CreateCommand<Country>, Domain.Tea.Country> CreateCommand { get; }
        IAsyncQueryHandler<SearchRefValuesQuery<Domain.Tea.Country>> SearchRefValuesQuery { get; }
        ITranslator<IQueryResult, IActionResult> QueryTranslator { get; }
        ITranslator<ICommandResult, IActionResult> CommandTranslator { get; }

        [HttpGet()]
        [ProducesResponseType(typeof(SearchResult<Country>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Countries([FromQuery] string searchterm = "") {
            var result = await SearchCountriesCommand.ExecuteAsync(new SearchQuery(searchterm, 1000));
            return QueryTranslator.Translate(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] Country country) {
            var result = await CreateCommand.ExecuteAsync(new CreateCommand<Country>(country));
            return CommandTranslator.Translate(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Country country) {
            var result = await UpdateCommand.ExecuteAsync(new UpdateCommand<Country>(id, country));
            return CommandTranslator.Translate(result);
        }

        [HttpGet, Route("refvalues/{searchterm:alpha}")]
        [ProducesResponseType(typeof(IEnumerable<RefValue>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefValues([FromQuery] string searchterm = "") {
            var result = await SearchRefValuesQuery.ExecuteAsync(new SearchRefValuesQuery<Domain.Tea.Country>(searchterm));
            return QueryTranslator.Translate(result);
        }
    }
}
