namespace TheCollection.Application.Services.Commands {
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class UpdateCommandHandler<TViewModel, TEntity> : IAsyncCommandHandler<UpdateCommand<TViewModel>, TEntity>
               where TEntity : class, IEntity {
            public UpdateCommandHandler(IUpdateRepository<TEntity> updateRepository,
                            IGetRepository<TEntity> getRepository,
                            ILinqSearchRepository<IActivity> activityRepository,
                            IActivityAuthorizer authorizer,
                            ITranslator<TViewModel, TEntity> translator) {
            UpdateRepository = updateRepository ?? throw new System.ArgumentNullException(nameof(updateRepository));
            GetRepository = getRepository ?? throw new System.ArgumentNullException(nameof(getRepository));
            ActivityRepository = activityRepository ?? throw new System.ArgumentNullException(nameof(activityRepository));
            Authorizer = authorizer ?? throw new System.ArgumentNullException(nameof(authorizer));
            Translator = translator ?? throw new System.ArgumentNullException(nameof(translator));
        }

        IUpdateRepository<TEntity> UpdateRepository { get; }
        IGetRepository<TEntity> GetRepository { get; }
        ILinqSearchRepository<IActivity> ActivityRepository { get; }
        IActivityAuthorizer Authorizer { get; }
        ITranslator<TViewModel, TEntity> Translator { get; }

        public async Task<ICommandResult> ExecuteAsync(UpdateCommand<TViewModel> command) {
            var activity = await ActivityRepository.SearchItemsAsync(x => x.Name == $"{typeof(TViewModel)}{nameof(UpdateCommandHandler<UpdateCommand<TViewModel>, TEntity>)}");
            if (await Authorizer.IsAuthorized(activity.FirstOrDefault())) {
                return new ForbidResult();
            }

            if (command.Data == null) {
                return new ErrorResult("Update item cannot be null");
            }

            var entity = Translator.Translate(command.Data);
            await UpdateRepository.UpdateItemAsync(command.Id, entity);
            return new OkResult();
        }
    }
}
