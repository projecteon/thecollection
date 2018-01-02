namespace TheCollection.Application.Services.Commands {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Application.Services.Contracts.Repository;
    using TheCollection.Application.Services.Models;
    using TheCollection.Domain.Contracts;
    using TheCollection.Domain.Extensions;

    public class SearchOwnedCommand<TEntity> : IAsyncCommand<ISearch> where TEntity : class, IOwnedEntity, new() {
        public SearchOwnedCommand(ISearchRepository<TEntity> searchRepository,
                                  IActivity activity,
                                  IActivityAuthorizer authorizer,
                                  Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> resultSorter = null) {
            SearchRepository = searchRepository;
            Activity = activity;
            Authorizer = authorizer;
            ResultSorter = resultSorter;
        }

        ISearchRepository<TEntity> SearchRepository { get; }
        IActivity Activity { get; }
        IActivityAuthorizer Authorizer { get; }
        Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> ResultSorter { get; }

        public async Task<IActivityResult> ExecuteAsync(ISearch search) {
            //var activity = await ActivityRepository.GetAll(x => x.Name == $"{typeof(TEntity)}{nameof(SearchOwnedCommand<TEntity>)}");
            if (Authorizer.IsAuthorized(Activity)) {
                return new ForbidResult();
            }

            if (search.Searchterm.IsNullOrWhiteSpace()) {
                return new ErrorResult("Search cannot be empty");
            }

            var entities = await SearchRepository.SearchAsync(search.Searchterm, search.Pagesize);
            if(ResultSorter != null) {
                entities = ResultSorter(entities);
            }

            var result = new SearchResult<TEntity> {
                Count = await SearchRepository.SearchRowCountAsync(search.Searchterm),
                Data = entities
            };

            return new OkObjectResult<SearchResult<TEntity>>(result);
        }
    }
}
