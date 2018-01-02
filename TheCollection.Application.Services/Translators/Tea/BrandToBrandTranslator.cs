namespace TheCollection.Application.Services.Translators.Tea {
    using System.Linq;
    using TheCollection.Application.Services.Constants;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Tea;

    public class BrandToBrandTranslator : ITranslator<Brand, ViewModels.Tea.Brand> {

        public BrandToBrandTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IApplicationUser ApplicationUser { get; }

        public void Translate(Brand source, ViewModels.Tea.Brand destination) {
            destination.id = source.Id;
            destination.name = source.Name;
            destination.iseditable = ApplicationUser.Roles.Any(x => x.Name == Roles.SystemAdministrator);
        }
    }
}
