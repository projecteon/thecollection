namespace TheCollection.Application.Services.Commands {
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class CreateOwnedCommand<TEntity> : IAsyncCommand<TEntity> where TEntity : class, IOwnedEntity, new() {
        public CreateOwnedCommand(ICreateRepository<TEntity> createRepository,
                             ILinqSearchRepository<IActivity> activityRepository,
                             IActivityAuthorizer authorizer,
                             IApplicationUser applicationUser) {
            CreateRepository = createRepository;
            ActivityRepository = activityRepository;
            Authorizer = authorizer;
            ApplicationUser = applicationUser;
        }

        ICreateRepository<TEntity> CreateRepository { get; }
        ILinqSearchRepository<IActivity> ActivityRepository { get; }
        IActivityAuthorizer Authorizer { get; }
        public IApplicationUser ApplicationUser { get; }

        public async Task<IActivityResult> ExecuteAsync(TEntity entity) {
            var activity = await ActivityRepository.SearchItemsAsync(x => x.Name == $"{typeof(TEntity)}{nameof(CreateOwnedCommand<TEntity>)}");
            if (Authorizer.IsAuthorized(activity.FirstOrDefault())) {
                return new ForbidResult();
            }

            if (entity == null) {
                return new ErrorResult("New item cannot be null");
            }

            entity.OwnerId = ApplicationUser.Id;
            entity.Id = await CreateRepository.CreateItemAsync(entity);
            return new OkObjectResult<TEntity>(entity);
        }
    }
}
