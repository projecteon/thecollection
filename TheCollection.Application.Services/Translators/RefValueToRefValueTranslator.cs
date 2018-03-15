namespace TheCollection.Application.Services.Translators {
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Constants;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class RefValueToRefValueTranslator : IAsyncTranslator<Domain.RefValue, ViewModels.RefValue> {
        public RefValueToRefValueTranslator(IGetRepository<IApplicationUser> repository) {
            Repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        IGetRepository<IApplicationUser> Repository { get; }

        public async Task<ViewModels.RefValue> Translate(Domain.RefValue source) {
            if (source == null)
                return null;

            var ApplicationUser = await Repository.GetItemAsync();
            var canaddnew = ApplicationUser.Roles.Any(x => x.Name == Roles.SystemAdministrator);
            return new ViewModels.RefValue(source.Id, source.Name, canaddnew);
        }
    }
}
