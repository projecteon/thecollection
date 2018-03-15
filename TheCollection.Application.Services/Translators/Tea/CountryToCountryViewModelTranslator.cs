namespace TheCollection.Application.Services.Translators.Tea {
    using System.Linq;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Constants;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;

    public class CountryToCountryViewModelTranslator : IAsyncTranslator<Domain.Tea.Country, ViewModels.Tea.Country> {
        public CountryToCountryViewModelTranslator(IGetRepository<IApplicationUser> repository) {
            Repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        public IGetRepository<IApplicationUser> Repository { get; }

        public async Task<ViewModels.Tea.Country> Translate(Domain.Tea.Country source) {
            var applicationUser = await Repository.GetItemAsync();
            var isEditable = applicationUser.Roles.Any(x => x.Name == Roles.SystemAdministrator);
            return new ViewModels.Tea.Country(source.Id, source.Name, isEditable);
        }
    }
}
