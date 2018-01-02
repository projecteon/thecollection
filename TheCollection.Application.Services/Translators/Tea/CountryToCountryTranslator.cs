namespace TheCollection.Application.Services.Translators.Tea {
    using System.Linq;
    using TheCollection.Application.Services.Constants;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Tea;

    public class CountryToCountryTranslator : ITranslator<Country, ViewModels.Tea.Country> {
        public CountryToCountryTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public void Translate(Country source, ViewModels.Tea.Country destination) {
            destination.id = source.Id;
            destination.name = source.Name;
            destination.iseditable = ApplicationUser.Roles.Any(x => x.Name == Roles.SystemAdministrator);
        }
    }
}
