namespace TheCollection.Application.Services.Translators.Tea {
    using TheCollection.Application.Services.Contracts;

    public class CountryDtoToCountryTranslator : ITranslator<ViewModels.Tea.Country, Domain.Tea.Country> {
        public CountryDtoToCountryTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public void Translate(ViewModels.Tea.Country source, Domain.Tea.Country destination) {
            destination.Name = source.name;
        }
    }
}
