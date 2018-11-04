namespace TheCollection.Infrastructure.Scheduling.Tea {
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using NodaTime;
    using TheCollection.Application.Services.Commands.Tea;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class UpdateTotalCountByInsertDateStatisticsTask : IScheduledTask {
        public UpdateTotalCountByInsertDateStatisticsTask(
            ILogger<UpdateTotalCountByInsertDateStatisticsTask> logger,
            IServiceProvider services) {
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        ILogger<UpdateTotalCountByInsertDateStatisticsTask> Logger { get; }
        IServiceProvider Services { get; }

        public Period Schedule => Period.FromMinutes(20);
        public bool RunOnStartup => false;

        public async Task ExecuteAsync(CancellationToken cancellationToken) {
            using (var scope = Services.CreateScope()) {
                var applicationUserRepository = scope.ServiceProvider.GetRequiredService<IGetAllRepository<IApplicationUser>>();
                var createTotalCountByInsertDateCommand = scope.ServiceProvider.GetRequiredService<IAsyncCommandHandler<CreateTotalBagsCountByInsertDateCommand>>();

                var applicationUsers = await applicationUserRepository.GetAllAsync();
                foreach (var applicationUser in applicationUsers.Where(x => x.Email == "l.wolterink@hotmail.com")) {
                    await createTotalCountByInsertDateCommand.ExecuteAsync(new CreateTotalBagsCountByInsertDateCommand(applicationUser));
                }
            }
        }
    }
}
