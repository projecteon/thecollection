namespace TheCollection.Infrastructure.Scheduling {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TheCollection.Domain.Core.Contracts;

    public class SchedulerHostedService : HostedService {

        public event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException;

        private readonly IList<IScheduler> _scheduledTasks = new List<IScheduler>();

        public SchedulerHostedService(IEnumerable<IScheduler> scheduledTasks) {
            _scheduledTasks = scheduledTasks.ToList();
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
            while (!cancellationToken.IsCancellationRequested) {
                await ExecuteOnceAsync(cancellationToken);

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }

        private async Task ExecuteOnceAsync(CancellationToken cancellationToken) {
            var taskFactory = new TaskFactory(TaskScheduler.Current);
            foreach (var taskThatShouldRun in _scheduledTasks) {
                await taskFactory.StartNew(
                    async () => {
                        try {
                            await taskThatShouldRun.ExecuteAsync(cancellationToken);
                        }
                        catch (Exception ex) {
                            var args = new UnobservedTaskExceptionEventArgs(
                                ex as AggregateException ?? new AggregateException(ex));

                            UnobservedTaskException?.Invoke(this, args);

                            if (!args.Observed) {
                                throw;
                            }
                        }
                    },
                    cancellationToken);
            }
        }
    }
}
