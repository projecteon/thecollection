namespace TheCollection.Application.Services.Translators {
    using System.Linq;
    using TheCollection.Application.Services.Constants;
    using TheCollection.Application.Services.Contracts;

    public class RefValueToRefValueTranslator : ITranslator<Domain.RefValue, ViewModels.RefValue> {
        public RefValueToRefValueTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public void Translate(Domain.RefValue source, ViewModels.RefValue destination) {
            destination.id = source?.Id;
            destination.name = source?.Name;
            destination.canaddnew = ApplicationUser.Roles.Any(x => x.Name == Roles.SystemAdministrator);
        }
    }
}
