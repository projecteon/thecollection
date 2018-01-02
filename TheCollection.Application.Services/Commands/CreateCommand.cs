namespace TheCollection.Application.Services.Commands {
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Application.Services.Contracts.Repository;
    using TheCollection.Domain.Contracts;

    public class CreateCommand<TEntity> : IAsyncCommand<TEntity> where TEntity : class, IEntity, new() {
        public CreateCommand(ICreateRepository<TEntity> createRepository,
                             ILinqSearchRepository<IActivity> activityRepository,
                             IActivityAuthorizer authorizer) {
            CreateRepository = createRepository;
            ActivityRepository = activityRepository;
            Authorizer = authorizer;
        }

        ICreateRepository<TEntity> CreateRepository { get; }
        ILinqSearchRepository<IActivity> ActivityRepository { get; }
        IActivityAuthorizer Authorizer { get; }

        public async Task<IActivityResult> ExecuteAsync(TEntity entity) {
            var activity = await ActivityRepository.SearchItemsAsync(x => x.Name == $"{typeof(TEntity)}{nameof(CreateCommand<TEntity>)}");
            if (Authorizer.IsAuthorized(activity.FirstOrDefault())) {
                return new ForbidResult();
            }

            if (entity == null) {
                return new ErrorResult("New item cannot be null");
            }

            entity.Id = await CreateRepository.CreateItemAsync(entity);
            return new OkObjectResult<TEntity>(entity);
        }
    }
}
