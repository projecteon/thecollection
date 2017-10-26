namespace TheCollection.Web.Translators {

    using System.Linq;
    using TheCollection.Web.Models;

    public class RefValueToRefValueTranslator : ITranslator<Business.RefValue, Models.RefValue> {

        public RefValueToRefValueTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        public IApplicationUser ApplicationUser { get; }

        public void Translate(Business.RefValue source, Models.RefValue destination) {
            destination.id = source?.Id;
            destination.name = source?.Name;
            destination.canaddnew = ApplicationUser.Roles.Any(x => x.Name == "sysadmin");
        }
    }
}
