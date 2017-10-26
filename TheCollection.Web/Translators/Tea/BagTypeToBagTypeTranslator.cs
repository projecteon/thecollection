namespace TheCollection.Web.Translators.Tea {

    using System.Linq;
    using TheCollection.Business.Tea;
    using TheCollection.Web.Models;

    public class BagTypeToBagTypeTranslator : ITranslator<BagType, Models.Tea.BagType> {

        public BagTypeToBagTypeTranslator(IApplicationUser applicationUser) {
            ApplicationUser = applicationUser;
        }

        public IApplicationUser ApplicationUser { get; }

        public void Translate(BagType source, Models.Tea.BagType destination) {
            destination.id = source.Id;
            destination.name = source.Name;
            destination.iseditable = ApplicationUser.Roles.Any(x => x.Name == "sysadmin");
        }
    }
}
