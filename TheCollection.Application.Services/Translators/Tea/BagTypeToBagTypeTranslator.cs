namespace TheCollection.Application.Services.Translators.Tea {
    using System.Linq;
    using TheCollection.Application.Services.Constants;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Tea;

    public class BagTypeToBagTypeTranslator : ITranslator<BagType, ViewModels.Tea.BagType> {
        public BagTypeToBagTypeTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public void Translate(BagType source, ViewModels.Tea.BagType destination) {
            destination.id = source.Id;
            destination.name = source.Name;
            destination.iseditable = ApplicationUser.Roles.Any(x => x.Name == Roles.SystemAdministrator);
        }
    }
}
