namespace TheCollection.Application.Services.Commands {
    using TheCollection.Domain.Core.Contracts;

    public class CreateResult : ICommandResult {
        public CreateResult(string id) {
            if (string.IsNullOrWhiteSpace(id)) {
                throw new System.ArgumentException("message", nameof(id));
            }

            Id = id;
        }

        public string Id { get; }
    }
}
