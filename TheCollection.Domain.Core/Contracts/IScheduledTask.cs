namespace TheCollection.Domain.Core.Contracts {
    using System.Threading;
    using System.Threading.Tasks;
    using NodaTime;

    public interface IScheduledTask {
        Period Schedule { get; }
        bool RunOnStartup { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
