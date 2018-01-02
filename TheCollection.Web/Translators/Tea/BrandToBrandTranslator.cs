namespace TheCollection.Web.Translators.Tea {
    using System.Linq;
    using TheCollection.Domain.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Contracts;

    public class BrandToBrandTranslator : ITranslator<Brand, Models.Tea.Brand> {

        public BrandToBrandTranslator(IWebUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        IWebUser ApplicationUser { get; }

        public void Translate(Brand source, Models.Tea.Brand destination) {
            destination.id = source.Id;
            destination.name = source.Name;
            destination.iseditable = ApplicationUser.Roles.Any(x => x.Name == Roles.SystemAdministrator);
        }
    }
}
