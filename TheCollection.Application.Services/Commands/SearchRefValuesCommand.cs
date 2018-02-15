namespace TheCollection.Application.Services.Commands {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Extensions;

    public class SearchRefValuesCommand<T> : IAsyncCommand<ISearch> where T : class, IRef {
        public SearchRefValuesCommand(ISearchRepository<T> repository, IApplicationUser applicationUser) {
            Repository = repository;
            ApplicationUser = applicationUser;
        }

        public ISearchRepository<T> Repository { get; }
        public IApplicationUser ApplicationUser { get; }

        public async Task<IActivityResult> ExecuteAsync(ISearch search) {
            if (search.Searchterm.IsNullOrWhiteSpace()) {
                return new ErrorResult();
            }

            var refValues = await Repository.SearchAsync(search.Searchterm);
            if (refValues == null) {
                return new NotFoundResult();
            }

            return new OkObjectResult<IEnumerable<T>>(refValues.OrderBy(x => x.Name));
        }
    }
}
