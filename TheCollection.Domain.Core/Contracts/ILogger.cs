namespace TheCollection.Domain.Core.Contracts {
    using System.Threading.Tasks;

    public interface ILogger<T> {
        Task LogInformationAsync(string message);
        Task LogErrorAsync(string message);
    }
}
