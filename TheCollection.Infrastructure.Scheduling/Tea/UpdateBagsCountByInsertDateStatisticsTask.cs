namespace TheCollection.Infrastructure.Scheduling.Tea {
    using System.Threading;
    using System.Threading.Tasks;
    using NodaTime;
    using TheCollection.Application.Services.Commands.Tea;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class UpdateBagsCountByInsertDateStatisticsTask : IScheduledTask {

        public UpdateBagsCountByInsertDateStatisticsTask(
            IGetRepository<IApplicationUser> applicationUserRepository,
            IAsyncCommandHandler<CreateBagsCountByInsertDateCommand> createCountByInsertDateCommand,
            ILogger<UpdateTotalCountByInsertDateStatisticsTask> logger) {
            ApplicationUserRepository = applicationUserRepository ?? throw new System.ArgumentNullException(nameof(applicationUserRepository));
            CreateCountByInsertDateCommand = createCountByInsertDateCommand ?? throw new System.ArgumentNullException(nameof(createCountByInsertDateCommand));
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        IGetRepository<IApplicationUser> ApplicationUserRepository { get; }
        IAsyncCommandHandler<CreateBagsCountByInsertDateCommand> CreateCountByInsertDateCommand { get; }
        ILogger<UpdateTotalCountByInsertDateStatisticsTask> Logger { get; }

        public Period Schedule => Period.FromMinutes(20);
        public bool RunOnStartup => false;

        public async Task ExecuteAsync(CancellationToken cancellationToken) {
            var applicationUser = await ApplicationUserRepository.GetItemAsync();
            var result = await CreateCountByInsertDateCommand.ExecuteAsync(new CreateBagsCountByInsertDateCommand(applicationUser));
        }
    }
}
