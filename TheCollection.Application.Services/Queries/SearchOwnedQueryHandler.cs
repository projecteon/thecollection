namespace TheCollection.Application.Services.Queries {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Application.Services.ViewModels;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Extensions;

    public class SearchOwnedQueryHandler<TViewModel, TEntity> : IAsyncQueryHandler<SearchOwnedQuery> where TEntity : class, IOwnedEntity, new() where TViewModel : class {
        public SearchOwnedQueryHandler(ISearchRepository<TEntity> searchRepository,
                                  ITranslator<TEntity, TViewModel> translator,
                                  IActivity activity,
                                  IActivityAuthorizer authorizer,
                                  Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> resultSorter = null) {
            SearchRepository = searchRepository ?? throw new ArgumentNullException(nameof(searchRepository));
            Translator = translator ?? throw new ArgumentNullException(nameof(translator));
            Activity = activity ?? throw new ArgumentNullException(nameof(activity));
            Authorizer = authorizer ?? throw new ArgumentNullException(nameof(authorizer));
            ResultSorter = resultSorter;
        }

        ISearchRepository<TEntity> SearchRepository { get; }
        ITranslator<TEntity, TViewModel> Translator { get; }
        IActivity Activity { get; }
        IActivityAuthorizer Authorizer { get; }
        Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> ResultSorter { get; }

        public async Task<IQueryResult> ExecuteAsync(SearchOwnedQuery query) {
            //var activity = await ActivityRepository.GetAll(x => x.Name == $"{typeof(TEntity)}{nameof(SearchOwnedCommand<TEntity>)}");
            if (await Authorizer.IsAuthorized(Activity)) {
                return new ForbidResult();
            }

            if (query.SearchTerm.IsNullOrWhiteSpace()) {
                return new ErrorResult("Search cannot be empty");
            }

            var entities = await SearchRepository.SearchAsync(query.SearchTerm, query.PageSize);
            if (ResultSorter != null) {
                entities = ResultSorter(entities);
            }

            var count = await SearchRepository.SearchRowCountAsync(query.SearchTerm);
            var result = new SearchResult<TViewModel>(entities.Select(Translator.Translate), count);

            return new OkResult(result);
        }
    }
}
