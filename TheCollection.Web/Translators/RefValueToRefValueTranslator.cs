namespace TheCollection.Web.Translators {
    using System.Linq;
    using TheCollection.Web.Contracts;

    public class RefValueToRefValueTranslator : ITranslator<Domain.RefValue, Models.RefValue> {
        public RefValueToRefValueTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public void Translate(Domain.RefValue source, Models.RefValue destination) {
            destination.id = source?.Id;
            destination.name = source?.Name;
            destination.canaddnew = ApplicationUser.Roles.Any(x => x.Name == "sysadmin");
        }
    }
}
