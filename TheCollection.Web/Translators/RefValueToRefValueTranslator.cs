namespace TheCollection.Web.Translators {
    using System.Linq;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;

    public class RefValueToRefValueTranslator : ITranslator<Domain.RefValue, Models.RefValue> {
        public RefValueToRefValueTranslator(IWebUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IWebUser ApplicationUser { get; }

        public void Translate(Domain.RefValue source, Models.RefValue destination) {
            destination.id = source?.Id;
            destination.name = source?.Name;
            destination.canaddnew = ApplicationUser.Roles.Any(x => x.Name == Roles.SystemAdministrator);
        }
    }
}
