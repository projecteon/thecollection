namespace TheCollection.Application.Services.Commands {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Application.Services.Contracts.Repository;
    using TheCollection.Domain.Contracts;
    using TheCollection.Domain.Extensions;

    public class SearchCommand<TEntity> : IAsyncCommand<ISearch> where TEntity : class, IEntity, new() {
        public SearchCommand(ISearchRepository<TEntity> searchRepository,
                             ILinqSearchRepository<IActivity> activityRepository,
                             IActivityAuthorizer authorizer) {
            SearchRepository = searchRepository;
            ActivityRepository = activityRepository;
            Authorizer = authorizer;
        }

        ISearchRepository<TEntity> SearchRepository { get; }
        ILinqSearchRepository<IActivity> ActivityRepository { get; }
        IActivityAuthorizer Authorizer { get; }

        public async Task<IActivityResult> ExecuteAsync(ISearch search) {
            var activity = await ActivityRepository.SearchItemsAsync(x => x.Name == $"{typeof(TEntity)}{nameof(SearchCommand<TEntity>)}");
            if (Authorizer.IsAuthorized(activity.FirstOrDefault())) {
                return new ForbidResult();
            }

            if (search.Searchterm.IsNullOrWhiteSpace()) {
                return new ErrorResult("Search cannot be empty");
            }

            var entities = await SearchRepository.SearchAsync(search.Searchterm, search.Pagesize);
            return new OkObjectResult<IEnumerable<TEntity>>(entities);
        }
    }
}
