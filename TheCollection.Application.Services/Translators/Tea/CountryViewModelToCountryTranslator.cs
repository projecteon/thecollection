namespace TheCollection.Application.Services.Translators.Tea {
    using TheCollection.Domain.Core.Contracts;

    public class CountryViewModelToCountryTranslator : ITranslator<ViewModels.Tea.Country, Domain.Tea.Country> {
        public CountryViewModelToCountryTranslator() {
        }

        public Domain.Tea.Country Translate(ViewModels.Tea.Country source) {
            return new Domain.Tea.Country(source.Id, source.Name);
        }
    }
}
