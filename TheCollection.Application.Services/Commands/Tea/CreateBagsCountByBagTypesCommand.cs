namespace TheCollection.Application.Services.Commands.Tea {
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;

    public class CreateBagsCountByBagTypesCommand : ICommand {
        public CreateBagsCountByBagTypesCommand(IApplicationUser user) {
            User = user ?? throw new System.ArgumentNullException(nameof(user));
        }

        public IApplicationUser User { get; }
    }
}
