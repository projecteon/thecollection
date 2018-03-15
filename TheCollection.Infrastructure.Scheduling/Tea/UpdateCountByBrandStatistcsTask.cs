namespace TheCollection.Infrastructure.Scheduling.Tea {
    using System.Threading;
    using System.Threading.Tasks;
    using NodaTime;
    using TheCollection.Application.Services.Commands.Tea;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class UpdateCountByBrandStatistcsTask : IScheduledTask {

        public UpdateCountByBrandStatistcsTask(
            IGetRepository<IApplicationUser> applicationUserRepository,
            IAsyncCommandHandler<CreateBagsCountByBrandsCommand> createCountByBrandsCommand,
            ILogger<UpdateTotalCountByInsertDateStatisticsTask> logger) {
            ApplicationUserRepository = applicationUserRepository ?? throw new System.ArgumentNullException(nameof(applicationUserRepository));
            CreateCountByBrandsCommand = createCountByBrandsCommand ?? throw new System.ArgumentNullException(nameof(createCountByBrandsCommand));
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        IGetRepository<IApplicationUser> ApplicationUserRepository { get; }
        IAsyncCommandHandler<CreateBagsCountByBrandsCommand> CreateCountByBrandsCommand { get; }
        ILogger<UpdateTotalCountByInsertDateStatisticsTask> Logger { get; }

        public Period Schedule => Period.FromMinutes(20);
        public bool RunOnStartup => false;

        public async Task ExecuteAsync(CancellationToken cancellationToken) {
            var applicationUser = await ApplicationUserRepository.GetItemAsync();
            var result = await CreateCountByBrandsCommand.ExecuteAsync(new CreateBagsCountByBrandsCommand(applicationUser));
        }
    }
}
