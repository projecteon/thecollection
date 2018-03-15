namespace TheCollection.Application.Services.Queries {
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Extensions;

    public class SearchRefValuesQueryHandler<T> : IAsyncQueryHandler<SearchRefValuesQuery<T>> where T : class, IRef {
        public SearchRefValuesQueryHandler(ISearchRepository<T> repository, IGetRepository<IApplicationUser> applicationUserRepository) {
            Repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            ApplicationUserRepository = applicationUserRepository ?? throw new System.ArgumentNullException(nameof(applicationUserRepository));
        }

        ISearchRepository<T> Repository { get; }
        IGetRepository<IApplicationUser> ApplicationUserRepository { get; }

        public async Task<IQueryResult> ExecuteAsync(SearchRefValuesQuery<T> query) {
            if (query.SearchTerm.IsNullOrWhiteSpace()) {
                return new ErrorResult($"{nameof(query.SearchTerm)} cannot be null or whitespace");
            }

            var refValues = await Repository.SearchAsync(query.SearchTerm);
            if (refValues == null) {
                return new NotFoundResult();
            }

            return new OkResult(refValues.OrderBy(x => x.Name));
        }
    }
}
