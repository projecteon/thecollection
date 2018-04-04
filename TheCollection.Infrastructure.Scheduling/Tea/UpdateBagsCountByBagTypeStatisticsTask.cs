namespace TheCollection.Infrastructure.Scheduling.Tea {
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using NodaTime;
    using TheCollection.Application.Services.Commands.Tea;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class UpdateBagsCountByBagTypeStatisticsTask : IScheduledTask {

        public UpdateBagsCountByBagTypeStatisticsTask(
            IGetAllRepository<IApplicationUser> applicationUserRepository,
            IAsyncCommandHandler<CreateBagsCountByBagTypesCommand> createCountByBagTypesCommand,
            ILogger<UpdateTotalCountByInsertDateStatisticsTask> logger) {
            ApplicationUserRepository = applicationUserRepository ?? throw new System.ArgumentNullException(nameof(applicationUserRepository));
            CreateCountByBagTypesCommand = createCountByBagTypesCommand ?? throw new System.ArgumentNullException(nameof(createCountByBagTypesCommand));
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        private IGetAllRepository<IApplicationUser> ApplicationUserRepository { get; }
        private IAsyncCommandHandler<CreateBagsCountByBagTypesCommand> CreateCountByBagTypesCommand { get; }
        private ILogger<UpdateTotalCountByInsertDateStatisticsTask> Logger { get; }

        public Period Schedule => Period.FromMinutes(20);
        public bool RunOnStartup => false;

        public async Task ExecuteAsync(CancellationToken cancellationToken) {
            var applicationUsers = await ApplicationUserRepository.GetAllAsync();
            foreach (var applicationUser in applicationUsers.Where(x => x.Email == "l.wolterink@hotmail.com")) {
                await CreateCountByBagTypesCommand.ExecuteAsync(new CreateBagsCountByBagTypesCommand(applicationUser));
            }
        }
    }
}
