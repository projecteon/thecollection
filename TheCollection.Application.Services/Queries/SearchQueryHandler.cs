namespace TheCollection.Application.Services.Queries {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Application.Services.ViewModels;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Extensions;

    public class SearchQueryHandler<TViewModel, TEntity> : IAsyncQueryHandler<SearchQuery> where TEntity : class, IEntity where TViewModel : class {
        public SearchQueryHandler(ISearchRepository<TEntity> searchRepository,
                            IAsyncTranslator<TEntity, TViewModel> translator,
                            ILinqSearchRepository<IActivity> activityRepository,
                            IActivityAuthorizer authorizer) {
            SearchRepository = searchRepository ?? throw new System.ArgumentNullException(nameof(searchRepository));
            Translator = translator ?? throw new System.ArgumentNullException(nameof(translator));
            ActivityRepository = activityRepository ?? throw new System.ArgumentNullException(nameof(activityRepository));
            Authorizer = authorizer ?? throw new System.ArgumentNullException(nameof(authorizer));
        }

        ISearchRepository<TEntity> SearchRepository { get; }
        IAsyncTranslator<TEntity, TViewModel> Translator { get; }
        ILinqSearchRepository<IActivity> ActivityRepository { get; }
        IActivityAuthorizer Authorizer { get; }

        public async Task<IQueryResult> ExecuteAsync(SearchQuery query) {
            var activity = await ActivityRepository.SearchItemsAsync(x => x.Name == $"{typeof(TEntity)}{nameof(SearchQueryHandler<TViewModel, TEntity>)}");
            if (await Authorizer.IsAuthorized(activity.FirstOrDefault())) {
                return new ForbidResult();
            }

            if (query.SearchTerm.IsNullOrWhiteSpace()) {
                return new ErrorResult("Search cannot be empty");
            }

            var entities = await SearchRepository.SearchAsync(query.SearchTerm, query.PageSize);
            var result = new List<TViewModel>();
            foreach (var entity in entities) {
                var viewModel = await Translator.Translate(entity);
                result.Add(viewModel);
            }

            var count = await SearchRepository.SearchRowCountAsync(query.SearchTerm);

            return new OkResult(new SearchResult<TViewModel>(result, count));
        }
    }
}
