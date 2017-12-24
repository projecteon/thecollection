namespace TheCollection.Web.Translators.Tea {
    using TheCollection.Web.Contracts;

    public class CountryDtoToCountryTranslator : ITranslator<Models.Tea.Country, Domain.Tea.Country> {
        public CountryDtoToCountryTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public void Translate(Models.Tea.Country source, Domain.Tea.Country destination) {
            destination.Name = source.name;
        }
    }
}
