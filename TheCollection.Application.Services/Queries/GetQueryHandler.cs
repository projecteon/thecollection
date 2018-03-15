namespace TheCollection.Application.Services.Queries {
    using System.Threading.Tasks;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class GetQueryHandler<TViewModel, TEntity> : IAsyncQueryHandler<GetQuery> where TEntity : class, IEntity where TViewModel : class {
        public GetQueryHandler(IGetRepository<TEntity> repository, IAsyncTranslator<TEntity, TViewModel> translator) {
            Repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            Translator = translator ?? throw new System.ArgumentNullException(nameof(translator));
        }

        IGetRepository<TEntity> Repository { get; }
        IAsyncTranslator<TEntity, TViewModel> Translator { get; }

        public async Task<IQueryResult> ExecuteAsync(GetQuery query) {
            var data = await Repository.GetItemAsync(query.Id);
            if (data == null) {
                return new NotFoundResult();
            }

            var viewModel = await Translator.Translate(data);
            return new OkResult(viewModel);
        }
    }
}
