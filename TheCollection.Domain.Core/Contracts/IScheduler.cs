namespace TheCollection.Domain.Core.Contracts {
    using System.Threading;
    using System.Threading.Tasks;

    public interface IScheduler {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
