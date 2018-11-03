namespace TheCollection.Application.Services.Commands {
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class CreateOwnedCommandHandler<TCommand, TEntity> : IAsyncCommandHandler<TCommand>
            where TCommand : ICommand
            where TEntity : class, IOwnedEntity, new() {
        public CreateOwnedCommandHandler(ICreateRepository<TEntity> createRepository,
                             ILinqSearchRepository<IActivity> activityRepository,
                             IActivityAuthorizer authorizer,
                             IApplicationUser applicationUser,
                             ITranslator<TCommand, TEntity> translator) {
            CreateRepository = createRepository ?? throw new System.ArgumentNullException(nameof(createRepository));
            ActivityRepository = activityRepository ?? throw new System.ArgumentNullException(nameof(activityRepository));
            Authorizer = authorizer ?? throw new System.ArgumentNullException(nameof(authorizer));
            ApplicationUser = applicationUser ?? throw new System.ArgumentNullException(nameof(applicationUser));
            Translator = translator ?? throw new System.ArgumentNullException(nameof(translator));
        }

        ICreateRepository<TEntity> CreateRepository { get; }
        ILinqSearchRepository<IActivity> ActivityRepository { get; }
        IActivityAuthorizer Authorizer { get; }
        IApplicationUser ApplicationUser { get; }
        ITranslator<TCommand, TEntity> Translator { get; }

        public async Task<ICommandResult> ExecuteAsync(TCommand command) {
            var activity = await ActivityRepository.SearchItemsAsync(x => x.Name == $"{typeof(TEntity)}{nameof(CreateOwnedCommandHandler<TCommand, TEntity>)}");
            if (await Authorizer.IsAuthorized(activity.FirstOrDefault())) {
                return new ForbidResult();
            }

            if (command == null) {
                return new ErrorResult("New item cannot be null");
            }

            var entity = Translator.Translate(command);
            entity.OwnerId = ApplicationUser.Id;
            var id = await CreateRepository.CreateItemAsync(entity);
            return new CreateResult(id);
        }
    }
}
