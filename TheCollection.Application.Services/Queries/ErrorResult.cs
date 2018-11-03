namespace TheCollection.Application.Services.Queries {
    using TheCollection.Domain.Core.Contracts;

    public class ErrorResult : IQueryResult {
        public ErrorResult(string message) {
            Message = message;
        }

        public string Message { get; }
    }
}
