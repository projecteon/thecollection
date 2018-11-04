namespace TheCollection.Application.Services.Commands {
    using TheCollection.Domain.Core.Contracts;

    public class ErrorResult : ICommandResult {
        public ErrorResult(string message) {
            Message = message;
        }

        public string Message { get; }
    }
}
