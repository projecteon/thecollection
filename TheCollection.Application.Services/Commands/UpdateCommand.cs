namespace TheCollection.Application.Services.Commands {
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class UpdateCommand<TEntity> : IAsyncCommand<TEntity> where TEntity : class, IEntity, new() {
        public UpdateCommand(IUpdateRepository<TEntity> updateRepository,
                             ILinqSearchRepository<IActivity> activityRepository,
                             IActivityAuthorizer authorizer) {
            UpdateRepository = updateRepository;
            ActivityRepository = activityRepository;
            Authorizer = authorizer;
        }

        IUpdateRepository<TEntity> UpdateRepository { get; }
        ILinqSearchRepository<IActivity> ActivityRepository { get; }
        IActivityAuthorizer Authorizer { get; }

        public async Task<IActivityResult> ExecuteAsync(TEntity entity) {
            var activity = await ActivityRepository.SearchItemsAsync(x => x.Name == $"{typeof(TEntity)}{nameof(UpdateCommand<TEntity>)}");
            if (Authorizer.IsAuthorized(activity.FirstOrDefault())) {
                return new ForbidResult();
            }

            if (entity == null) {
                return new ErrorResult("Update item cannot be null");
            }

            entity.Id = await UpdateRepository.UpdateItemAsync(entity.Id, entity);
            return new OkObjectResult<TEntity>(entity);
        }
    }
}
