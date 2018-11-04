namespace TheCollection.Application.Services.Commands {
    using TheCollection.Domain.Core.Contracts;

    public class CreateCommand<TViewModel> : ICommand {
        public CreateCommand(TViewModel data) {
            Data = data;
        }

        public TViewModel Data { get; }
    }
}
