namespace TheCollection.Application.Services.Commands {
    using TheCollection.Domain.Core.Contracts;

    public class UpdateCommand<TViewModel> : ICommand {
        public UpdateCommand(string id, TViewModel data) {
            Id = id;
            Data = data;
        }

        public string Id { get; }
        public TViewModel Data { get; }
    }
}
