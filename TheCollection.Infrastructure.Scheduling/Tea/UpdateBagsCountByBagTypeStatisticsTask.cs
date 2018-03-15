namespace TheCollection.Infrastructure.Scheduling.Tea {

    using System.Threading;
    using System.Threading.Tasks;
    using NodaTime;
    using TheCollection.Application.Services.Commands.Tea;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class UpdateBagsCountByBagTypeStatisticsTask : IScheduledTask {

        public UpdateBagsCountByBagTypeStatisticsTask(
            IGetRepository<IApplicationUser> applicationUserRepository,
            IAsyncCommandHandler<CreateBagsCountByBagTypesCommand> createCountByBagTypesCommand,
            ILogger<UpdateTotalCountByInsertDateStatisticsTask> logger) {
            ApplicationUserRepository = applicationUserRepository ?? throw new System.ArgumentNullException(nameof(applicationUserRepository));
            CreateCountByBagTypesCommand = createCountByBagTypesCommand ?? throw new System.ArgumentNullException(nameof(createCountByBagTypesCommand));
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        private IGetRepository<IApplicationUser> ApplicationUserRepository { get; }
        private IAsyncCommandHandler<CreateBagsCountByBagTypesCommand> CreateCountByBagTypesCommand { get; }
        private ILogger<UpdateTotalCountByInsertDateStatisticsTask> Logger { get; }

        public Period Schedule => Period.FromMinutes(20);
        public bool RunOnStartup => false;

        public async Task ExecuteAsync(CancellationToken cancellationToken) {
            var applicationUser = await ApplicationUserRepository.GetItemAsync();
            var result = await CreateCountByBagTypesCommand.ExecuteAsync(new CreateBagsCountByBagTypesCommand(applicationUser));
        }
    }
}
