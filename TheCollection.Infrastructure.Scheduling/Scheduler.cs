namespace TheCollection.Infrastructure.Scheduling {

    using System.Threading;
    using System.Threading.Tasks;
    using NodaTime;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Extensions;

    public class Scheduler : IScheduler {

        public Scheduler(IScheduledTask scheduledTask, IClock clock) {
            ScheduledTask = scheduledTask;
            Clock = clock;
            if (scheduledTask.RunOnStartup) {
                NextRunTime = Clock.NowLocalDateTime();
            }
            else {
                NextRunTime = Clock.NowLocalDateTime().Plus(ScheduledTask.Schedule);
            }
        }

        private LocalDateTime LastRunTime { get; set; }
        private LocalDateTime NextRunTime { get; set; }
        private IClock Clock { get; }
        private IScheduledTask ScheduledTask { get; }

        private void SetNextRunTime() {
            LastRunTime = NextRunTime;
            NextRunTime = NextRunTime.Plus(ScheduledTask.Schedule);
        }

        private bool ShouldRun {
            get {
                var currentTime = Clock.NowLocalDateTime();
                return NextRunTime < currentTime && LastRunTime != NextRunTime;
            }
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken) {
            if (ShouldRun == false) {
                return;
            }

            await ScheduledTask.ExecuteAsync(cancellationToken);

            SetNextRunTime();
        }
    }
}
