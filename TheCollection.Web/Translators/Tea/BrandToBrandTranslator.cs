namespace TheCollection.Web.Translators.Tea {

    using System.Linq;
    using TheCollection.Business.Tea;
    using TheCollection.Web.Models;

    public class BrandToBrandTranslator : ITranslator<Brand, Models.Tea.Brand> {

        public BrandToBrandTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        public IApplicationUser ApplicationUser { get; }

        public void Translate(Brand source, Models.Tea.Brand destination) {
            destination.id = source.Id;
            destination.name = source.Name;
            destination.iseditable = ApplicationUser.Roles.Any(x => x.Name == "sysadmin");
        }
    }
}
