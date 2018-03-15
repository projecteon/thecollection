namespace TheCollection.Infrastructure.Logging
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class ConsoleLogger<T> : TheCollection.Domain.Core.Contracts.ILogger<T> {
        public ConsoleLogger(Microsoft.Extensions.Logging.ILogger<T> logger) {
            Logger = logger;
        }

        public Microsoft.Extensions.Logging.ILogger<T> Logger { get; }

        public async Task LogErrorAsync(string message) {
            await Task.Run(() => Logger.LogError(message));
        }

        public async Task LogInformationAsync(string message) {
            await Task.Run(() => Logger.LogInformation(message));
        }
    }
}
