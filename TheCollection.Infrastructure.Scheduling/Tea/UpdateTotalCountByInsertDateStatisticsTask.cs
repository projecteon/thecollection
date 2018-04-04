namespace TheCollection.Infrastructure.Scheduling.Tea {
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using NodaTime;
    using TheCollection.Application.Services.Commands.Tea;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class UpdateTotalCountByInsertDateStatisticsTask : IScheduledTask {
        public UpdateTotalCountByInsertDateStatisticsTask(
            IGetAllRepository<IApplicationUser> applicationUserRepository,
            IAsyncCommandHandler<CreateTotalBagsCountByInsertDateCommand> createTotalCountByInsertDateCommand,
            ILogger<UpdateTotalCountByInsertDateStatisticsTask> logger) {
            ApplicationUserRepository = applicationUserRepository ?? throw new System.ArgumentNullException(nameof(applicationUserRepository));
            CreateTotalCountByInsertDateCommand = createTotalCountByInsertDateCommand ?? throw new System.ArgumentNullException(nameof(createTotalCountByInsertDateCommand));
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        IGetAllRepository<IApplicationUser> ApplicationUserRepository { get; }
        IAsyncCommandHandler<CreateTotalBagsCountByInsertDateCommand> CreateTotalCountByInsertDateCommand { get; }
        ILogger<UpdateTotalCountByInsertDateStatisticsTask> Logger { get; }

        public Period Schedule => Period.FromMinutes(20);
        public bool RunOnStartup => true;

        public async Task ExecuteAsync(CancellationToken cancellationToken) {
            var applicationUsers = await ApplicationUserRepository.GetAllAsync();
            foreach (var applicationUser in applicationUsers.Where(x => x.Email == "l.wolterink@hotmail.com")) {
                await CreateTotalCountByInsertDateCommand.ExecuteAsync(new CreateTotalBagsCountByInsertDateCommand(applicationUser));
            }
        }
    }
}
