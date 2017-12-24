namespace TheCollection.Web.Translators.Tea {

    using System.Linq;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;

    public class CountryToCountryTranslator : ITranslator<Country, Models.Tea.Country> {

        public CountryToCountryTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public void Translate(Country source, Models.Tea.Country destination) {
            destination.id = source.Id;
            destination.name = source.Name;
            destination.iseditable = ApplicationUser.Roles.Any(x => x.Name == Roles.SystemAdministrator);
        }
    }
}
